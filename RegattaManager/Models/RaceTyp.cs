using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace RegattaManager.Models
{
    public class RaceTyp
    {
        [Key]
        public virtual int RaceTypId { get; set; }
        [Display(Name = "Renntyp")]
        public virtual string Name { get; set; }
        public virtual bool isFinal { get; set; }
        public virtual List<RaceDrawRules> RaceDrawRules {get;set;}
    }
}
