using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScriptRW
{
    public interface Node
    {
        void Parse(string str, List<ObjectProperty> properties);
    }

    class LineNode : Node
    {
        public Properties Properties { private get; set; }
        List<ObjectProperty> current_list;

        bool SetCurrentList(string str)
        {
            if (!str.Contains(":")) return false;
            if(str.Contains("decolation"))
            {
                current_list = Properties.Decolations;
            }else if(str.Contains("item"))
            {
                current_list = Properties.Items;
            }else if(str.Contains("enemy"))
            {
                current_list = Properties.Enemies;
            }
            else if(str.Contains("texture")){
                current_list = Properties.Textures;
            }
            else if (str.Contains("sound"))
            {
                current_list = Properties.Sounds;
            }
            return true;
        }

        public void Parse(string str, List<ObjectProperty> properties)
        {
            if (str.Length == 0) return;
            if(!SetCurrentList(str))
            {
                TerminalNode node = new TerminalNode();
                node.Parse(str, current_list);
            }
        }
    }

    public class TerminalNode : Node
    {
        public void Parse(string str, List<ObjectProperty> properties)
        {
            ObjectProperty new_property = new ObjectProperty();
            string[] words = str.Split(' ');
            new_property.Id = Int32.Parse(words[0]);
            new_property.Name = words[1];
            new_property.AssetPath = words[2];
            properties.Add(new_property);
        }
    }
}
