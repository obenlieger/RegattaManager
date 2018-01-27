using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace RegattaManager.Models
{
    public class StartingFee
    {
        [Key]
        public virtual int StartingFeeId { get; set; }
        public virtual float Amount { get; set; }
        public virtual int BoatclassId { get; set; }
        public virtual Boatclass Boatclasses { get; set; }
        public virtual int OldclassId { get; set; }
        public virtual Oldclass Oldclasses { get; set; }
        public virtual List<RegattaStartingFee> RegattaStartingFees { get; set; }
    }
}
