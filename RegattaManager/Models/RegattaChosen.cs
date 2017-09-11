using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace RegattaManager.Models
{
    public class RegattaChosen
    {
        [Key]
        public virtual int RegattaChosenId { get; set; }
        public virtual int RegattaId { get; set; }
        public virtual Regatta Regatta { get; set; }
    }
}
