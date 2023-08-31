using System;
using System.CommandLine;
using System.Reflection;
using System.Threading;
using UnityEngine;

namespace Mafias.System
{
    public class CommandlineManager
    {
        public static RootCommand RootCommand { get; private set; }

        private static Thread ShellThread { get; } = new(() =>
        {
            while (true)
            {
                Console.Write("> ");
                var line = Console.ReadLine() ?? "";
                _ = RootCommand.Invoke(line);
            }
        });

        public static void Initialize()
        {
            var root = new RootCommand(string.Empty);
            var nameField = typeof(RootCommand).GetField(
                "_executableName", 
                BindingFlags.NonPublic | BindingFlags.Instance);
            if (nameField is null)
                throw new MissingFieldException(
                    $"Cannot find field named \"_executableName\" in type \"{typeof(RootCommand).AssemblyQualifiedName}\"");
            nameField.SetValueDirect(__makeref(root), nameField);
            RootCommand = root;

            Application.wantsToQuit += () =>
            {
                if (!ShellThread.Join(TimeSpan.FromSeconds(10)))
                    ShellThread.Abort();
                return true;
            };
        }

        public static void RunLoop()
        {
            ShellThread.Start();
        }
    }
}