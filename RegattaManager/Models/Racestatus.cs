using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace RegattaManager.Models
{
    public class Racestatus
    {
        [Key]
        public virtual int RacestatusId { get; set; }
        public virtual string Name { get; set; }
        public virtual List<Race> Races { get; set; }
    }
}
