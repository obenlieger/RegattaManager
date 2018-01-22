using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RegattaManager.ViewModels
{
    public class RegattaVM
    {
        public int RegattaId { get; set; }        
        public string RegattaName { get; set; }        
        public DateTime RegattaVon { get; set; }
        public DateTime RegattaBis { get; set; }
    }
}
