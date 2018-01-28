using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RegattaManager.Models
{
    public class RegattaCompetition
    {
        public virtual int CompetitionId { get; set; }
        public virtual Competition Competitions { get; set; }
        public virtual int RegattaId { get; set; }
        public virtual Regatta Regattas { get; set; }
    }
}
