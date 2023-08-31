using Mafias.Interactions;
using Mafias.System.Interop;
using UnityEngine;

namespace Mafias.System
{
    public class ConsoleController : MonoBehaviour
    {
        private ConsoleAdapter Adapter { get; set; }
        
        private bool IsOpened { get; set; }
        
        private void Awake()
        {
            DontDestroyOnLoad(this);
            
            Adapter = new ConsoleAdapter();
            Adapter.Initialize();
            Adapter.SetCodepage(Sys.CP_UTF16);
            Adapter.HideConsole();
            Adapter.WelcomeMessage();
            Application.quitting += Adapter.Dispose;
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyMap.ConsoleVisible.Key))
            {
                if (IsOpened)
                    Adapter.HideConsole();
                else
                    Adapter.ShowConsole();
                IsOpened = !IsOpened;
            }
        }
    }
}