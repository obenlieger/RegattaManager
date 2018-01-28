using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using RegattaManager.Models;

namespace RegattaManager.ViewModels
{
    public class RegattaVM
    {
        public int RegattaId { get; set; }     
        [Display(Name = "Name")]
        public string RegattaName { get; set; }  
        [Display(Name = "Von")]
        public DateTime RegattaVon { get; set; }
        [Display(Name = "Bis")]
        public DateTime RegattaBis { get; set; }
        [Display(Name = "Wassertiefe")]
        public int Waterdepth { get; set; }
        [Display(Name = "Startbahnen")]
        public int Startslots { get; set; }
        [Display(Name = "Meldung")]
        public string ReportText { get; set; }
        [Display(Name = "Meldetermin")]
        public DateTime ReportSchedule { get; set; }
        [Display(Name = "Meldeeröffnung")]
        public DateTime ReportOpening { get; set; }
        [Display(Name = "Meldeadresse")]
        public string ReportAddress { get; set; }
        [Display(Name = "Telefon")]
        public string ReportTel { get; set; }
        [Display(Name = "Fax")]
        public string ReportFax { get; set; }
        [Display(Name = "E-Mail")]
        public string ReportMail { get; set; }
        [Display(Name = "Kampfrichter")]
        public string Judge { get; set; }
        [Display(Name = "Auszeichnungen")]
        public string Awards { get; set; }
        [Display(Name = "Sicherheit")]
        public string Security { get; set; }
        [Display(Name = "Zeitplan")]
        public string ScheduleText { get; set; }
        [Display(Name = "Teilnehmergebühren")]
        public float SubscriberFee { get; set; }
        [Display(Name = "Unterkunft")]
        public string Accomodation { get; set; }
        [Display(Name = "Bemerkungen")]
        public string Comment { get; set; }
        [Display(Name = "Verpflegung")]
        public string Catering { get; set; }
        public int WaterId { get; set; }
        public int ClubId { get; set; }        
        public IEnumerable<Oldclass> Oldclasses { get; set; }
        [Display(Name = "Startklassen")]
        public IEnumerable<RegattaOldclass> RegattaOldclasses { get; set; }        
        public IEnumerable<CampingFee> CampingFees { get; set; }
        [Display(Name = "Zeltplatzgebühren")]
        public IEnumerable<RegattaCampingFee> RegattaCampingFees { get; set; }
        [Display(Name = "Startgebühren")]
        public IEnumerable<StartingFee> StartingFees { get; set; }
        public IEnumerable<RegattaStartingFee> RegattaStartingFees { get; set; }
        [Display(Name = "Wettkämpfe")]
        public IEnumerable<Competition> Competitions { get; set; }
        public IEnumerable<RegattaCompetition> RegattaCompetitions { get; set; }        
        public IEnumerable<Raceclass> Raceclasses { get; set; }
    }
}
