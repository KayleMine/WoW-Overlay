using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

// sad .cs file under

namespace kibitka.box
{
    public class Memory
    {
        public Process ProcessToRead;
        private IntPtr ProcessHwnd;

        public enum Mode : uint
        {
            READ = 0x10,
            WRITE = 0x20,
            BOTH = 0x30,
            ALL = 0x1f0fff,
        }

        public string PlayerNameFromGuid(ulong guid)
        {
            ulong mask, base_, offset, current, shortGUID, testGUID;

            mask = ReadUInt32(box.Name.NameStore + box.Name.NameMask);
            base_ = ReadUInt32(box.Name.NameStore + box.Name.NameBase);

            shortGUID = guid & 0xffffffff;
            offset = 12 * (mask & shortGUID);

            current = ReadUInt32((IntPtr)(base_ + offset + 8));
            offset = ReadUInt32((IntPtr)(base_ + offset));

            if ((current & 0x1) == 0x1) { return ""; }

            testGUID = ReadUInt32((IntPtr)current);

            while (testGUID != shortGUID)
            {
                current = ReadUInt32((IntPtr)(current + offset + 4));

                if ((current & 0x1) == 0x1) { return ""; }
                testGUID = ReadUInt32((IntPtr)(current));
            }


            //Читаем имя игрока
            string readedName = ReadUTF8String((IntPtr)(current + box.Name.NameString), 26);

            //Возвращяем прочитаный ник
            return readedName;
        }

        public T Read<T>(IntPtr address) where T : struct
        {
            int size = Marshal.SizeOf<T>();
            byte[] buffer = new byte[size];
            IntPtr bytesRead;

            IntPtr targetAddress = new IntPtr(address.ToInt64());

            if (MemoryApi.ReadProcessMemory(this.ProcessHwnd, targetAddress, buffer, (uint)size, out bytesRead) != 0
                && bytesRead.ToInt32() == size)
            {
                GCHandle handle = GCHandle.Alloc(buffer, GCHandleType.Pinned);
                try
                {
                    return Marshal.PtrToStructure<T>(handle.AddrOfPinnedObject());
                }
                finally
                {
                    handle.Free();
                }
            }
            return default(T);
        }

        public IntPtr ReadPointer(IntPtr address)
        {
            return Read<IntPtr>(address);
        }

        public int WriteUInt32(IntPtr memoryAddress, uint data)
        {
            return WriteProcessMemory(memoryAddress, BitConverter.GetBytes(data));
        }

        public int WriteUInt(IntPtr memoryAddress, uint data)
        {
            return WriteProcessMemory(memoryAddress, BitConverter.GetBytes(data));
        }

        public int WriteInt32(IntPtr memoryAddress, int data)
        {
            return WriteProcessMemory(memoryAddress, BitConverter.GetBytes(data));
        }

        public int WriteInt(IntPtr memoryAddress, int data)
        {
            return WriteProcessMemory(memoryAddress, BitConverter.GetBytes(data));
        }

        public int WriteUInt64(IntPtr memoryAddress, ulong data)
        {
            return WriteProcessMemory(memoryAddress, BitConverter.GetBytes(data));
        }

        public int WriteULong(IntPtr memoryAddress, ulong data)
        {
            return WriteProcessMemory(memoryAddress, BitConverter.GetBytes(data));
        }

        public int WriteInt64(IntPtr memoryAddress, long data)
        {
            return WriteProcessMemory(memoryAddress, BitConverter.GetBytes(data));
        }

        public int WriteLong(IntPtr memoryAddress, long data)
        {
            return WriteProcessMemory(memoryAddress, BitConverter.GetBytes(data));
        }

        public int WriteFloat(IntPtr memoryAddress, float data)
        {
            return WriteProcessMemory(memoryAddress, BitConverter.GetBytes(data));
        }

        public int WriteDouble(IntPtr memoryAddress, double data)
        {
            return WriteProcessMemory(memoryAddress, BitConverter.GetBytes(data));
        }

        public int WriteUTF8String(IntPtr memoryAddress, string data)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(data + "\0");
            return WriteProcessMemory(memoryAddress, bytes);
        }

        private int WriteProcessMemory(IntPtr memoryAddress, byte[] buffer)
        {
            uint size = (uint)buffer.Length;
            if (ProcessToRead == null)
                throw new InvalidOperationException("Process not initialized");

            IntPtr bytesWritten;
            return MemoryApi.WriteProcessMemory(ProcessHwnd, memoryAddress, buffer, size, out bytesWritten);
        }

        public bool SetProcess(object process, Mode mode)
        {
            if (process is Process proc)
                ProcessToRead = proc;
            else if (process is int pid)
                ProcessToRead = Process.GetProcessById(pid);
            else if (process is string processName)
            {
                Process[] processes = Process.GetProcessesByName(processName);
                if (processes.Length == 0) return false;
                ProcessToRead = processes[0];
            }
            else
                return false;

            if (!SetDebugPrivilege())
                return false;

            OpenProcess((uint)mode);
            return true;
        }

