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
        [Display(Name = "Gebühr")]
        public virtual float Amount { get; set; }
        [Display(Name = "Bootsklasse")]
        public virtual int BoatclassId { get; set; }
        [Display(Name ="Bootsklasse")]
        public virtual Boatclass Boatclasses { get; set; }
        [Display(Name = "Von Altersklasse")]
        public virtual int FromOldclassId { get; set; }
        [Display(Name = "Bis Altersklasse")]
        public virtual int ToOldclassId { get; set; }
        public virtual List<RegattaStartingFee> RegattaStartingFees { get; set; }
    }
}
