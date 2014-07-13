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
    }
}