        private void OpenProcess(uint mode)
        {
            if (ProcessToRead == null)
                throw new ArgumentNullException("Process to open is null");

            ProcessHwnd = MemoryApi.OpenProcess(mode, 1, (uint)ProcessToRead.Id);
        }

        public bool CloseProcess()
        {
            if (MemoryApi.CloseHandle(ProcessHwnd) == false)
                return false;

            ProcessToRead = null;
            ProcessHwnd = IntPtr.Zero;
            return true;
        }

        public int IsReady()
        {
            if (ProcessToRead == null) return 0;
            return ProcessToRead.HasExited ? -1 : 1;
        }

        private bool SetDebugPrivilege()
        {
            IntPtr token;
            if (!MemoryApi.OpenProcessToken(MemoryApi.GetCurrentProcess(), 0x28, out token))
                return false;

            try
            {
                MemoryApi.LUID luid;
                if (!MemoryApi.LookupPrivilegeValue("", "SeDebugPrivilege", out luid))
                    return false;

                MemoryApi.TOKEN_PRIVILEGES tp = new MemoryApi.TOKEN_PRIVILEGES
                {
                    PrivilegeCount = 1,
                    Luid = luid,
                    Attributes = 0x2
                };

                return MemoryApi.AdjustTokenPrivileges(token, false, ref tp, 0, IntPtr.Zero, IntPtr.Zero);
            }
            finally
            {
                MemoryApi.CloseHandle(token);
            }
        }

        public byte[] ReadRaw(IntPtr memoryAddress, uint len)
        {
            return ReadProcessMemory(memoryAddress, len);
        }

        public string ReadString(IntPtr memoryAddress, uint len)
        {
            byte[] bytes = ReadProcessMemory(memoryAddress, len);
            return Encoding.ASCII.GetString(bytes.TakeWhile(b => b != 0).ToArray());
        }

        public string ReadUTF8String(IntPtr memoryAddress, uint maxLength)
        {
            List<byte> bytes = new List<byte>();
            byte currentByte;

            do
            {
                currentByte = ReadByte(memoryAddress + bytes.Count);
                if (currentByte != 0) bytes.Add(currentByte);
            } while (currentByte != 0 && bytes.Count < maxLength);

            return Encoding.UTF8.GetString(bytes.ToArray());
        }


