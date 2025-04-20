using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Crypto.View;
using Crypto.ViewModal;
using Microsoft.Extensions.DependencyInjection;

namespace Crypto.Modal
{
    public class WindowService : IWindowService
    {
        private readonly IServiceProvider _sp;
        public WindowService(IServiceProvider sp) => _sp = sp;

        public bool? ShowDialog<TViewModel>(TViewModel viewModel)
        {
            Window wnd = viewModel switch
            {
                MainViewModal => _sp.GetRequiredService<MainWindow>(),
                CalculatorViewModal => _sp.GetRequiredService<CalculatorWindow>(),
                _ => throw new InvalidOperationException($"Не знаю, яке вікно для {typeof(TViewModel).Name}")
            };

            wnd.DataContext = viewModel;
            wnd.Owner = Application.Current.MainWindow;
            wnd.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            return wnd.ShowDialog();
        }
    }
}
