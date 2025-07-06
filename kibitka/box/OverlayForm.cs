using kibitka;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows.Forms;

public class WowOverlay : Form
{
    private IntPtr _wowHwnd;
    private Timer _positionTimer;
    private List<PlayerData> _players = new List<PlayerData>();
    private ResourcePanelState _panelState = new ResourcePanelState();
    private Rectangle _titleBarRect;
    private Rectangle _collapseButtonRect;
    private static Dictionary<string, Image> _textureAtlas = new Dictionary<string, Image>();

    public WowOverlay(IntPtr wowHwnd)
    {
        _wowHwnd = wowHwnd;
        InitializeForm();
        SetupWindowStyle(); // Должен вызываться ПОСЛЕ создания окна
        InitializeTimer();

    }

    private void InitializeForm()
    {
        this.FormBorderStyle = FormBorderStyle.None;
        this.TopMost = true;
        this.ShowInTaskbar = false;
        this.BackColor = Color.Gray;
        this.TransparencyKey = Color.Gray;
        this.DoubleBuffered = true;
    }

    private void SetupWindowStyle()
    {
        const int WS_EX_LAYERED = 0x80000;
        const int WS_EX_TOOLWINDOW = 0x80;
        const int WS_EX_NOACTIVATE = 0x8000000;
        this.TransparencyKey = Color.Gray;
        NativeMethods.SetWindowLong(
            this.Handle,
            -20,
            (IntPtr)(WS_EX_LAYERED | WS_EX_TOOLWINDOW | WS_EX_NOACTIVATE)
        );

        // Устанавливаем прозрачность только для фона
        NativeMethods.SetLayeredWindowAttributes(
            this.Handle,
            Color.Gray.ToArgb(),
            255,
            NativeMethods.LWA_COLORKEY
        );
    }


    private void InitializeTimer()
    {
        _positionTimer = new Timer { Interval = 100 };
        _positionTimer.Tick += PositionTimer_Tick;
        _positionTimer.Start();
    }

