using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
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

        private HttpCoinList httpCoinList;

        public ObservableCollection<Coin> coins { get; set; }

        public ICommand NextPage { get; set; }

        public ICommand BackPage { get; set; }

        public MainViewModal()
        {
            httpCoinList = new HttpCoinList();
            coins = new ObservableCollection<Coin>();

            NextPage = new RelayCommand(Next, Can);
            BackPage = new RelayCommand(Back, Can);

            LoadPage();
        }

        private async void LoadPage()
        {
            List<Coin> list = await httpCoinList.GetPage();

            foreach(var item in list)
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
            httpCoinList.MoveNext();
            coins.Clear();
            LoadPage();
        }

        private void Back(object obj)
        {
            httpCoinList.MoveBack();
            coins.Clear();
            LoadPage();
        }


    }
}
