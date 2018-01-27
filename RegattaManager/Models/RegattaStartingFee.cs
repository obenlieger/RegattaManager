using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace RegattaManager.Models
{
    public class RegattaStartingFee
    {
        public virtual int StartingFeeId { get; set; }
        public virtual StartingFee StartingFees { get; set; }
        public virtual int RegattaId { get; set; }
        public virtual Regatta Regattas { get; set; }
    }
}
