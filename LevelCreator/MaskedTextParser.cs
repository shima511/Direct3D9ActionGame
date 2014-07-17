using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LevelCreator
{
    class MaskedTextParser
    {
        public static float ToSingle(string text)
        {
            float ret = 0;
            text = text.Replace(" ", "");
            if (text[0] == '-' && text[1] == '.' || text[0] == '.')
            {
                text = "0.0";
            }
            ret = float.Parse(text);
            return ret;
        }

        public static int ToInt32(string text)
        {
            int ret = 0;
            text = text.Replace(" ", "");
            if (text == "" || text == "-")
            {
                text = "0";
            }
            ret = int.Parse(text);
            return ret;
        }
    }
}
