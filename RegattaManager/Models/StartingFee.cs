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
        public virtual int BoatclassId { get; set; }
        [Display(Name ="Bootsklasse")]
        public virtual Boatclass Boatclasses { get; set; }
        public virtual int OldclassId { get; set; }
        [Display(Name ="Altersklasse")]
        public virtual Oldclass Oldclasses { get; set; }
        public virtual string Name
        {
            get
            {
                return string.Format("{0} {1} = {2}", Boatclasses.Name, Oldclasses.Name, Amount);
            }
        }
        public virtual List<RegattaStartingFee> RegattaStartingFees { get; set; }
    }
}
