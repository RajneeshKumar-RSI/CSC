using CSC.Domain;
using CSC.Dtos;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace CSC
{
    public class Downloader
    {
        private readonly string all_countries_endpoint = "https://api.countrystatecity.in/v1/countries";
        private readonly string endpoint_country = "https://raw.githubusercontent.com/dr5hn/countries-states-cities-database/master/countries.json";
        private readonly string endpoint_state = "https://raw.githubusercontent.com/dr5hn/countries-states-cities-database/master/states.json";
        private readonly string endpoint_city = "https://raw.githubusercontent.com/dr5hn/countries-states-cities-database/master/cities.json";

        public async Task RunAsync()
        {
            Console.WriteLine("Downloading... .. .");

            /*Get ALL*/
            var Taskcountries = Get<CountryDto>(endpoint_country);

            var Taskstates = Get<StateDto>(endpoint_state);

            var Taskcities = Get<CityDto>(endpoint_city);

            await Task.WhenAll(Taskcountries, Taskstates, Taskcities);

            var csc = new CSCDto
            {
                Countries = await Taskcountries,

                States = await Taskstates,

                Cities = await Taskcities
            };

            Console.WriteLine("Extracting and saving data... .. .");

            await SaveAsync(csc);

            Console.WriteLine("Completed... .. .");
        }

        #region private
        private async Task SaveAsync(CSCDto? dto)
        {
            var list = new List<Country>();

            foreach (var c in dto.Countries)
            {

                Console.WriteLine($"mapping Country {c.name}....,");

                var stateIDs = dto.States.Where(x => x.country_id == c.id).ToArray();

                var countryAllStates = new List<State>();

                var countryAllCities = new List<City>();

                foreach (var s in stateIDs)
                {
                    var singleState = dto.States.Where(x => x.id == s.id)
                        .Select(x => new State
                        {
                            Name = x.name,
                            CountryCode = x.country_code,
                            CountryName = x.country_name,
                            StateCode = x.state_code,
                            Type = x.type,
                            Latitude = x.latitude,
                            Longitude = x.longitude,
                        })
                        .SingleOrDefault();

                    var singleStateCities = dto.Cities.Where(x => x.state_id == s.id)
                        .Select(p => new City
                        {
                            Name = p.name,
                            StateCode = p.state_code,
                            Latitude = p.latitude,
                            Longitude = p.longitude,
                            WikiDataId = p.wikiDataId,
                        })
                        .ToList();

                    singleState.Cities = singleStateCities;
                    countryAllStates.Add(singleState);
                    countryAllCities.AddRange(singleStateCities);

                }

                Console.WriteLine($"mapping {countryAllStates.Count} states........");

                Console.WriteLine($"mapping {countryAllCities.Count} cities.........................");

                var timezone = c.timezones.Select(x => new Timezone
                {
                    ZoneName = x.zoneName,
                    GMTOffset = x.gmtOffset,
                    GMTOffsetName = x.gmtOffsetName,
                    Abbreviation = x.abbreviation,
                    TZName = x.tzName
                }).ToList();

                var transactions = new Translation
                {
                    kr = c.translations.kr,
                    ptBR = c.translations.ptBR,
                    pt = c.translations.pt,
                    nl = c.translations.nl,
                    hr = c.translations.hr,
                    fa = c.translations.fa,
                    de = c.translations.de,
                    es = c.translations.es,
                    fr = c.translations.fr,
                    ja = c.translations.ja,
                    it = c.translations.it,
                    cn = c.translations.cn,
                    tr = c.translations.tr,
                };

                var country = new Country
                {
                    Name = c.name,
                    ISO3 = c.iso3,
                    ISO2 = c.iso2,
                    NumericCode = c.numeric_code,
                    PhoneCode = c.phone_code,
                    Capital = c.capital,
                    Currency = c.currency,
                    CurrencyName = c.currency_name,
                    CurrencySymbol = c.currency_symbol,
                    Tld = c.tld,
                    Native = c.native,
                    Region = c.region,
                    Subregion = c.subregion,
                    Latitude = c.latitude,
                    Longitude = c.longitude,
                    Emoji = c.emoji,
                    EmojiU = c.emojiU,
                    Translations = transactions,
                    Timezones = timezone,
                    States = countryAllStates,
                    Cities = countryAllCities
                };

                list.Add(country);

                dto.Cities.RemoveRange(0, countryAllCities.Count);
            }
            Console.WriteLine($@"Wait... /\/\/\/\/\/\/\/\/\/\/\....Saving {list.Count} countries......./\/\/\/\/\/\/\/\/\/\/\/\/");

            foreach (var chunck50 in list.Chunk(50))
            {
                var result = await Save<Country>(chunck50.ToList());
                await Task.Delay(5000);
            }


            Console.WriteLine($"Saved all countries and their data....................-(@!@)-");
        }
        private static async Task<int> Save<T>(List<T> response) where T : class
        {
            int result;
            using (var context = new DatabaseContext())
            {
                using (var tran = context.Database.BeginTransaction())
                {
                    await context.Set<T>().AddRangeAsync(response);

                    result = await context.SaveChangesAsync();

                    tran.Commit();
                }
            }

            return result;
        }
        private async Task<List<T>> Get<T>(string endpoint)
        {
            List<T> data = new();

            using (HttpClient client = new())
            {
                //client.BaseAddress = new Uri(baseURL);               
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage response = await client.GetAsync(endpoint);
                response.EnsureSuccessStatusCode();
                if (response.IsSuccessStatusCode)
                {
                    data = await response.Content.ReadFromJsonAsync<List<T>>() ?? new List<T>();
                }
            }

            return data;
        }
        #endregion
    }
}
