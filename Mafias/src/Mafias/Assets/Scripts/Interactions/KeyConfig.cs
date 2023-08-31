using UnityEngine;

namespace Mafias.Interactions
{
    public class KeyConfig
    {
        public KeyConfig(string name, KeyCode key)
        {
            Name = name;
            Key = key;
        }

        public string Name { get; }
        public KeyCode Key { get; }
    }
}