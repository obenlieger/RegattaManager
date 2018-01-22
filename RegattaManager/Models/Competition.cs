using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace RegattaManager.Models
{
    public class Competition
    {
        [Key]
        public virtual int CompetitionId { get; set; }
        public virtual int RegattaId { get; set; }
        public virtual Regatta Regattas { get; set; }
        public virtual int BoatclassId { get; set; }
        public virtual Boatclass Boatclasses { get; set; }
        public virtual int RaceclassId { get; set; }
        public virtual Raceclass Raceclasses { get; set; }
    }
}
