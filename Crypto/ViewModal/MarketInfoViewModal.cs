using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Crypto.Modal;

namespace Crypto.ViewModal
{
    public class MarketInfoViewModal : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string name = null) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));

        public string NameCoin { get; set; }

        public string id { get; set; }

        public ObservableCollection<MarketInfo> Markets { get; set; }

        private IApiSevice coinApiService;

        public MarketInfoViewModal(string _id, string _NameCoin)
        {
            id = _id;
            NameCoin = _NameCoin;
            coinApiService = new CoinApiService();
            Markets = new ObservableCollection<MarketInfo>();
            LoadInfo();
        }

        public async void LoadInfo()
        {
            IEnumerable<MarketInfo> list = await coinApiService.GetCoinMarketsAsync(id);

            Markets.Clear();

            foreach (var item in list)
            {
                Markets.Add(item);
            }
        }

    }
}
