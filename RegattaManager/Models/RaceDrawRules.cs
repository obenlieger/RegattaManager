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
        [Display(Name = "Auslosungsregel")]
        public virtual int RaceDrawRuleId { get; set; }
        [Display(Name = "Auslosung")]
        public virtual int RaceDrawId { get; set; }
        [Display(Name = "Auslosung")]
        public virtual RaceDraw RaceDraw { get; set; }
        [Display(Name = "Renntyp")]
        public virtual int RaceTypId { get; set; }
        [Display(Name = "Renntyp")]
        public virtual RaceTyp RaceTyp { get; set; }
        [Display(Name = "Lauf")]
        public virtual int RaceSequence { get; set; }
        [Display(Name = "Von Platz")]
        public virtual int PlacementFrom { get; set; }
        [Display(Name = "Bis Platz")]
        public virtual int PlacementTo { get; set; }
        [Display(Name = "In Renntyp")]
        public virtual int ToRaceTypId { get; set; }
        [Display(Name = "In Lauf")]
        public virtual int ToRaceSequence { get; set; }
    }
}
