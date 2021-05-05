using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Main.Cross
{
    public class Rango : Attribute
    {
        public int rowA { get; set; }
        public int rowB { get; set; }
        
        public Rango(int _rowA, int _rowB)
        {
            rowA = _rowA;
            rowB = _rowB;
        }
    }
}
