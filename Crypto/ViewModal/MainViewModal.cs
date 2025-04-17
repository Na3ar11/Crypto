using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Crypto.Commands;
using Crypto.Modal;

namespace Crypto.ViewModal
{
    public class MainViewModal : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private IApiSevice coinApiService;

        private int currentPage;
        public ObservableCollection<Coin> coins { get; set; }

        private string searchText;
        public string SearchText
        {
            get { return searchText; }
            set
            {
                searchText = value;
                OnPropertyChanged(nameof(SearchText));
            }
        }
        public ICommand NextPage { get; }

        public ICommand BackPage { get; }

        public ICommand SearchCommand { get;}

        public MainViewModal()
        {
            coinApiService = new CoinApiService();
            coins = new ObservableCollection<Coin>();

            NextPage = new RelayCommand(Next, Can);
            BackPage = new RelayCommand(Back, Can);
            SearchCommand = new RelayCommand(Search, Can);

            currentPage = 1;

            LoadPage();
        }

        private async void LoadPage()
        {
            IEnumerable<Coin> list = await coinApiService.GetPage(currentPage);

            coins.Clear();

            foreach (var item in list)
            {
                coins.Add(item);
            }
        }


        private bool Can(object obj)
        {
            return true;
        }

        private void Next(object obj)
        {
            currentPage++;
            LoadPage();
        }

        private void Back(object obj)
        {
            if (currentPage > 1)
                currentPage--;
            LoadPage();
        }

        private async void Search(object parameter)
        {
            var query = SearchText ?? string.Empty;
            if (query == "")
            {
                LoadPage();
            }
            else
            {
                coins.Clear();
                IEnumerable<Coin> list = await coinApiService.SearchCoin(query);
                if (list == null)
                    return;
                foreach (var coin in list)
                {
                    coins.Add(coin);
                }
            }
        }

    }
}
