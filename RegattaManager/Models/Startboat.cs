using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace RegattaManager.Models
{
    public class Startboat
    {
        [Key]
        public virtual int StartboatId { get; set; }
        public virtual int Startslot { get; set; }
        public virtual int Placement { get; set; }
        public virtual DateTime FinishTime { get; set; }
        public virtual int ClubId { get; set; }        
        public virtual int RaceId { get; set; }
        public virtual int StartboatstatusId { get; set; }
        public virtual Club Club { get; set; }
        public virtual Race Race { get; set; }
        public virtual Startboatstatus Startboatstatus { get; set; }
        public virtual List<StartboatMember> StartboatMembers { get; set; }
        public virtual int RegattaId { get; set; }
        public virtual Regatta Regatta { get; set; }
    }
}
