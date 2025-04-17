using System;
using System.Collections.Generic;
using System.DirectoryServices;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Crypto.Modal
{
    public class CoinApiService : IApiSevice
    {
        private readonly HttpClient client;

        private JsonSerializerOptions option;

        public CoinApiService()
        {
            client = new HttpClient();

            option = new JsonSerializerOptions()
            {
                UnmappedMemberHandling = JsonUnmappedMemberHandling.Skip,
                PropertyNameCaseInsensitive = true
            };

            
        }

        public async Task<IEnumerable<Coin>> GetPage(int pageNumber)
        {

            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri($"https://api.coingecko.com/api/v3/coins/markets?vs_currency=usd&order=market_cap_desc&per_page=10&page={pageNumber}&sparkline=false&price_change_percentage=1h,24h,7d"),
                Headers =
            {
                { "accept", "application/json" },
                { "x-cg-demo-api-key", "CG-azvhBCwyzTtfnNCiyZjh5iAN" },
            },
            };
            IEnumerable<Coin> list;
            using (var response = await client.SendAsync(request))
            {
                response.EnsureSuccessStatusCode();
                var body = await response.Content.ReadAsStringAsync();
                list = JsonSerializer.Deserialize<IEnumerable<Coin>>(body, option);
            }

            return list;
        }

        public async Task<IEnumerable<Coin>> SearchCoin(string name)
        {
            var searchReq = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri($"https://api.coingecko.com/api/v3/search?query={name}"),
                Headers =
                {
                    { "accept", "application/json" },
                    { "x-cg-demo-api-key", "CG-azvhBCwyzTtfnNCiyZjh5iAN" },
                },
            };

            string idsString;
            using (var resp = await client.SendAsync(searchReq))
            {
                resp.EnsureSuccessStatusCode();
                var json = await resp.Content.ReadAsStringAsync();
                var searchResult = JsonSerializer.Deserialize<SearchResult>(json, option);
                var coinIds = searchResult?.coins?.Select(c => c.id).ToList();
                if (coinIds == null || !coinIds.Any())
                {
                    return null;
                }
                idsString = string.Join(",", coinIds);
            }

            var marketReq = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri(
                    $"https://api.coingecko.com/api/v3/coins/markets" +
                    $"?vs_currency=usd" +
                    $"&ids={idsString}" +
                    $"&sparkline=false" +
                    $"&price_change_percentage=1h,24h,7d"
                ),
                Headers =
        {
            { "accept", "application/json" },
            { "x-cg-demo-api-key", "CG-azvhBCwyzTtfnNCiyZjh5iAN" },
        },
            };

            IEnumerable<Coin> list;
            using (var resp = await client.SendAsync(marketReq))
            {
                resp.EnsureSuccessStatusCode();
                var body = await resp.Content.ReadAsStringAsync();
                list = JsonSerializer.Deserialize<IEnumerable<Coin>>(body, option);
            }

            return list;
        }
    }
}
