using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;

namespace CSC.Dtos
{
    public class CSCDto
    {
        public List<CountryDto>? Countries { get; set; }
        public List<StateDto>? States { get; set; }
        public List<CityDto>? Cities { get; set; }
    }
}
