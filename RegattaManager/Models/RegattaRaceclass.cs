using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace RegattaManager.Models
{
    public class RegattaRaceclass
    {
        [Key]
        public int RegattaRaceclassId { get; set; }

        public int RegattaId { get; set; }
        public Regatta Regatta { get; set; }

        public int RaceclassId { get; set; }
        public Raceclass Raceclass { get; set; }
    }
}
