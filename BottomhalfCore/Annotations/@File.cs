using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BottomhalfCore.Annotations
{
    public class @File : Attribute
    {
        public readonly string Filename;
        public File(string Filename) { this.Filename = Filename; }
    }
}