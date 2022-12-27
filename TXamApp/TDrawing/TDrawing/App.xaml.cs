using System;
using TDrawing.Services;
using TDrawing.Views;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TDrawing
{
    public partial class App : Application
    {

        public App()
        {
            InitializeComponent();

            DependencyService.Register<MockDataStore>();
            MainPage = new LoginPage();
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
