using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static PlumbingProps.Document.WordHelper;

namespace Business.Main.Base
{
    public class GlobalDataReport
    {
        public IObjectReport data { get; set; }
        public Dictionary<string, CellTitles[]> HeadersTables { get; set; }
    }
}
