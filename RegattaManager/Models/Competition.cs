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
        public virtual int BoatclassId { get; set; }
        [Display(Name="Bootsklassen")]
        public virtual Boatclass Boatclasses { get; set; }
        public virtual int RaceclassId { get; set; }
        [Display(Name="Rennklassen")]
        public virtual Raceclass Raceclasses { get; set; }        
        public virtual string Name {
            get
            {
                return string.Format("{0} {1}", Boatclasses.Name, Raceclasses.Name);
            }
        }
        public virtual List<RegattaCompetition> RegattaCompetitions { get; set; }
    }
}
