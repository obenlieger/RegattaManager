using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace RegattaManager.ViewModels
{
    public class Mannschaftswertung
    {
        [Key]
        public int MWId { get; set; }
        public int ClubId { get; set; }
        public string ClubName { get; set; }
        public string OldclassName { get; set; }
        public double Wertung { get; set; }
    }
}
