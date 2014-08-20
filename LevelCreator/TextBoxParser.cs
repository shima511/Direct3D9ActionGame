using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace LevelCreator
{
    class TextBoxParser
    {
        public static float ToSingle(string text)
        {
            if (text.Length == 0) return 0;
            bool minus = false;
            if (text[0] == '-') minus = true;
            float ret = 0;
            Regex re = new Regex(@"[^0-9 | .]");
            string formated_text = re.Replace(text, "");
            if (formated_text.Length == 0) return 0;
            ret = float.Parse(formated_text);
            if (minus) ret *= -1;
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

        public static string ToString(float num)
        {
            string str = num.ToString();
            
            return str;
        }
    }
}
