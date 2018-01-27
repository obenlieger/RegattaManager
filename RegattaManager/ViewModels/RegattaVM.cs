using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RegattaManager.Models;

namespace RegattaManager.ViewModels
{
    public class RegattaVM
    {
        public int RegattaId { get; set; }        
        public string RegattaName { get; set; }        
        public DateTime RegattaVon { get; set; }
        public DateTime RegattaBis { get; set; }
        public int Waterdepth { get; set; }
        public int Startslots { get; set; }
        public string ReportText { get; set; }
        public DateTime ReportSchedule { get; set; } 
        public DateTime ReportOpening { get; set; }
        public string ReportAddress { get; set; }
        public string ReportTel { get; set; }
        public string ReportFax { get; set; }
        public string ReportMail { get; set; }
        public string Judge { get; set; }
        public string Awards { get; set; }
        public string Security { get; set; }
        public string ScheduleText { get; set; }
        public float SubscriberFee { get; set; }
        public string Accomodation { get; set; }
        public string Comment { get; set; }
        public string Catering { get; set; }
        public int WaterId { get; set; }
        public int ClubId { get; set; }
        public IEnumerable<Oldclass> Oldclasses { get; set; }
        public IEnumerable<RegattaOldclass> RegattaOldclasses { get; set; }
        public IEnumerable<CampingFee> CampingFees { get; set; }
        public IEnumerable<RegattaCampingFee> RegattaCampingFees { get; set; }
        public IEnumerable<StartingFee> StartingFees { get; set; }
        public IEnumerable<Competition> Competitions { get; set; }
        public IEnumerable<Raceclass> Raceclasses { get; set; }
    }
}
