namespace LMSGUIApp.Pages
{
    public partial class UpdateLivestock : ContentPage
    {
        private Database _database;
        private ContentPage _parentPage;
        private DeleteLivestock _deletePage; // Added deletePage parameter
        private UpdateLivestockViewModel _viewModel;
        private Livestock _selectedLivestock;
        public UpdateLivestockViewModel UpdateLivestockViewModel { get; }

        public UpdateLivestock(ContentPage parentPage, DeleteLivestock deletePage, MainViewModel mainViewModel)
        {
            InitializeComponent();
            _database = Database.GetInstance();
            _viewModel = new UpdateLivestockViewModel(_database, mainViewModel); // Pass the required parameters
            BindingContext = _viewModel;
            _parentPage = parentPage;
            _deletePage = deletePage; // Initialize deletePage
        }

        private async void OnResetClicked(object sender, EventArgs e)
        {
            IdEntry.Text = string.Empty;
            CostEntry.Text = string.Empty;
            WeightEntry.Text = string.Empty;
            ProduceEntry.Text = string.Empty;
            LivestockDetailsLayout.IsVisible = false;
        }

        private async void OnDisplayDetailsClicked(object sender, EventArgs e)
        {
            int enteredId;
            if (int.TryParse(IdEntry.Text, out enteredId))
            {
                _selectedLivestock = _database.GetLivestockById(enteredId);
                if (_selectedLivestock != null)
                {
                    _viewModel.SelectedLivestock = _selectedLivestock;
                    DisplayLivestockDetails(_selectedLivestock);
                }
                else
                {
                    await _parentPage.DisplayAlert("Error", "Livestock not found", "OK");
                    LivestockDetailsLayout.IsVisible = false;
                }
            }
            else
            {
                await _parentPage.DisplayAlert("Error", "Invalid ID", "OK");
                LivestockDetailsLayout.IsVisible = false;
            }
        }

        private void OnSubmitClicked(object sender, EventArgs e)
        {
            _viewModel.UpdateLivestockCommand.Execute(null);
            OnResetClicked(sender, e);

            // Refresh the delete page data
            _deletePage.RefreshData();

            // Navigate back to the previous page
            Navigation.PopAsync(); // This will call OnAppearing() in DeleteLivestock
        }

        private async void OnCloseDetailsClicked(object sender, EventArgs e)
        {
            LivestockDetailsLayout.IsVisible = false;
        }

        private void DisplayLivestockDetails(Livestock livestock)
        {
            LivestockIdLabel.Text = $"{livestock.Id}";
            LivestockCostLabel.Text = $"{livestock.Cost:C2}";
            LivestockWeightLabel.Text = $"{livestock.Weight:F1} kg";
            LivestockColourLabel.Text = $"{livestock.Colour}";

            if (livestock is Sheep sheep)
            {
                LivestockTypeLabel.Text = "Sheep";
                LivestockWoolLabel.Text = $"{sheep.Wool:F1} kg";
                WoolStackLayout.IsVisible = true;
                MilkStackLayout.IsVisible = false;
                _viewModel.UpdateAvailableColors();
            }
            else if (livestock is Cow cow)
            {
                LivestockTypeLabel.Text = "Cow";
                LivestockMilkLabel.Text = $"{cow.Milk:F1} kg";
                MilkStackLayout.IsVisible = true;
                WoolStackLayout.IsVisible = false;
                _viewModel.UpdateAvailableColors();
            }

            LivestockDetailsLayout.IsVisible = true;
        }
    }
}