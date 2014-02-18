using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Seven
{
    public class Addon
    {
        public bool Active { get; set; }
        public string Name { get; set; }
        public string Path { get; set; }

        public Addon(string name, string path)
        {
            Name = name;
            Path = path;
            Active = false;
        }
    }
}
