using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO.Packaging;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Markup;
using Crypto.Commands;
using Crypto.Modal;
using static System.Net.Mime.MediaTypeNames;

namespace Crypto.ViewModal
{
    public class CalculatorViewModal : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string name = null) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));

        private const decimal MaxValue = 100000000000000000;
        private decimal _firstBox;
        public decimal FirstBox
        {
            get => _firstBox;
            set
            {
                var number = Math.Min(value, MaxValue);
                _firstBox = number;
                _secondBox = Math.Round(number * (FirstPrice / SecondPrice), 4);
                _thirdBox = Math.Round(number * (FirstPrice / ThirdPrice), 4);
                OnPropertyChanged(nameof(FirstBox));
                OnPropertyChanged(nameof(SecondBox));
                OnPropertyChanged(nameof(ThirdBox));
            }
        }

        private decimal _secondBox;
        public decimal SecondBox
        {
            get => _secondBox;
            set
            {
                var number = Math.Min(value, MaxValue);
                _secondBox = number;
                _firstBox = Math.Round(number * (SecondPrice / FirstPrice), 4);
                _thirdBox = Math.Round(number * (SecondPrice / ThirdPrice), 4);
                OnPropertyChanged(nameof(FirstBox));
                OnPropertyChanged(nameof(SecondBox));
                OnPropertyChanged(nameof(ThirdBox));
            }
        }

        private decimal _thirdBox;

        public decimal ThirdBox
        {
            get => _thirdBox;
            set
            {
                var number = Math.Min(value, MaxValue);
                _thirdBox = number;
                _firstBox = number * (ThirdPrice / FirstPrice);
                _secondBox = number * (ThirdPrice / SecondPrice);
                OnPropertyChanged(nameof(FirstBox));
                OnPropertyChanged(nameof(SecondBox));
                OnPropertyChanged(nameof(ThirdBox));
            }
        }

        private decimal FirstPrice;
        private decimal SecondPrice;
        private decimal ThirdPrice;
        public string FirstCoin { get; private set; }
        public string SecondCoin { get; private set; }
        public string ThirdCoin { get; private set; }

        public bool first = true;

        public bool second = true;

        public bool third = true;

        public ICommand FirstButton { get; }

        public ICommand SecondButton { get; }

        public ICommand ThirdButton { get;  }
        public CalculatorViewModal(Coin[] coins)
        {
            FirstPrice = coins[0].current_price;
            SecondPrice = coins[1].current_price;
            ThirdPrice = coins[2].current_price;

            FirstCoin = coins[0].name;
            SecondCoin = coins[1].name;
            ThirdCoin = coins[2].name;
            FirstBox = 1;

            FirstButton = new RelayCommand(ChangeOne,Can);

            SecondButton = new RelayCommand(ChangeTwo, Can); ;

            ThirdButton = new RelayCommand(ChangeThree, Can); ;
        }

        private void ChangeOne(object obj)
        {
            first = false;
            var window = System.Windows.Application.Current.Windows
                         .OfType<Window>()
                         .Single(w => w.DataContext == this);
           window.DialogResult = true;
           window.Close();
        }

        private void ChangeTwo(object obj)
        {
            second = false;
            var window = System.Windows.Application.Current.Windows
                         .OfType<Window>()
                         .Single(w => w.DataContext == this);
            window.DialogResult = true;
            window.Close();
        }

        private void ChangeThree(object obj)
        {
            third = false;
            var window = System.Windows.Application.Current.Windows
                         .OfType<Window>()
                         .Single(w => w.DataContext == this);
            window.DialogResult = true;
            window.Close();
        }

        private bool Can(object obj)
        {
            return true;
        }

    }
}
