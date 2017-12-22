using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace RegattaManager.Models
{
    public class Regatta
    {
        [Key]
        public virtual int RegattaId { get; set; }
        [Display(Name = "Regattaname")]
        public virtual string Name { get; set; }
        public virtual bool Choosen { get; set; }
        [Display(Name = "Von")]
        public virtual DateTime FromDate { get; set; }
        [Display(Name = "Bis")]
        public virtual DateTime ToDate { get; set; }
        public virtual int ClubId { get; set; }
        [Display(Name = "Verein")]
        public virtual Club Club { get; set; }        
        public virtual List<Startboat> Startboats { get; set; }
        public virtual List<Race> Races { get; set; }        
    }
}
