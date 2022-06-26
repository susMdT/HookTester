using System;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;

namespace Dinvoke
{
	class Program
	{

		[DllImport("kernel32", CharSet = CharSet.Ansi, ExactSpelling = true, SetLastError = true)]
		static extern IntPtr GetProcAddress(IntPtr hModule, string procName);

		[DllImport("kernel32", SetLastError = true, CharSet = CharSet.Ansi)]
		static extern IntPtr LoadLibrary([MarshalAs(UnmanagedType.LPStr)] string lpFileName);

		[DllImport("kernel32.dll")]
		static extern void Sleep(uint dwMilliseconds);
		// Function to be hooked
		[DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Auto)]
		static extern bool CreateProcessA(
		   string lpApplicationName,
		   string lpCommandLine,
		   ref Structs.SECURITY_ATTRIBUTES lpProcessAttributes,
		   ref Structs.SECURITY_ATTRIBUTES lpThreadAttributes,
		   bool bInheritHandles,
		   uint dwCreationFlags,
		   IntPtr lpEnvironment,
		   string lpCurrentDirectory,
		   [In] ref Structs.STARTUPINFO lpStartupInfo,
		   out Structs.PROCESS_INFORMATION lpProcessInformation);


		// Must create delegate type
		// Marshal.GetFunctionPointerForDelegate requires non generic delegate tpe
		delegate bool CreateProcessFx(string lpApplicationName,
		   string lpCommandLine,
		   ref Structs.SECURITY_ATTRIBUTES lpProcessAttributes,
		   ref Structs.SECURITY_ATTRIBUTES lpThreadAttributes,
		   bool bInheritHandles,
		   uint dwCreationFlags,
		   IntPtr lpEnvironment,
		   string lpCurrentDirectory,
		   [In] ref Structs.STARTUPINFO lpStartupInfo,
		   out Structs.PROCESS_INFORMATION lpProcessInformation);
		// Hook function must be of same signature as of original function
		static bool CreateProcess2(string lpApplicationName,
		   string lpCommandLine,
		   ref Structs.SECURITY_ATTRIBUTES lpProcessAttributes,
		   ref Structs.SECURITY_ATTRIBUTES lpThreadAttributes,
		   bool bInheritHandles,
		   uint dwCreationFlags,
		   IntPtr lpEnvironment,
		   string lpCurrentDirectory,
		   [In] ref Structs.STARTUPINFO lpStartupInfo,
		   out Structs.PROCESS_INFORMATION lpProcessInformation)
		{
			MessageBox.Show("CreateProcessA hooked this!");
			lpProcessInformation = new Structs.PROCESS_INFORMATION();
			return true;
			// Uninstall hook before calling real function otherwise stack overflow
			//hook.Uninstall();
			//hook.Install();
		}
		static void dinvokeApiCreateProcessA(string lpApplicationName,
		   string lpCommandLine,
		   ref Structs.SECURITY_ATTRIBUTES lpProcessAttributes,
		   ref Structs.SECURITY_ATTRIBUTES lpThreadAttributes,
		   bool bInheritHandles,
		   uint dwCreationFlags,
		   IntPtr lpEnvironment,
		   string lpCurrentDirectory,
		   [In] ref Structs.STARTUPINFO lpStartupInfo,
		   out Structs.PROCESS_INFORMATION lpProcessInformation)
		{
			lpProcessInformation = new Structs.PROCESS_INFORMATION();
			object[] funcargs =
{
				lpApplicationName, lpCommandLine, lpProcessAttributes, lpThreadAttributes, bInheritHandles, dwCreationFlags, lpEnvironment, lpCurrentDirectory, lpStartupInfo, lpProcessInformation
			};
			lpProcessInformation = (Structs.PROCESS_INFORMATION) funcargs[9];
			DynamicInvoke.Generic.DynamicApiInvoke("kernel32.dll", "CreateProcessA", typeof(CreateProcessFx), ref funcargs);
		}
		static FxHook hook;
		static void Main(string[] args)
		{
			var k32 = LoadLibrary("kernel32");
			var createProcAddr = GetProcAddress(k32, "CreateProcessA");


			//setting up funcargs and manually mapping module to memory 
			string lpApplicationName = null;
			string lpCommandLine = "C:\\windows\\system32\\notepad.exe";
			Structs.SECURITY_ATTRIBUTES lpProcessAttributes = new Structs.SECURITY_ATTRIBUTES();
			Structs.SECURITY_ATTRIBUTES lpThreadAttributes = new Structs.SECURITY_ATTRIBUTES();
			bool bInheritHandles = false;
			uint dwCreationFlags = 0;
			IntPtr lpEnvironment = IntPtr.Zero;
			string lpCurrentDirectory = null;
			Structs.STARTUPINFO lpStartupInfo = new Structs.STARTUPINFO();
			Structs.PROCESS_INFORMATION lpProcessInformation = new Structs.PROCESS_INFORMATION();
			object[] funcargs = 
			{
				lpApplicationName, lpCommandLine, lpProcessAttributes, lpThreadAttributes, bInheritHandles, dwCreationFlags, lpEnvironment, lpCurrentDirectory, lpStartupInfo, lpProcessInformation 
			};
			Console.WriteLine("Mapping kernel32!");
			Data.PE.PE_MANUAL_MAP moduleDetails = ManualMap.Map.MapModuleToMemory("C:\\Windows\\system32\\kernel32.dll");

			using (hook = new FxHook(createProcAddr, (CreateProcessFx)CreateProcess2))
			{
				Structs.STARTUPINFO si = new Structs.STARTUPINFO();
				Structs.PROCESS_INFORMATION pi = new Structs.PROCESS_INFORMATION();
				Structs.SECURITY_ATTRIBUTES lpa = new Structs.SECURITY_ATTRIBUTES();
				Structs.SECURITY_ATTRIBUTES lta = new Structs.SECURITY_ATTRIBUTES();

				Console.WriteLine("Pointer to CreateProcessA in kernel32.dll made");
				IntPtr createProcessPointerA = DynamicInvoke.Generic.GetLibraryAddress("kernel32.dll", "CreateProcessA");
				Sleep(5000);

				Structs.STARTUPINFO si2 = new Structs.STARTUPINFO();
				Structs.PROCESS_INFORMATION pi2 = new Structs.PROCESS_INFORMATION();
				Structs.SECURITY_ATTRIBUTES lpa2 = new Structs.SECURITY_ATTRIBUTES();
				Structs.SECURITY_ATTRIBUTES lta2 = new Structs.SECURITY_ATTRIBUTES();

				Console.WriteLine("CreateProcessA has been hooked!");
				hook.Install();
				CreateProcessA(null, "C:\\windows\\system32\\notepad.exe", ref lpa, ref lta, false, 0, IntPtr.Zero, null, ref si, out pi);
				DynamicInvoke.Generic.DynamicFunctionInvoke(createProcessPointerA, typeof(CreateProcessFx), ref funcargs);
				dinvokeApiCreateProcessA(null, "C:\\windows\\system32\\notepad.exe", ref lpa2, ref lta2, false, 0, IntPtr.Zero, null, ref si2, out pi2);
				DynamicInvoke.Generic.CallMappedDLLModuleExport(moduleDetails.PEINFO, moduleDetails.ModuleBase, "CreateProcessA", typeof(CreateProcessFx), funcargs);			
			}
		}


	}
}