using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace RegattaManager.Models
{
    public class Race
    {
        [Key]
        public virtual int RaceId { get; set; }
        [DataType(DataType.DateTime)]
        public virtual DateTime Starttime { get; set; }
        [DataType(DataType.DateTime)]
        public virtual DateTime Realstarttime { get; set; }
        public virtual string Gender { get; set; }
        public virtual int FinishType { get; set; }
        public virtual string Comment { get; set; }
        public virtual int RegattaId { get; set; }
        public virtual int RaceclassId { get; set; }
        public virtual int OldclassId { get; set; }
        public virtual int BoatclassId { get; set; }
        public virtual int RacestatusId { get; set; }
        public virtual Regatta Regatta { get; set; }
        public virtual Raceclass Raceclass { get; set; }
        public virtual Oldclass Oldclass { get; set; }
        public virtual Boatclass Boatclass { get; set; }
        public virtual Racestatus Racestatus { get; set; }
        public virtual List<Startboat> Startboats { get; set; }
    }
}
