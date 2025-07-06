using kibitka;
using kibitka.box;
using kibitka.box.DB;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace kibitka
{
    public partial class Form1 : Form
    {   // Import required Windows API functions
        [DllImport("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);

        [DllImport("user32.dll")]
        public static extern bool ReleaseCapture();

        // Constants for Windows messages
        private const int WM_NCLBUTTONDOWN = 0xA1;
        private const int HT_CAPTION = 0x2;
        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

        private Memory _memory = new Memory();
        private Process_Stuff _processStuff = new Process_Stuff();
        private WoWCamera _camera;
        private WowOverlay _overlay;
        private Scanner _scanner;

        public PlayerObject MyPlayer { get; } = new PlayerObject();
        public DB Db { get; } = new DB();
        [DllImport("Gdi32.dll", EntryPoint = "CreateRoundRectRgn")]
        private static extern IntPtr CreateRoundRectRgn
        (
            int nLeftRect, // x-coordinate of upper-left corner
            int nTopRect, // y-coordinate of upper-left corner
            int nRightRect, // x-coordinate of lower-right corner
            int nBottomRect, // y-coordinate of lower-right corner
            int nWidthEllipse, // width of ellipse
            int nHeightEllipse // height of ellipse
        );

        public Form1()
        {
            InitializeComponent();
            this.FormBorderStyle = FormBorderStyle.None;
            Region = System.Drawing.Region.FromHrgn(CreateRoundRectRgn(0, 0, Width, Height, 20, 20));
            MouseDown += _MouseDown;
        }
        private void _MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                ReleaseCapture();
                SendMessage(Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);
            }
        }
        private void open_Click(object sender, EventArgs e)
        {
            pidbox.Enabled = false;
            open.Enabled = false;
            if (!int.TryParse(pidbox.Text.Trim(), out int processId))
            {
                pidbox.Enabled = true;
                open.Enabled = true;
                MessageBox.Show("Введите корректный PID.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            try
            {
                var process = Process.GetProcessById(processId);
                _memory.SetProcess(process, Memory.Mode.READ);
                InitializeAddresses();

                _camera = new WoWCamera(_memory);
                _scanner = new Scanner(_memory, _processStuff, _camera, Db, MyPlayer, this);

                var hWnd = _memory.ProcessToRead.MainWindowHandle;
                //Console.WriteLine(hWnd);
                _overlay = new WowOverlay(hWnd);
                _overlay.Show();

                timer.Enabled = true;
                pidbox.Enabled = false;
                open.Enabled = false;
            }
            catch (Exception ex)
            {
                pidbox.Enabled = true;
                open.Enabled = true;
                MessageBox.Show($"Ошибка подключения: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void InitializeAddresses()
        {
            _processStuff.ClientConnection = _memory.ReadPointer(Client.StaticClientConnection);
            _processStuff.ObjectManager = _memory.ReadPointer(_processStuff.ClientConnection + Client.ObjectManagerOffset);
            MyPlayer.Guid = _memory.ReadUInt64(_processStuff.ObjectManager + Client.LocalGuidOffset);
        }
        public bool IsOreBoxChecked => OreBox.Checked;
        public bool IsHerbBoxChecked => HerbBox.Checked;
        public bool IsContainerBoxChecked => ContainerBox.Checked;
        public bool IsRareBoxChecked => RareBox.Checked;

        private void timer_Tick(object sender, EventArgs e)
        {
            try
            {
                if (_overlay == null || _overlay.IsDisposed) return;

                var visiblePlayers = _scanner.Update();
                FilterAndUpdateOverlay(visiblePlayers);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка таймера: {ex}");
            }
        }

        private void FilterAndUpdateOverlay(List<PlayerData> players)
        {
            var filtered = new List<PlayerData>();
            foreach (var player in players)
            {
                if (player.ScreenPosition.X >= 0 && player.ScreenPosition.X < _overlay.ClientSize.Width &&
                    player.ScreenPosition.Y >= 0 && player.ScreenPosition.Y < _overlay.ClientSize.Height)
                {
                    filtered.Add(player);
                }
            }
            _overlay.UpdatePlayers(filtered);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }

            if (disposing)
            {
                _overlay?.Dispose();
                components?.Dispose();
            }
            base.Dispose(disposing);
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            Process.GetCurrentProcess().Kill();
            Environment.Exit(0);
        }

        private void ex_Click(object sender, EventArgs e)
        {
            Process.GetCurrentProcess().Kill();
            Environment.Exit(0);
        }

        //private ParticleSystem particleSystem;
        private void Form1_Load(object sender, EventArgs e)
        {
            WowOverlay.PreloadTextures();
            LabL.Text = "Welcome user";
            LabL.Text += "! \nType wow process id";
        }

        private void stop_click(object sender, EventArgs e)
        {
            // Останавливаем все таймеры
            if (_overlay != null)
            {
                _overlay.StopPositionTimer();
            }

            timer.Enabled = false;
            pidbox.Enabled = true;
            open.Enabled = true;

            if (_overlay != null)
            {
                if (!_overlay.IsDisposed)
                {
                    // Убираем отписку от несуществующего события
                    _overlay.Dispose();
                }
                _overlay = null;
            }
        }

        private void OreBox_CheckedChanged(object sender, EventArgs e)
        {

        }
    }
}