        public float[] SwedReadMatrix(IntPtr address)
        {
            var bytes = ReadBytes(address, 4 * 16);
            var matrix = new float[bytes.Length];

            matrix[0] = BitConverter.ToSingle(bytes, 0 * 4);
            matrix[1] = BitConverter.ToSingle(bytes, 1 * 4);
            matrix[2] = BitConverter.ToSingle(bytes, 2 * 4);
            matrix[3] = BitConverter.ToSingle(bytes, 3 * 4);

            matrix[4] = BitConverter.ToSingle(bytes, 4 * 4);
            matrix[5] = BitConverter.ToSingle(bytes, 5 * 4);
            matrix[6] = BitConverter.ToSingle(bytes, 6 * 4);
            matrix[7] = BitConverter.ToSingle(bytes, 7 * 4);

            matrix[8] = BitConverter.ToSingle(bytes, 8 * 4);
            matrix[9] = BitConverter.ToSingle(bytes, 9 * 4);
            matrix[10] = BitConverter.ToSingle(bytes, 10 * 4);
            matrix[11] = BitConverter.ToSingle(bytes, 11 * 4);

            matrix[12] = BitConverter.ToSingle(bytes, 12 * 4);
            matrix[13] = BitConverter.ToSingle(bytes, 13 * 4);
            matrix[14] = BitConverter.ToSingle(bytes, 14 * 4);
            matrix[15] = BitConverter.ToSingle(bytes, 15 * 4);

            return matrix;

        }
        public float[,] ReadMatrix(IntPtr baseAddress, int rows, int columns)
        {
            // Calculate total elements and bytes needed
            int totalElements = rows * columns;
            int bytesToRead = totalElements * sizeof(float);

            // Read raw bytes from memory
            byte[] bytes = ReadRaw(baseAddress, (uint)bytesToRead);

            // Convert bytes to a flat float array
            float[] flatArray = new float[totalElements];
            Buffer.BlockCopy(bytes, 0, flatArray, 0, bytes.Length);

            // Reshape into a 2D matrix (row-major order)
            float[,] matrix = new float[rows, columns];
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < columns; j++)
                {
                    matrix[i, j] = flatArray[i * columns + j];
                }
            }

            return matrix;
        }
        public float ReadFloat(IntPtr address) => BitConverter.ToSingle(ReadProcessMemory(address, 4), 0);
        public short ReadShort(IntPtr address) => BitConverter.ToInt16(ReadProcessMemory(address, 2), 0);
        public ushort ReadUShort(IntPtr address) => BitConverter.ToUInt16(ReadProcessMemory(address, 2), 0);
        public uint ReadUInt32(IntPtr address) => BitConverter.ToUInt32(ReadProcessMemory(address, 4), 0);
        public int ReadInt32(IntPtr address)
        {
            byte[] data = ReadProcessMemory(address, 4);
            return BitConverter.ToInt32(data, 0);
        }
        public ulong ReadUInt64(IntPtr address) => BitConverter.ToUInt64(ReadProcessMemory(address, 8), 0);
        public long ReadLong(IntPtr address) => BitConverter.ToInt64(ReadProcessMemory(address, 8), 0);
        public byte ReadByte(IntPtr address) => ReadProcessMemory(address, 1)[0];

        private byte[] ReadProcessMemory(IntPtr address, uint bytesToRead)
        {
            byte[] buffer = new byte[bytesToRead];
            IntPtr bytesRead;
            MemoryApi.ReadProcessMemory(ProcessHwnd, address, buffer, bytesToRead, out bytesRead);
            return buffer;
        }
        public byte[] ReadBytes(IntPtr address, int length)
        {
            return ReadRaw(address, (uint)length);
        }

        // В класс Memory добавьте:
        public T[] ReadArray<T>(IntPtr address, int count) where T : struct
        {
            int elementSize = Marshal.SizeOf<T>();
            byte[] buffer = new byte[elementSize * count];
            IntPtr bytesRead;

            MemoryApi.ReadProcessMemory(ProcessHwnd, address, buffer, (uint)buffer.Length, out bytesRead);

            T[] result = new T[count];
            GCHandle handle = GCHandle.Alloc(buffer, GCHandleType.Pinned);
            try
            {
                IntPtr ptr = handle.AddrOfPinnedObject();
                for (int i = 0; i < count; i++)
                {
                    result[i] = Marshal.PtrToStructure<T>(ptr);
                    ptr += elementSize;
                }
            }
            finally
            {
                handle.Free();
            }
            return result;
        }

        private static byte[] ConcatArrays(byte[] a, byte[] b)
        {
            byte[] result = new byte[a.Length + b.Length];
            Buffer.BlockCopy(a, 0, result, 0, a.Length);
            Buffer.BlockCopy(b, 0, result, a.Length, b.Length);
            return result;
        }

        internal class MemoryApi
        {
            [StructLayout(LayoutKind.Sequential)]
            public struct LUID
            {
                public uint LowPart;
                public int HighPart;
            }

            [StructLayout(LayoutKind.Sequential)]
            public struct TOKEN_PRIVILEGES
            {
                public uint PrivilegeCount;
                public LUID Luid;
                public uint Attributes;
            }

            [DllImport("kernel32.dll", SetLastError = true)]
            public static extern IntPtr OpenProcess(uint dwDesiredAccess, int bInheritHandle, uint dwProcessId);

            [DllImport("kernel32.dll", SetLastError = true)]
            [return: MarshalAs(UnmanagedType.Bool)]
            public static extern bool CloseHandle(IntPtr hObject);

            [DllImport("kernel32.dll", SetLastError = true)]
            public static extern int ReadProcessMemory(
                IntPtr hProcess,
                IntPtr lpBaseAddress,
                [Out] byte[] lpBuffer,
                uint nSize,
                out IntPtr lpNumberOfBytesRead);

            [DllImport("kernel32.dll", SetLastError = true)]
            public static extern int WriteProcessMemory(
                IntPtr hProcess,
                IntPtr lpBaseAddress,
                byte[] lpBuffer,
                uint nSize,
                out IntPtr lpNumberOfBytesWritten);

            [DllImport("advapi32.dll", SetLastError = true)]
            [return: MarshalAs(UnmanagedType.Bool)]
            public static extern bool OpenProcessToken(
                IntPtr ProcessHandle,
                uint DesiredAccess,
                out IntPtr TokenHandle);

            [DllImport("advapi32.dll", SetLastError = true, CharSet = CharSet.Auto)]
            [return: MarshalAs(UnmanagedType.Bool)]
            public static extern bool LookupPrivilegeValue(
                string lpSystemName,
                string lpName,
                out LUID lpLuid);

            [DllImport("advapi32.dll", SetLastError = true)]
            [return: MarshalAs(UnmanagedType.Bool)]
            public static extern bool AdjustTokenPrivileges(
                IntPtr TokenHandle,
                [MarshalAs(UnmanagedType.Bool)] bool DisableAllPrivileges,
                ref TOKEN_PRIVILEGES NewState,
                uint BufferLength,
                IntPtr PreviousState,
                IntPtr ReturnLength);

            [DllImport("kernel32.dll")]
            public static extern IntPtr GetCurrentProcess();
        }
    }
}