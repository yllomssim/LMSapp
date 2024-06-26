namespace LMSGUIApp.Pages
{
    public partial class ViewAllLivestock : ContentPage
    {
        private MainViewModel vm;
        private Database _database;

        public ViewAllLivestock()
        {
            InitializeComponent();
            vm = new MainViewModel();
            BindingContext = vm;
            _database = new Database();
        }
    }
}