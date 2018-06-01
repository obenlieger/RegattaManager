using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace RegattaManager.Models
{
    public class ClubCampingFee
    {
        [Key]
        public int ClubCampingFeeId { get; set; }
        [Display(Name = "Zeltgebühr")]
        public int CampingFeeId { get; set; }
        public CampingFee campingFee { get; set; }
        [Display(Name = "Verein")]
        public int ClubId { get; set; }
        public Club club { get; set; }
        [Display(Name = "Anzahl")]
        public int campingcount { get; set; }
    }
}
