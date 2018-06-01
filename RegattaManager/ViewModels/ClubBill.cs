using RegattaManager.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RegattaManager.ViewModels
{
    public class ClubBill
    {
        public string ClubName { get; set; }
        public int StartboatCount { get; set; }
        public List<ReportedStartboat> reportedStartboats { get; set; }
        public List<Member> members { get; set; }
        public double StartingFee { get; set; }
        public double CampingFee { get; set; }
        public double SubscriberFee { get; set; }
    }
}
