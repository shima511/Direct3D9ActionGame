using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileArchiver
{
    class Program
    {

        static void Main(string[] args)
        {
            string[] dirs = Directory.GetDirectories(args[0]);
            foreach (var item in dirs)
            {
                if(item == "Models")
                {

                }

            }
        }
    }
}
