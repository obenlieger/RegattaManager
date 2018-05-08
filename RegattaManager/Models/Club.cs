using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace RegattaManager.Models
{
    public class Club
    {
        [Key]
        public virtual int ClubId { get; set; }
        [Display(Name ="Vereinsname")]
        public virtual string Name { get; set; }
        [Display(Name ="Stadt")]
        public virtual string City { get; set; }
        [Display(Name = "Vereinsnummer")]
        public virtual string VNr { get; set; }
        [Display(Name = "Kurzname")]
        public virtual string ShortName { get; set; }
        public virtual List<Member> Members { get; set; }
        public virtual List<Startboat> Startboats { get; set; }
        public virtual List<Regatta> Regatten { get; set; }
        public virtual List<ReportedStartboat> ReportedStartboats { get; set; }
        public virtual List<RegattaClub> RegattaClubs { get; set; }
    }
}
