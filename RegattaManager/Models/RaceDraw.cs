using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace RegattaManager.Models
{
    public class RaceDraw
    {
        [Key]
        public virtual int RaceDrawId { get; set; }
        [Display(Name = "Von Anzahl Startbooten")]
        public virtual int ReportedSBCountFrom { get; set; }
        [Display(Name = "Bis Anzahl Startbooten")]
        public virtual int ReportedSBCountTo { get; set; }
        [Display(Name = "Anzahl Vorläufe")]
        public virtual int VorlaufCount { get; set; }
        [Display(Name = "Anzahl Hoffnungsläufe")]
        public virtual int HoffnungslaufCount { get; set; }
        [Display(Name = "Anzahl Zwischenläufe")]
        public virtual int ZwischenlaufCount { get; set; }
        [Display(Name = "Anzahl Endläufe")]
        public virtual int EndlaufCount { get; set; }
        public virtual List<RaceDrawRules> RaceDrawRules { get; set; }
    }
}
