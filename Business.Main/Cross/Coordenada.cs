using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Main.Cross
{
    [Serializable]
    public class Coordenada : Attribute
    {
        public int row { get; set; }
        public int column { get; set; }
        public Coordenada(int _row, int _column)
        {
            row = _row;
            column = _column;
        }
    }
}
