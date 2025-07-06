using SharpDX;
using System;
using System.Reflection;
using System.Runtime.InteropServices;

namespace kibitka.box
{

    internal static class Pointers
    {
        internal class Drawing
        {
            internal static uint WorldFrame = 0x00B7436C;
            internal static uint ActiveCamera = 0x7E20;
            //internal static uint RenderBackground = 0x2532E0;
        }
    }
    public class WoWCamera
    {
        [StructLayout(LayoutKind.Sequential)]
        public unsafe struct CGCamera
        {
            public IntPtr vTable;           // 0x0
            private int unk0;               // 0x4
            public Vector3 Position;        // 0x8
            public fixed float Facing[9];   // 0x14 (3x3 Matrix)
            public float NearClip;          // 0x38
            public float FarClip;           // 0x3C
            public float FieldOfView;       // 0x40
            public float Aspect;            // 0x44
        }


        private readonly Memory _memory;
        private readonly IntPtr _hWnd;
        public int scX = 0;
        public int scY = 0;

        public WoWCamera(Memory memory)
        {
            _memory = memory;
            _hWnd = _memory.ProcessToRead.MainWindowHandle;
            UpdateScreenDimensions();
        }

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

        private void UpdateScreenDimensions()
        {
            if (_hWnd == IntPtr.Zero) return;

            NativeMethods.RECT clientRect;
            NativeMethods.GetClientRect(_hWnd, out clientRect);

            scX = Math.Max(1, clientRect.Right - clientRect.Left);
            scY = Math.Max(1, clientRect.Bottom - clientRect.Top);
        }

        public unsafe bool WorldToScreen(float x, float y, float z, out Vector2 screenPos)
        {
            screenPos = Vector2.Zero;
            try
            {
                // Получение данных камеры
                IntPtr worldFramePtr = _memory.ReadPointer((IntPtr)Pointers.Drawing.WorldFrame);
                IntPtr cameraPtr = _memory.ReadPointer(worldFramePtr + (int)Pointers.Drawing.ActiveCamera);
                CGCamera cameraData = _memory.Read<CGCamera>(cameraPtr);

                // Проверка валидности данных
                if (!IsValidCameraData(cameraData))
                {
                    Console.WriteLine("Invalid camera data");
                    return false;
                }

                // Построение матриц
                Matrix view = CreateViewMatrix(ref cameraData);
                Matrix projection = CreateProjectionMatrix(ref cameraData);

                // Преобразование координат
                Vector3 worldPos = new Vector3(x, y, z);
                Vector3 screenCoord = Vector3.Project(
                    worldPos,
                    0, 0,
                    scX, scY,
                    cameraData.NearClip,
                    cameraData.FarClip,
                    view * projection
                );
                Vector3 cameraForward = new Vector3(cameraData.Facing[0], cameraData.Facing[1], cameraData.Facing[2]);
                cameraForward.Normalize();
                Vector3 objectDirection = worldPos - cameraData.Position;
                objectDirection.Normalize();

                float dotProduct = Vector3.Dot(cameraForward, objectDirection);
                dotProduct = Math.Max(-1.0f, Math.Min(1.0f, dotProduct));
                float angle = (float)Math.Acos(dotProduct);

                if (angle > (cameraData.FieldOfView * 0.6f))
                {
                    return false;
                }

                // Проверка результата
                bool check = IsValidScreenCoord(screenCoord);
                Console.WriteLine(check+" Coords: "+ screenCoord);
                if (check)
                {
                    screenPos = new Vector2(screenCoord.X, screenCoord.Y);
 
                    return true;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"WorldToScreen error: {ex.Message}");
            }
            return false;
        }

        public System.Numerics.Vector2 GetObjectScreenPosition(float worldX, float worldY, float worldZ)
        {
            UpdateScreenDimensions();
            if (WorldToScreen(worldX, worldY, worldZ, out Vector2 screenPos))
            {
                return new System.Numerics.Vector2(screenPos.X, screenPos.Y);
            }
            return System.Numerics.Vector2.Zero;
        }

        private unsafe Matrix CreateViewMatrix(ref CGCamera cameraData)
        {
            Vector3 eye = cameraData.Position;
            Vector3 forward = new Vector3(cameraData.Facing[0], cameraData.Facing[1], cameraData.Facing[2]);
            forward.Normalize();
            return Matrix.LookAtRH(eye, eye + forward, new Vector3(0, 0, 1));
        }

        private Matrix CreateProjectionMatrix(ref CGCamera cameraData)
        {
            return Matrix.PerspectiveFovRH(
                cameraData.FieldOfView*0.6f,
                (float)scX / scY,
                cameraData.NearClip,
                cameraData.FarClip
            );
        }
        public Vector3 GetCameraPosition()
        {
            IntPtr worldFramePtr = _memory.ReadPointer((IntPtr)Pointers.Drawing.WorldFrame);
            IntPtr cameraPtr = _memory.ReadPointer(worldFramePtr + (int)Pointers.Drawing.ActiveCamera);
            CGCamera cameraData = _memory.Read<CGCamera>(cameraPtr);
            return cameraData.Position;
        }

        public unsafe Vector3 GetCameraForward()
        {
            IntPtr worldFramePtr = _memory.ReadPointer((IntPtr)Pointers.Drawing.WorldFrame);
            IntPtr cameraPtr = _memory.ReadPointer(worldFramePtr + (int)Pointers.Drawing.ActiveCamera);
            CGCamera cameraData = _memory.Read<CGCamera>(cameraPtr);
            return new Vector3(cameraData.Facing[0], cameraData.Facing[1], cameraData.Facing[2]);
        }
        private bool IsValidCameraData(CGCamera data)
        {
            return !float.IsNaN(data.Position.X) &&
                   !float.IsNaN(data.FieldOfView) &&
                   data.NearClip > 0 &&
                   data.FarClip > data.NearClip;
        }

        private bool IsValidScreenCoord(Vector3 coord)
        {
            return !float.IsNaN(coord.X) &&
                   !float.IsNaN(coord.Y) &&
                   coord.Z >= 0.1f &&
                   //coord.Z <= 1000.0f && // bleh
                   coord.X >= 0 &&
                   coord.X <= scX &&
                   coord.Y >= 0 &&
                   coord.Y <= scY;
        }

    }

    internal static class NativeMethods
    {
        [StructLayout(LayoutKind.Sequential)]
        internal struct RECT
        {
            public int Left;
            public int Top;
            public int Right;
            public int Bottom;
        }

        [DllImport("user32.dll")]
        internal static extern bool GetClientRect(IntPtr hWnd, out RECT rect);
    }

}