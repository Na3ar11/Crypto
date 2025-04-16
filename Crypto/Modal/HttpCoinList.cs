using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Crypto.Modal
{
    public class HttpCoinList
    {
        private readonly HttpClient client;

        private JsonSerializerOptions option;

        private int PageNumber;
        public HttpCoinList()
        {
            client = new HttpClient();

            option = new JsonSerializerOptions()
            {
                UnmappedMemberHandling = JsonUnmappedMemberHandling.Skip
            };

            PageNumber = 1;
        }

        public async Task<List<Coin>> GetPage()
        {
            List<Coin> list ;

            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri($"https://api.coingecko.com/api/v3/coins/markets?vs_currency=usd&order=market_cap_desc&per_page=10&page={PageNumber}&sparkline=false&price_change_percentage=1h,24h,7d"),
                Headers =
            {
                { "accept", "application/json" },
                { "x-cg-demo-api-key", "CG-azvhBCwyzTtfnNCiyZjh5iAN" },
            },
            };
            using (var response = await client.SendAsync(request))
            {
                response.EnsureSuccessStatusCode();
                var body = await response.Content.ReadAsStringAsync();
                list = JsonSerializer.Deserialize<List<Coin>>(body, option);
            }

            return list;
        }

        public void MoveNext()
        {
            PageNumber++;
        }
        public void MoveBack()
        {
            if(PageNumber > 1)
            {
                PageNumber--;
            }
        }

        public IEnumerable<Coin> SearchCoin(string name) 
        {
            List<Coin> coin = new List<Coin>();

            return coin;
        }
    }
}
