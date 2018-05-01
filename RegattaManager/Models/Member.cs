using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace RegattaManager.Models
{
    public class Member
    {
        [Key]
        public virtual int MemberId { get; set; }
        public virtual string LastName { get; set; }
        public virtual string FirstName { get; set; }
        public virtual string FullName
        {
            get
            {
                return string.Format("{0}, {1}", LastName, FirstName);
            }
        }
        public virtual string FullNameClub
        {
            get
            {
                return string.Format("{0}, {1}, {2}", LastName, FirstName, ClubId);
            }
        }
        public virtual int Birthyear { get; set; }
        public virtual string Gender { get; set; }
        public virtual bool isRented { get; set; }
        public virtual int RentedToClubId { get; set; }
        public virtual int RentYear { get; set; }
        public virtual int ClubId { get; set; }
        public virtual Club Club { get; set; }        
        public virtual List<StartboatMember> StartboatMembers { get; set; }
        public virtual List<StartboatStandby> StartboatStandbys { get; set; }
        public virtual List<ReportedStartboatMember> ReportedStartboatMembers { get; set; }
        public virtual List<ReportedStartboatStandby> ReportedStartboatStandbys { get; set; }
    }
}