    public static void PreloadTextures()
    {
        string[] folders = { "Herb", "Mine", "Other" };
        foreach (var folder in folders)
        {
            var fullPath = Path.Combine(Application.StartupPath, folder);
            if (Directory.Exists(fullPath))
            {
                foreach (var file in Directory.GetFiles(fullPath, "*.tga"))
                {
                    try
                    {
                        using (var bmp = TgaLoader.LoadTga(file))
                        {
                            var resized = new Bitmap(bmp, new Size(32, 32));
                            var textureName = Path.GetFileNameWithoutExtension(file);
                            _textureAtlas[textureName] = resized;
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error loading {file}: {ex.Message}");
                    }
                }
            }
        }
    }

    public void UpdatePlayers(List<PlayerData> newPlayers)
    {
        if (this.InvokeRequired)
        {
            this.Invoke(new Action(() => UpdatePlayers(newPlayers)));
            return;
        }
        _players = newPlayers;
        this.Invalidate();
    }

    private void PositionTimer_Tick(object sender, EventArgs e)
    { 
        // Добавляем проверку на уничтожение формы
        if (this.IsDisposed || Disposing)
        {
            _positionTimer?.Stop();
            return;
        }
        try { 
        if (_wowHwnd == IntPtr.Zero || !NativeMethods.IsWindow(_wowHwnd))
        {
            this.Close();
            return;
        }

        IntPtr activeWindow = NativeMethods.GetForegroundWindow();
        bool isWowActive = activeWindow == _wowHwnd;

        if (isWowActive)
        {
            NativeMethods.RECT windowRect;
            if (!NativeMethods.GetWindowRect(_wowHwnd, out windowRect))
            {
                if (this.Visible) this.Hide();
                return;
            }

            NativeMethods.RECT clientRect;
            if (!NativeMethods.GetClientRect(_wowHwnd, out clientRect))
            {
                if (this.Visible) this.Hide();
                return;
            }

            int clientWidth = clientRect.Right - clientRect.Left;
            int clientHeight = clientRect.Bottom - clientRect.Top;

            if (clientWidth == 0 || clientHeight == 0)
            {
                if (this.Visible) this.Hide();
                return;
            }

            int fullWidth = windowRect.Right - windowRect.Left;
            int fullHeight = windowRect.Bottom - windowRect.Top;

            int borderWidth = fullWidth - clientWidth;
            int borderHeight = fullHeight - clientHeight;

            int leftRightBorder = borderWidth / 2;
            int topBorder = borderHeight - leftRightBorder;

            int overlayX = windowRect.Left + leftRightBorder;
            int overlayY = windowRect.Top + topBorder;

            this.Location = new Point(overlayX, overlayY);
            this.Size = new Size(clientWidth, clientHeight);

            if (!this.Visible)
            {
                if (!this.IsDisposed && !this.Disposing)
                    this.Show();
            }
            this.Invalidate();
        }
        else
        {
            if (this.Visible)
            {
                if (!this.IsDisposed && !this.Disposing)
                    this.Hide();
            }
        }
    } catch (ObjectDisposedException)
        {
            _positionTimer?.Stop();
        }
    }
    protected override void WndProc(ref Message m)
    {
        const int WM_NCHITTEST = 0x84;
        const int HTTRANSPARENT = -1;
        const int HTCLIENT = 1;

        if (m.Msg == WM_NCHITTEST)
        {
            Point mousePos = new Point(
                (int)(m.LParam.ToInt64() & 0xFFFF),
                (int)((m.LParam.ToInt64() >> 16) & 0xFFFF)
            );
            mousePos = PointToClient(mousePos);

            // Если клик внутри панели - обрабатываем
            if (_panelState.PanelBounds.Contains(mousePos))
            {
                m.Result = (IntPtr)HTCLIENT;
                return;
            }

            // Все остальные клики пропускаем
            m.Result = (IntPtr)HTTRANSPARENT;
            return;
        }

        base.WndProc(ref m);
    }

    protected override void OnFormClosing(FormClosingEventArgs e)
    {
        StopPositionTimer();
        base.OnFormClosing(e);
    }

    protected override void OnFormClosed(FormClosedEventArgs e)
    {
        foreach (var texture in _textureAtlas.Values) texture.Dispose();
        _textureAtlas.Clear();
        base.OnFormClosed(e);
    }

    protected override void OnPaint(PaintEventArgs e)
    {
        base.OnPaint(e);

        DrawVisibleObjects(e.Graphics);
        DrawOffScreenPanel(e.Graphics);
    }

    private void DrawVisibleObjects(Graphics g)
    {
        foreach (var player in _players.Where(p => !p.IsOffScreen))
        {
            if (string.IsNullOrEmpty(player.TexturePath)) continue;

            int iconSize;
            if (player.Distance <= 50)
            {
                iconSize = 16;
            }
            else if (player.Distance <= 100)
            {
                iconSize = 24;
            }
            else
            {
                iconSize = 32;
            }
            var textureName = Path.GetFileNameWithoutExtension(player.TexturePath);
            if (_textureAtlas.TryGetValue(textureName, out Image image))
            {
                int x = player.ScreenPosition.X - iconSize / 2;
                int y = player.ScreenPosition.Y - iconSize / 2;
                g.DrawImage(image, new Rectangle(x, y, iconSize, iconSize));

                using (Font font = new Font("Arial", 10, FontStyle.Bold))
                using (SolidBrush brush = new SolidBrush(Color.White))
                {
                    string text = player.Name;
                    SizeF textSize = g.MeasureString(text, font);
                    float textX = x + (iconSize - textSize.Width) / 2;
                    float textY = y + iconSize + 2;
                    g.DrawString(text, font, brush, textX, textY);
                }
            }
        }
    }

    private void DrawOffScreenPanel(Graphics g)
    {
        var offScreenList = _players.Where(p => p.IsOffScreen).ToList();
        if (offScreenList.Count == 0) return;

        if (_panelState.IsCollapsed)
            DrawCollapsedPanel(g, offScreenList.Count);
        else
            DrawExpandedPanel(g, offScreenList);
    }

    private void DrawExpandedPanel(Graphics g, List<PlayerData> offScreenList)
    {
        // Рассчитываем адаптивные размеры
        int safetyMargin = 10;
        int itemHeight = 36;
        Font itemFont = new Font("Arial", 9);
        Font indicatorFont = new Font("Arial", 8, FontStyle.Italic);

        // 1. Определяем ширину панели
        int panelWidth = Math.Max(120, Math.Min((int)(this.ClientSize.Width * 0.3), 250));

        // 2. Ограничиваем количество отображаемых элементов
        int maxVisibleItems = Math.Max(3, (int)((this.ClientSize.Height * 0.7) / itemHeight));
        int visibleCount = Math.Min(offScreenList.Count, maxVisibleItems);

        // 3. Рассчитываем высоту панели
        int panelHeight = 24 + (visibleCount * itemHeight) + 12 + (offScreenList.Count > maxVisibleItems ? 20 : 0);
        _panelState.Size = new Size(panelWidth, panelHeight);

        // 4. Корректируем позицию, чтобы не выходила за границы
        _panelState.Position = new Point(
            Math.Min(_panelState.Position.X, this.ClientSize.Width - panelWidth - safetyMargin),
            Math.Min(_panelState.Position.Y, this.ClientSize.Height - panelHeight - safetyMargin)
        );

        // 5. Отрисовываем фон
        using (GraphicsPath path = DrawingUtils.GenerateRoundedRectangle(
            new RectangleF(_panelState.Position.X, _panelState.Position.Y, panelWidth, panelHeight), 12))
        using (SolidBrush brush = new SolidBrush(Color.FromArgb(200, 40, 40, 45)))
        {
            g.FillPath(brush, path);
        }

        // 6. Заголовок панели
        _titleBarRect = new Rectangle(_panelState.Position.X, _panelState.Position.Y, panelWidth, 24);
        using (Font titleFont = new Font("Arial", 10, FontStyle.Bold))
        {
            string cringe = CultureInfo.CurrentCulture.Name.StartsWith("ru") ? "Ресурсы рядом" : "Resource near";
            string title = cringe;
            Rectangle textRect = new Rectangle(
                _panelState.Position.X + 8,
                _panelState.Position.Y + 4,
                panelWidth - 32,
                20
            );
            TextRenderer.DrawText(g, title, titleFont, textRect, Color.White,
                TextFormatFlags.Left | TextFormatFlags.EndEllipsis);
        }

        // 7. Кнопка сворачивания
        _collapseButtonRect = new Rectangle(
            _panelState.Position.X + panelWidth - 28,
            _panelState.Position.Y + 4,
            20, 20
        );
        g.DrawString("—", new Font("Arial", 12, FontStyle.Bold), Brushes.White, _collapseButtonRect);

        // 8. Список элементов
        int currentY = _panelState.Position.Y + 28;
        foreach (var resource in offScreenList.Take(visibleCount))
        {
            // Определяем размер иконки
            int iconSize;
            if (resource.Distance <= 50)
            {
                iconSize = 32;
            }
            else if (resource.Distance <= 100)
            {
                iconSize = 24;
            }
            else
            {
                iconSize = 16;
            }

            // Отрисовываем иконку
            if (_textureAtlas.TryGetValue(Path.GetFileNameWithoutExtension(resource.TexturePath), out Image icon))
            {
                int iconY = currentY + (itemHeight - iconSize) / 2;
                g.DrawImage(icon, _panelState.Position.X + 8, iconY, iconSize, iconSize);

                // Текст с расстоянием
                string text = $"{resource.Name} ({(int)resource.Distance}m)";
                Rectangle textRect = new Rectangle(
                    _panelState.Position.X + iconSize + 16,
                    iconY,
                    panelWidth - iconSize - 24,
                    iconSize
                );

                using (var format = new StringFormat()
                {
                    LineAlignment = StringAlignment.Center,
                    Trimming = StringTrimming.EllipsisCharacter
                })
                {
                    g.DrawString(text, itemFont, Brushes.White, textRect, format);
                }
            }
            currentY += itemHeight;
        }

        // 9. Индикатор скрытых элементов
        if (offScreenList.Count > visibleCount)
        {
            int remaining = offScreenList.Count - visibleCount;
            string indicatorText = $"+{remaining} ещё...";
            Rectangle indicatorRect = new Rectangle(
                _panelState.Position.X + 8,
                currentY + 4,
                panelWidth - 16,
                16
            );
            TextRenderer.DrawText(g, indicatorText, indicatorFont, indicatorRect,
                Color.LightGray, TextFormatFlags.Right | TextFormatFlags.EndEllipsis);
        }

        // 10. Обновляем границы панели
        _panelState.PanelBounds = new Rectangle(
            _panelState.Position.X,
            _panelState.Position.Y,
            panelWidth,
            panelHeight
        );

        // Очищаем ресурсы
        itemFont.Dispose();
        indicatorFont.Dispose();
    }
    private void DrawCollapsedPanel(Graphics g, int resourcesCount)
    {
        // 1. Определяем ширину панели
        int panelWidth = Math.Max(120, Math.Min((int)(this.ClientSize.Width * 0.3), 250));

        int panelHeight = 24;
        _panelState.Size = new Size(panelWidth, panelHeight); // Обновляем размер

        // Background
        using (GraphicsPath path = DrawingUtils.GenerateRoundedRectangle(
            new RectangleF(_panelState.Position.X, _panelState.Position.Y, panelWidth, panelHeight), 6))
        using (SolidBrush brush = new SolidBrush(Color.FromArgb(160, 60, 60, 60)))
        {
            g.FillPath(brush, path);
        }

        // Title
        using (Font font = new Font("Arial", 10, FontStyle.Bold))
        {
            string cringe = CultureInfo.CurrentCulture.Name.StartsWith("ru") ? "Ресурсы рядом" : "Resource near";
            string title = cringe + $" ({resourcesCount})";
            g.DrawString(title, font, Brushes.White, _panelState.Position.X + 6, _panelState.Position.Y + 4);
        }

        // Expand button
        _collapseButtonRect = new Rectangle(
            _panelState.Position.X + panelWidth - 24,
            _panelState.Position.Y + 4,
            16, 16);
        using (Font font = new Font("Arial", 10, FontStyle.Bold))
        {
            g.DrawString("+", font, Brushes.White, _collapseButtonRect.X, _collapseButtonRect.Y);
        }

        // Задаем область заголовка для перемещения
        _titleBarRect = new Rectangle(_panelState.Position.X, _panelState.Position.Y, panelWidth, panelHeight);
        _panelState.PanelBounds = new Rectangle(
            _panelState.Position.X,
            _panelState.Position.Y,
            panelWidth,
            panelHeight
        );
    }

    public void StopPositionTimer()
    {
        if (_positionTimer != null)
        {
            _positionTimer.Stop();
            _positionTimer.Dispose();
            _positionTimer = null;
        }
    }


    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            StopPositionTimer();
            if (_positionTimer != null)
            {
                _positionTimer.Dispose();
                _positionTimer = null;
            }
        }
        base.Dispose(disposing);
    }
    protected override void OnMouseDown(MouseEventArgs e)
    {
        // Сначала проверяем клик на кнопке сворачивания/разворачивания
        if (_collapseButtonRect.Contains(e.Location))
        {
            _panelState.IsCollapsed = !_panelState.IsCollapsed;
            Invalidate();
        }
        // Затем проверяем клик на заголовке для перетаскивания
        else if (_titleBarRect.Contains(e.Location))
        {
            _panelState.IsDragging = true;
            _panelState.DragStartPosition = new Point(e.X - _panelState.Position.X, e.Y - _panelState.Position.Y);
        }
        base.OnMouseDown(e);
    }

    protected override void OnMouseMove(MouseEventArgs e)
    {
        if (_panelState.IsDragging)
        {
            Point newPos = new Point(
                e.X - _panelState.DragStartPosition.X,
                e.Y - _panelState.DragStartPosition.Y);

            newPos.X = Math.Max(0, Math.Min(newPos.X, ClientSize.Width - _panelState.Size.Width));
            newPos.Y = Math.Max(0, Math.Min(newPos.Y, ClientSize.Height - _panelState.Size.Height));

            _panelState.Position = newPos;
            Invalidate();
        }
        base.OnMouseMove(e);
    }

    protected override void OnMouseUp(MouseEventArgs e)
    {
        _panelState.IsDragging = false;
        base.OnMouseUp(e);
    }
}

