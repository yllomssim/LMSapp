namespace LMSGUIApp
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            MainPage = new AppShell();

            // Create an instance of MainViewModel
            var mainViewModel = new MainViewModel();

            // Pass MainViewModel to the DataPage constructor
            MainPage = new NavigationPage(new DataPage(mainViewModel));

            DependencyService.Register<MainViewModel>();
        }

    }
}
