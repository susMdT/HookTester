using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
namespace Dinvoke
{
    public class Delegates
    {
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate void Sleep(
            uint ms);
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate IntPtr EnumDisplayMonitors(
        IntPtr hdc,
        IntPtr lprcClip,
        IntPtr lpfnEnum,
        IntPtr dwData);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate IntPtr OpenProcess(uint processAccess, bool bInheritHandle, uint processId);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate IntPtr VirtualAlloc(
            IntPtr lpAddress,
            uint dwSize,
            uint flAllocationType,
            uint flProtect
        );
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate IntPtr VirtualAllocEx(
            IntPtr hProcess,
            IntPtr lpAddress,
            uint dwSize,
            uint flAllocationType,
            uint flProtect
         );
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate IntPtr CreateThread(
            IntPtr lpThreadAttributes,
            uint dwStackSize,
            IntPtr lpStartAddress,
            IntPtr lpParameter,
            uint dwCreationFlags,
            IntPtr lpThreadId);
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate Boolean CreateProcess(
            string lpApplicationName,
            string lpCommandLine,
            ref Structs.SECURITY_ATTRIBUTES lpProcessAttributes,
            ref Structs.SECURITY_ATTRIBUTES lpThreadAttributes,
            bool bInheritHandles,
            Structs.ProcessCreationFlags
            dwCreationFlags,
            IntPtr lpEnvironment,
            string lpCurrentDirectory,
            [In] ref Structs.STARTUPINFO lpStartupInfo,
            out Structs.PROCESS_INFORMATION lpProcessInformation);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate UInt32 ZwQueryInformationProcess(
            IntPtr hProcess,
            Int32 procInformationClass,
            ref Structs.PROCESS_BASIC_INFORMATION procInformation,
            UInt32 ProcInfoLen,
            ref UInt32 retlen);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate bool ReadProcessMemory(
            IntPtr hProcess,
            IntPtr lpBaseAddress,
            byte[] lpBuffer,
            Int32 nSize,
            out IntPtr lpNumberOfBytesRead);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate bool WriteProcessMemory(
            IntPtr hProcess,
            IntPtr lpBaseAddress,
            byte[] lpBuffer,
            Int32 nSize,
            out IntPtr lpNumberOfBytesWritten);
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate IntPtr OpenThread(
            Structs.ThreadAccess dwDesiredAccess,
            bool bInheritHandle,
            int dwThreadId);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate Boolean VirtualProtectEx(
            IntPtr hProcess,
            IntPtr lpAddress,
            int dwSize,
            uint flNewProtect,
            out uint lpflOldProtect);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate IntPtr QueueUserAPC(
            IntPtr pfnAPC,
            IntPtr hThread,
            IntPtr dwData);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate uint ResumeThread(
            IntPtr hThhread);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate UInt32 LdrLoadDll(
            IntPtr PathToFile,
            UInt32 dwFlags,
            ref Structs.UNICODE_STRING ModuleFileName,
            ref IntPtr ModuleHandle);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate void RtlInitUnicodeString(
            ref Structs.UNICODE_STRING DestinationString,
            [MarshalAs(UnmanagedType.LPWStr)] string SourceString);
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate IntPtr CreateRemoteThread(
            IntPtr hProcess,
            IntPtr lpThreadAttributes,
            uint dwStackSize,
            IntPtr lpStartAddress,
            IntPtr lpParameter,
            uint dwCreationFlags,
            out IntPtr lpThreadId);
    }
}
