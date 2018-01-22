using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace RegattaManager.Models
{
    public class Boatclass
    {
        [Key]
        public virtual int BoatclassId { get; set; }
        [Display(Name ="Bootsklasse")]
        public virtual string Name { get; set; }
        [Display(Name = "Anzahl Sitze")]
        public virtual int Seats { get; set; }
        public virtual List<Race> Races { get; set; }
        public virtual List<Competition> Competitions { get; set; }
    }
}
