using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSC.Domain
{
    public class Timezone
    {
        public int Id { get; set; }
        public string? ZoneName { get; set; }
        public int? GMTOffset { get; set; }
        public string? GMTOffsetName { get; set; }
        public string? Abbreviation { get; set; }
        public string? TZName { get; set; }        
    }
}
