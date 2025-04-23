using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data.Common;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Reflection.Metadata;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Crypto.Commands;
using Crypto.Modal;
using Crypto.View;

namespace Crypto.ViewModal
{
    public class MainViewModal : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private IWindowService _windowService;

        private IApiSevice coinApiService;

        private int currentPage;

        private Coin[] CoinsForCalculator;

        private int numberOfCoin = 0;
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

        private Coin _selectedCoin;

        public Coin SelectedCoin
        {
            get => _selectedCoin;
            set
            {
                if (_selectedCoin == value) return;
                _selectedCoin = value;
                if (value != null)
                {
                    ShowMarketInfoCommand.Execute(value);
                    CoinSelectedCommand.Execute(value);
                }
            }
        }
        public ICommand NextPage { get; }

        public ICommand BackPage { get; }

        public ICommand SearchCommand { get; }

        public ICommand ShowSettingsCommand { get; }

        public ICommand ShowCalculatorCommand { get; }

        public ICommand CoinSelectedCommand { get; }

        public ICommand ShowMarketInfoCommand { get; }

        public MainViewModal(IWindowService windowService)
        {
            coinApiService = new CoinApiService();
            coins = new ObservableCollection<Coin>();

            _windowService = windowService;
            NextPage = new RelayCommand(Next, Can);
            BackPage = new RelayCommand(Back, Can);
            SearchCommand = new RelayCommand(Search, Can);
            ShowSettingsCommand = new RelayCommand(ShowSettings, Can);
            ShowCalculatorCommand = new RelayCommand(ShowCalculator, Can);
            CoinSelectedCommand = new RelayCommand(ExecuteCoinSelected, Can);
            ShowMarketInfoCommand = new RelayCommand(ShowMarketInfo, Can);

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

            CoinsForCalculator = new Coin[] { coins[0], coins[1], coins[2] };
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

        private void ShowSettings(object obj)
        {
            var mainWindow = obj as Window;

            SettingsWindow settingsWin = new SettingsWindow();
            settingsWin.Owner = mainWindow;
            settingsWin.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            settingsWin.Show();
        }

        private void ShowCalculator(object obj)
        {
            var calculatorWindow = new CalculatorViewModal(CoinsForCalculator);

            bool? result = _windowService.ShowDialog(calculatorWindow);

            if (result == true)
            {
                if (!calculatorWindow.first)
                {
                    numberOfCoin = 1;
                }
                else if (!calculatorWindow.second)
                {
                    numberOfCoin = 2;
                }
                else if (!calculatorWindow.third)
                {
                    numberOfCoin = 3;
                }
                else
                {
                    numberOfCoin = 0;
                }
            }
        }

        private void ShowMarketInfo(object parameter)
        {
            if (parameter is Coin coin && numberOfCoin == 0)
            {
                var MarketWindow = new MarketInfoViewModal(coin.id, coin.name);
                _windowService.ShowDialog(MarketWindow);
            }
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

        private void ExecuteCoinSelected(object parameter)
        {
            if (parameter is Coin coin)
            {
                if(numberOfCoin != 0)
                {
                    CoinsForCalculator[numberOfCoin - 1] = coin;
                    numberOfCoin = 0;
                    ShowCalculator(null);
                }
            }
        }
    }
}
