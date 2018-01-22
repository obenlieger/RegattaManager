using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RegattaManager.Models
{
    public class RegattaOldclass
    {
        public virtual int OldclassId { get; set; }
        public virtual Oldclass Oldclasses { get; set; }
        public virtual int RegattaId { get; set; }
        public virtual Regatta Regattas { get; set; }
    }
}
