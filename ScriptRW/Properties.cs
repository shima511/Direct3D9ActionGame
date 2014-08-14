using System.Collections.Generic;

namespace ScriptRW
{
    public struct Properties
    {
        public List<ObjectProperty> Decolations { get; set; }
        public List<ObjectProperty> Items { get; set; }
        public List<ObjectProperty> Enemies { get; set; }
        public List<ObjectProperty> Textures { get; set; }
    }
}