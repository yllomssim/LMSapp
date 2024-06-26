namespace LMSGUIApp.Pages
{
    public partial class EditLivestock : ContentPage
    {
        private InsertNewLivestock _insertNewLivestockPage;
        private UpdateLivestock _updateLivestockPage;
        private DeleteLivestock _deleteLivestockPage;
        private MainViewModel _mainViewModel; // Add a field to hold MainViewModel

        public EditLivestock(MainViewModel mainViewModel)
        {
            InitializeComponent();
            _mainViewModel = mainViewModel; // Assign the passed MainViewModel object
            _insertNewLivestockPage = new InsertNewLivestock(this, _mainViewModel); // Pass mainViewModel as a parameter
            _deleteLivestockPage = new DeleteLivestock(this, _mainViewModel); // Pass mainViewModel as a parameter
            _updateLivestockPage = new UpdateLivestock(this, _deleteLivestockPage, _mainViewModel); // Pass _deleteLivestockPage as a parameter
        }

        private void OnInsertNewClicked(object sender, EventArgs e)
        {
            ContentArea.Content = _insertNewLivestockPage.Content;
            ContentArea.BindingContext = _insertNewLivestockPage.BindingContext;

            RefreshDatabase();
        }

        private void OnDeleteClicked(object sender, EventArgs e)
        {
            ContentArea.Content = _deleteLivestockPage.Content;
            ContentArea.BindingContext = _deleteLivestockPage.BindingContext;

            RefreshDatabase();
        }

        private void OnUpdateClicked(object sender, EventArgs e)
        {
            ContentArea.Content = _updateLivestockPage.Content;
            ContentArea.BindingContext = _updateLivestockPage.BindingContext;

            RefreshDatabase();
        }

        private void RefreshDatabase()
        {
            try
            {
                // Ensure each view model is initialized before calling RefreshData method
                if (_insertNewLivestockPage.InsertNewLivestockViewModel != null)
                {
                    _insertNewLivestockPage.InsertNewLivestockViewModel.RefreshData();
                }
                else
                {
                    Console.WriteLine("InsertNewLivestockViewModel is null.");
                }

                if (_deleteLivestockPage.DeleteLivestockViewModel != null)
                {
                    _deleteLivestockPage.DeleteLivestockViewModel.RefreshData();
                }
                else
                {
                    Console.WriteLine("DeleteLivestockViewModel is null.");
                }

                if (_updateLivestockPage.UpdateLivestockViewModel != null)
                {
                    _updateLivestockPage.UpdateLivestockViewModel.RefreshData();
                }
                else
                {
                    Console.WriteLine("UpdateLivestockViewModel is null.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
        }
    }
}