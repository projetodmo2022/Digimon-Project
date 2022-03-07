using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;
using System.Linq;
using System.ComponentModel;
using Memory;
using System.Threading;

namespace DMO_Launcher
{
    class Program
    {
        const int PROCESS_CREATE_PROCESS = 0x0080;
        const int PROCESS_QUERY_INFORMATION = 0x0400;
        const int PROCESS_VM_OPERATION = 0x0008;
        const int PROCESS_VM_READ = 0x0010;
        const int PROCESS_VM_WRITE = 0x0020;

        const uint MEM_COMMIT = 0x1000;
        const uint MEM_RESERVE = 0x2000;
        const uint PAGE_EXECUTE_READWRITE = 0x0040;

        [DllImport("kernel32.dll")]
        public static extern IntPtr OpenProcess(int dwDesiredAccess, bool bInheritHandle, int dwProcessId);
        [DllImport("kernel32.dll", SetLastError = true, ExactSpelling = true)]
        public static extern IntPtr VirtualAllocEx(IntPtr hProcess, IntPtr lpAddress, uint dwSize, uint flAllocationType, uint flProtect);
        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern bool WriteProcessMemory(IntPtr hProcess, IntPtr lpBaseAddress, byte[] lpBuffer, uint nSize, out UIntPtr lpNumberOfBytesWritten);
        [DllImport("kernel32.dll", CharSet = CharSet.Auto)]
        public static extern IntPtr GetModuleHandle(string lpModuleName);
        [DllImport("kernel32.dll", CharSet = CharSet.Ansi, ExactSpelling = true, SetLastError = true)]
        public static extern IntPtr GetProcAddress(IntPtr hModule, string lpProcName);
        [DllImport("kernel32.dll")]
        public static extern IntPtr CreateRemoteThread(IntPtr hProcess, IntPtr lpThreadAttributes, uint dwStackSize, IntPtr lpStartAddress, IntPtr lpParameter, uint dwCreationFlags, IntPtr lpThreadId);
        static void Main(string[] args)
        {
            string processName = "GDMO";

            Process GDMO = new Process();
            GDMO.StartInfo.FileName = @"GDMO.exe";
            GDMO.StartInfo.Arguments = " DiMaOAuthKey.value";
            GDMO.Start();

            //Console.WriteLine("1. Get the process ID");
            int pid = Program.FindProcessID(processName);
            if (pid < 0)
            {
                //Console.WriteLine("\t**[FAILURE]  Not found the process \"" + processName + "\".");
                Console.ReadKey();
                return;
            }
            //Console.WriteLine("\t[SUCCESS]  The process \"" + processName + "\" is found. And PID is " + pid + ".");

            //Console.WriteLine("2. Get the handle to the process");
            IntPtr pHandle = OpenProcess(
                PROCESS_CREATE_PROCESS |
                PROCESS_QUERY_INFORMATION |
                PROCESS_VM_OPERATION |
                PROCESS_VM_READ |
                PROCESS_VM_WRITE, false, pid);
            if (pHandle == null || pHandle == IntPtr.Zero)
            {
                ////Console.WriteLine("\t**[FAILURE]  Doesn't obtain the handle to the process (" + processName + ", " + pid + ").");
                Console.ReadKey();
                return;
            }
            //Console.WriteLine("\t[SUCCESS]  The handle to the process (" + processName + ", " + pid + ")  is 0x" + pHandle.ToString("X8") + ".");

            //Console.WriteLine("3. Allocate the memory for the dll path");
            string dllPath = "X3.dll";
            IntPtr dllAddr = VirtualAllocEx(
                pHandle,
                IntPtr.Zero,
                (uint)((dllPath.Length + 1) * Marshal.SizeOf(typeof(char))),
                MEM_RESERVE | MEM_COMMIT,
                PAGE_EXECUTE_READWRITE);
            if (dllAddr == null || dllAddr == IntPtr.Zero)
            {
                //Console.WriteLine("\t**[FAILURE]  Allocate memory failed.");
                Console.ReadKey();
                return;
            }
            //Console.WriteLine("\t[SUCCESS]  Successfully allocate memory at 0x" + dllAddr.ToString("X8") + ".");

            //Console.WriteLine("4. Write the dll path to the memory");
            UIntPtr bytesWritten;
            if (WriteProcessMemory(pHandle, dllAddr, Encoding.Default.GetBytes(dllPath), (uint)((dllPath.Length + 1) * Marshal.SizeOf(typeof(char))), out bytesWritten) == false)
            {
                //Console.WriteLine("\t**[FAILURE]  Failed to write the dll path into memory.");
                Console.ReadKey();
                return;
            }
            //Console.WriteLine("\t[SUCCESS]  The dll path is successfully written into the memory.");

            //Console.WriteLine("5. Get address of \"LoadLibraryA\"");
            IntPtr loadLibraryAAddr = GetProcAddress(GetModuleHandle("kernel32.dll"), "LoadLibraryA");
            if (loadLibraryAAddr == null || loadLibraryAAddr == IntPtr.Zero)
            {
                //Console.WriteLine("\t**[FAILURE]  LoadLibraryA is not found.");
                Console.ReadKey();
                return;
            }
            //Console.WriteLine("\t[SUCCESS]  LoadLibraryA is found at 0x" + loadLibraryAAddr.ToString("X8") + "");

            //Console.WriteLine("6. Create remote thread");
            IntPtr remoteThreadHandle = CreateRemoteThread(
                pHandle,
                IntPtr.Zero,
                0,
                loadLibraryAAddr,
                dllAddr,
                0,
                IntPtr.Zero);
            if (remoteThreadHandle == null || remoteThreadHandle == IntPtr.Zero)
            {
                //Console.WriteLine("\t**[FAILURE]  Remote thread creation failed.");
                Console.ReadKey();
                return;
            }
            //Console.WriteLine("\t[SUCCESS]  The handle to the remote thread is 0x" + remoteThreadHandle.ToString("X8") + ".");

            //Memory.Mem memory = new Memory.Mem();
            //memory.OpenProcess(Process.GetProcessesByName("GDMO").FirstOrDefault().Id);
            //memory.WriteMemory("x3.dll+484C8", "string", "127.0.0.1      ");

            //Console.ReadKey();
            Thread.Sleep(9000);
            Mem m = new Mem();
            m.OpenProcess(Process.GetProcessesByName("GDMO").FirstOrDefault().Id);
            m.WriteMemory("X3.dll+484C8", "string", "127.0.0.1      ");

            return;
        }
        static int FindProcessID(string processName)
        {
            Process[] processes = Process.GetProcessesByName(processName);
            if (processes == null || processes.Length == 0)
                return -1;
            return processes[0].Id;
        }

    }
}
