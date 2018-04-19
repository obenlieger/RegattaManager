using RegattaManager.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RegattaManager.ViewModels
{
    public class ReportRegatta
    {        
        public int RegattaId { get; set; }
        public string Name { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public int Waterdepth { get; set; }
        public int Startslots { get; set; }
        public string ReportText { get; set; }
        public DateTime ReportOpening { get; set; }
        public DateTime ReportSchedule { get; set; }
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
        public int ClubId { get; set; }
        public Club Club { get; set; }
        public int WaterId { get; set; }
        public Water Waters { get; set; }
        public List<RegattaCampingFee> RegattaCampingFees { get; set; }
        public List<RegattaStartingFee> RegattaStartingFees { get; set; }
        public List<RegattaOldclass> RegattaOldclasses { get; set; }
        public List<RegattaCompetition> RegattaCompetitions { get; set; }        
    }
}
