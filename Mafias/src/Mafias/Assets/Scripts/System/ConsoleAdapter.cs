using System;
using System.IO;
using Mafias.System.Interop;
using Microsoft.Win32.SafeHandles;

namespace Mafias.System
{
    public class ConsoleAdapter : IDisposable
    {
        private IntPtr _hWnd;
        
        public void Initialize()
        {
            Sys.AllocConsole();
            _hWnd = Sys.GetConsoleWindow();
            Sys.EnableMenuItem(
                Sys.GetSystemMenu(_hWnd, false), 
                Sys.SC_CLOSE, 
                Sys.MF_BYCOMMAND | Sys.MF_DISABLED | Sys.MF_GRAYED);

            var outFile = Sys.CreateFileW(
                "CONOUT$",
                Sys.GENERIC_WRITE | Sys.GENERIC_READ,
                Sys.FILE_SHARE_READ,
                IntPtr.Zero,
                Sys.OPEN_EXISTING,
                0,
                IntPtr.Zero);
            var handle = new SafeFileHandle(outFile, true);
            Sys.SetStdHandle(Sys.STD_OUTPUT_HANDLE, outFile);
            var fs = new FileStream(handle, FileAccess.Write);
            var writer = new StreamWriter(fs) { AutoFlush = true };
            Console.SetOut(writer);
            if (Sys.GetConsoleMode(outFile, out var cMode))
                Sys.SetConsoleMode(outFile, cMode | Sys.ENABLE_VIRTUAL_TERMINAL_INPUT);
        }

        public void WelcomeMessage()
        {
            Console.WriteLine(
                "Welcome to debug console.\n" +
                "Type \"help\" to show usage of this.\n");
        }

        public void HideConsole()
        {
            Sys.ShowWindow(_hWnd, Sys.SW_HIDE);
        }

        public void ShowConsole()
        {
            Sys.ShowWindow(_hWnd, Sys.SW_SHOW);
        }

        public void SetCodepage(uint codepage)
        {
            Sys.SetConsoleOutputCP(codepage);
            Sys.SetConsoleCP(codepage);
        }
        
        public void Dispose()
        {
            _hWnd = IntPtr.Zero;
            Sys.FreeConsole();
        }
    }
}
