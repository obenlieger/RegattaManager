using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RegattaManager.Models
{
    public class StartboatStandby
    {
        public int StartboatId { get; set; }
        public Startboat Startboat { get; set; }
        public int MemberId { get; set; }
        public Member Member { get; set; }
        public int Standbynumber { get; set; }
    }
}
