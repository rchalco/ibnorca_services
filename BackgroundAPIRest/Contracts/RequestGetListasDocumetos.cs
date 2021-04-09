using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BackgroundAPIRest.Contracts
{
    public class RequestGetListasDocumetos
    {
        public string area { get; set; }
        public string proceso { get; set; }
    }
}
