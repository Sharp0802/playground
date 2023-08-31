using System;
using System.Runtime.InteropServices;

namespace Mafias.System.Interop
{
    public static class Sys
    {
        // ReSharper disable InconsistentNaming
        // ReSharper disable IdentifierTypo
        
        public const int SW_HIDE = 0;
        public const int SW_SHOW = 5;

        public const uint MF_INSERT = 0x00000000;
        public const uint MF_CHANGE = 0x00000080;
        public const uint MF_APPEND = 0x00000100;
        public const uint MF_DELETE = 0x00000200;
        public const uint MF_REMOVE = 0x00001000;
        public const uint MF_BYCOMMAND = 0x00000000;
        public const uint MF_BYPOSITION = 0x00000400;
        public const uint MF_SEPARATOR = 0x00000800;
        public const uint MF_ENABLED = 0x00000000;
        public const uint MF_GRAYED = 0x00000001;
        public const uint MF_DISABLED = 0x00000002;
        public const uint MF_UNCHECKED = 0x00000000;
        public const uint MF_CHECKED = 0x00000008;
        public const uint MF_USECHECKBITMAPS = 0x00000200;
        public const uint MF_STRING = 0x00000000;
        public const uint MF_BITMAP = 0x00000004;
        public const uint MF_OWNERDRAW = 0x00000100;
        public const uint MF_POPUP = 0x00000010;
        public const uint MF_MENUBARBREAK = 0x00000020;
        public const uint MF_MENUBREAK = 0x00000040;
        public const uint MF_UNHILITE = 0x00000000;
        public const uint MF_HILITE = 0x00000080;
        public const uint MF_DEFAULT = 0x00001000;
        public const uint MF_SYSMENU = 0x00002000;
        public const uint MF_HELP = 0x00004000;
        public const uint MF_RIGHTJUSTIFY = 0x00004000;
        public const uint MF_MOUSESELECT = 0x00008000;
        
        public const int SC_SIZE                = 0xF000;
        public const int SC_MOVE                = 0xF010;
        public const int SC_MINIMIZE            = 0xF020;
        public const int SC_MAXIMIZE            = 0xF030;
        public const int SC_CLOSE               = 0xF060;
        public const int SC_RESTORE             = 0xF120;

        public const uint MB_OK = 0x00000000;
        public const uint MB_OKCANCEL = 0x00000001;
        public const uint MB_ABORTRETRYIGNORE = 0x00000002;
        public const uint MB_YESNOCANCEL = 0x00000003;
        public const uint MB_YESNO = 0x00000004;
        public const uint MB_RETRYCANCEL = 0x00000005;
        public const uint MB_CANCELTRYCONTINUE = 0x00000006;
        public const uint MB_HELP = 0x00004000;
        public const uint MB_ICONSTOP = 0x00000010;
        public const uint MB_ICONERROR = 0x00000010;
        public const uint MB_ICONHAND = 0x00000010;
        public const uint MB_ICONQUESTION = 0x00000020;
        public const uint MB_ICONEXCLAMATION = 0x00000030;
        public const uint MB_ICONWARNING = 0x00000030;
        public const uint MB_ICONINFORMATION = 0x00000040;
        public const uint MB_ICONASTERISK = 0x00000040;
        public const uint MB_DEFBUTTON1 = 0x00000000;
        public const uint MB_DEFBUTTON2 = 0x00000100;
        public const uint MB_DEFBUTTON3 = 0x00000200;
        public const uint MB_DEFBUTTON4 = 0x00000300;
        
        public const uint IDOK = 1;
        public const uint IDCANCEL = 2;
        public const uint IDABORT = 3;
        public const uint IDRETRY = 4;
        public const uint IDIGNORE = 5;
        public const uint IDYES = 6;
        public const uint IDNO = 7;
        public const uint IDTRYAGAIN = 10;
        public const uint IDCONTINUE = 11;

        public const uint CP_UTF16 = 1200;
        public const uint CP_UTF8 = 65001;

        public const uint GENERIC_WRITE = 0x40000000;
        public const uint GENERIC_READ = 0x80000000;
        
        public const uint OPEN_EXISTING = 0x00000003;
        public const uint FILE_SHARE_READ = 0x00000001;

        public const int STD_INPUT_HANDLE = -10;
        public const int STD_OUTPUT_HANDLE = -11;
        public const int STD_ERROR_HANDLE = -12;

        public const uint ENABLE_PROCESSED_INPUT = 0x0001;
        public const uint ENABLE_LINE_INPUT = 0x0002;
        public const uint ENABLE_ECHO_INPUT = 0x0004;
        public const uint ENABLE_WINDOW_INPUT = 0x0008;
        public const uint ENABLE_MOUSE_INPUT = 0x0010;
        public const uint ENABLE_INSERT_MODE = 0x0020;
        public const uint ENABLE_QUICK_EDIT_MODE = 0x0040;
        public const uint ENABLE_EXTENDED_FLAGS = 0x0080;
        public const uint ENABLE_AUTO_POSITION = 0x0100;
        public const uint ENABLE_VIRTUAL_TERMINAL_INPUT = 0x0200;
        
        public const uint ENABLE_PROCESSED_OUTPUT = 0x0001;
        public const uint ENABLE_WRAP_AT_EOL_OUTPUT = 0x0002;
        public const uint ENABLE_VIRTUAL_TERMINAL_PROCESSING = 0x0004;
        public const uint DISABLE_NEWLINE_AUTO_RETURN = 0x0008;
        public const uint ENABLE_LVB_GRID_WORLDWIDE = 0x0010;

        // ReSharper restore IdentifierTypo
        // ReSharper restore InconsistentNaming

        [DllImport("kernel32")]
        public static extern bool AllocConsole();

        [DllImport("kernel32")]
        public static extern bool FreeConsole();

        [DllImport("kernel32")]
        public static extern IntPtr GetConsoleWindow();

        [DllImport("user32")]
        public static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

        [DllImport("user32.dll")]
        public static extern bool EnableMenuItem(IntPtr hMenu, uint uIDEnableItem, uint uEnable);

        [DllImport("user32.dll")]
        public static extern IntPtr GetSystemMenu(IntPtr hWnd, bool bRevert);

        [DllImport("kernel32")]
        public static extern bool SetConsoleCP(uint wCodePageID);

        [DllImport("kernel32")]
        public static extern bool SetConsoleOutputCP(uint wCodePageID);
        
        [DllImport("kernel32.dll")]
        public static extern IntPtr GetStdHandle(int nStdHandle);
        
        [DllImport("kernel32.dll")]
        public static extern void SetStdHandle(int nStdHandle, IntPtr handle);
        
        [DllImport("kernel32.dll", CharSet = CharSet.Auto)]
        public static extern IntPtr CreateFileW(
            string lpFileName,
            uint dwDesiredAccess,
            uint dwShareMode,
            IntPtr lpSecurityAttributes,
            uint dwCreationDisposition,
            uint dwFlagsAndAttributes,
            IntPtr hTemplateFile
        );
        
        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern bool GetConsoleMode(IntPtr hConsoleHandle, out uint lpMode);
        
        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern bool SetConsoleMode(IntPtr hConsoleHandle, uint dwMode);
        
        [DllImport("user32", SetLastError = true, CharSet= CharSet.Auto)]
        public static extern int MessageBox(IntPtr hWnd, string text, string caption, uint type);
    }
}