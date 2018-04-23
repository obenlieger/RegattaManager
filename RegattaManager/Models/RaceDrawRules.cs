using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace RegattaManager.Models
{
    public class RaceDrawRules
    {
        [Key]
        public virtual int RaceDrawRuleId { get; set; }
        public virtual int RaceDrawId { get; set; }
        public virtual RaceDraw RaceDraw { get; set; }
        public virtual int RaceTypId { get; set; }
        public virtual RaceTyp RaceTyp { get; set; }
        public virtual int RaceSequence { get; set; }
        public virtual int PlacementFrom { get; set; }
        public virtual int PlacementTo { get; set; }
        public virtual int ToRaceTypId { get; set; }
        public virtual int ToRaceSequence { get; set; }
    }
}