internal static class NativeMethods
{
    [DllImport("user32.dll", SetLastError = true)]
    internal static extern bool GetWindowRect(IntPtr hWnd, out RECT lpRect);

    [DllImport("user32.dll", SetLastError = true)]
    internal static extern bool GetClientRect(IntPtr hWnd, out RECT lpRect);

    [DllImport("user32.dll", SetLastError = true)]
    internal static extern IntPtr GetWindowLong(IntPtr hWnd, int nIndex);

    [DllImport("user32.dll", SetLastError = true)]
    internal static extern IntPtr SetWindowLong(IntPtr hWnd, int nIndex, IntPtr dwNewLong);

    [StructLayout(LayoutKind.Sequential)]
    internal struct RECT
    {
        public int Left;
        public int Top;
        public int Right;
        public int Bottom;
    }
    [DllImport("user32.dll")]
    internal static extern bool SetLayeredWindowAttributes(
    IntPtr hwnd,
    int crKey,
    byte bAlpha,
    int dwFlags
    );

    internal const int LWA_COLORKEY = 0x1;
    [DllImport("user32.dll")]
    internal static extern IntPtr GetForegroundWindow();

    [DllImport("user32.dll")]
    [return: MarshalAs(UnmanagedType.Bool)]
    internal static extern bool IsWindow(IntPtr hWnd);
}

internal static class DrawingUtils
{
    public static GraphicsPath GenerateRoundedRectangle(RectangleF rect, float radius)
    {
        float diameter = radius * 2f;
        GraphicsPath path = new GraphicsPath();

        path.AddArc(rect.X, rect.Y, diameter, diameter, 180, 90);
        path.AddArc(rect.Right - diameter, rect.Y, diameter, diameter, 270, 90);
        path.AddArc(rect.Right - diameter, rect.Bottom - diameter, diameter, diameter, 0, 90);
        path.AddArc(rect.X, rect.Bottom - diameter, diameter, diameter, 90, 90);

        path.CloseFigure();
        return path;
    }
}

public class ResourcePanelState
{
    public Point Position { get; set; } = new Point(10, 40);
    public bool IsCollapsed { get; set; }
    public Size Size { get; set; } = new Size(200, 150);
    public bool IsDragging { get; set; }
    public Point DragStartPosition { get; set; }
    public Region PanelRegion { get; set; }
    public Rectangle PanelBounds { get; set; }
}