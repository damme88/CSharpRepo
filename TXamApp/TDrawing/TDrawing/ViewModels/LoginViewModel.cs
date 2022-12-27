using System;
using System.Collections.Generic;
using System.Text;
using TDrawing.Views;
using Xamarin.Essentials;
using Xamarin.Forms;
using System.Diagnostics;

namespace TDrawing.ViewModels
{
    public class LoginViewModel : BaseViewModel
    {
        public Command StartCommand { get; }
        public Command CloseCommand { get; }

        public LoginViewModel()
        {
            StartCommand = new Command(OnStartClicked);
            CloseCommand = new Command(OnCloseClicked);
        }

        private async void OnStartClicked(object obj)
        {
            Application.Current.MainPage = new AppShell();
            await Shell.Current.GoToAsync($"//{nameof(ItemsPage)}");
        }

        private void OnCloseClicked(object obj)
        {
            Process.GetCurrentProcess().Kill();
        }
    }
}
