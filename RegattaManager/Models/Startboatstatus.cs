using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace RegattaManager.Models
{
    public class Startboatstatus
    {
        [Key]
        public virtual int StartboatstatusId { get; set; }
        public virtual string Name { get; set; }
        public virtual List<Startboat> Startboats { get; set; }
    }
}
