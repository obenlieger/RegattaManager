using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace RegattaManager.Models
{
    public class CampingFee
    {
        [Key]
        public virtual int CampingFeeId { get; set; }
        public virtual string Name { get; set; }
        public virtual float Amount { get; set; }
        public virtual List<RegattaCampingFee> RegattaCampingFees { get; set; }
    }
}
