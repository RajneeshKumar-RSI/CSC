using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSC.Domain
{
    public class Country
    {
        public int? Id { get; set; }
        public string? Name { get; set; }
        public string? ISO3 { get; set; }
        public string? ISO2 { get; set; }
        public string? NumericCode { get; set; }
        public string? PhoneCode { get; set; }
        public string? Capital { get; set; }
        public string? Currency { get; set; }
        public string? CurrencyName { get; set; }
        public string? CurrencySymbol { get; set; }
        public string? Tld { get; set; }
        public string? Native { get; set; }
        public string? Region { get; set; }
        public string? Subregion { get; set; }
        public string? Latitude { get; set; }
        public string? Longitude { get; set; }
        public string? Emoji { get; set; }
        public string? EmojiU { get; set; }
        public virtual Translation? Translations { get; set; }
        public virtual ICollection<State>? States { get; set; }
        public virtual ICollection<City>? Cities { get; set; }
        public virtual ICollection<Timezone>? Timezones { get; set; }
    }
}
