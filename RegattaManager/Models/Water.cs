using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace RegattaManager.Models
{
    public class Water
    {
        [Key]
        public virtual int WaterId { get; set; }
        public virtual string Name { get; set; }
    }
}
