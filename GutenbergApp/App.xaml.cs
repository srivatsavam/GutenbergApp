using GutenbergApp.Services;
using GutenbergApp.Views;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace GutenbergApp
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            MainPage = new HomePageView();

            DependencyService.RegisterSingleton<BooksManagerService>(new BooksManagerService());

            _ = DependencyService.Resolve<BooksManagerService>();
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
