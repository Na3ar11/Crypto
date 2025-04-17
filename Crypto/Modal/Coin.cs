using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Crypto.Modal
{
    public class Coin
    {
        public string? id { get; set; }
        public string? name {  get; set; }

        public decimal current_price { get; set; }

        public decimal? price_change_percentage_1h_in_currency { get; set; }

        public decimal? price_change_percentage_24h_in_currency { get; set; }

        public decimal? price_change_percentage_7d_in_currency { get; set; }

        public decimal total_volume { get; set; }

        public decimal market_cap { get; set; }

    }

    public class SearchResult
    {
        public List<Coin>? coins { get; set; }
    }
}
