namespace LMSGUIApp.Pages
{
    public partial class DeleteLivestock : ContentPage
    {
        private MainViewModel _viewModel;
        private ContentPage _parentPage;
        private Database _database; // Define the _database field
        public DeleteLivestockViewModel DeleteLivestockViewModel { get; }

        public DeleteLivestock(ContentPage parentPage, MainViewModel viewModel)
        {
            InitializeComponent();
            _viewModel = viewModel;
            _database = Database.GetInstance(); // Initialize _database using the GetInstance method
            DeleteLivestockViewModel = new DeleteLivestockViewModel(_database, viewModel); // Initialize the DeleteLivestockViewModel
            BindingContext = DeleteLivestockViewModel; // Set BindingContext to DeleteLivestockViewModel
            _parentPage = parentPage; // Assign the parent page
        }

        // Display the details of the livestock ID entered
        private async void OnDisplayDetailsClicked(object sender, EventArgs e)
        {
            if (int.TryParse(IdEntry.Text, out int enteredId))
            {
                var livestock = _viewModel.Livestock.FirstOrDefault(l => l.Id == enteredId);
                if (livestock != null)
                {
                    DisplayLivestockDetails(livestock);
                    LivestockDetailsStackLayout.IsVisible = true;
                }
                else
                {
                    await _parentPage.DisplayAlert("Error", "Livestock not found", "OK");
                    LivestockDetailsStackLayout.IsVisible = false; // Hide details stack layout
                }
            }
            else
            {
                await _parentPage.DisplayAlert("Error", "Invalid ID", "OK");
                LivestockDetailsStackLayout.IsVisible = false; // Hide details stack layout
            }
        }

        private void DisplayLivestockDetails(Livestock livestock)
        {
            LivestockIdLabel.Text = $"{livestock.Id}";
            LivestockTypeLabel.Text = $"{livestock.Type}";
            LivestockCostLabel.Text = $"{livestock.Cost:C2}";
            LivestockWeightLabel.Text = $"{livestock.Weight} kg";
            LivestockColourLabel.Text = $"{livestock.Colour}";

            if (livestock is Sheep sheep)
            {
                LivestockWoolLabel.Text = $"{sheep.Wool} kg";
                WoolStackLayout.IsVisible = true;
                MilkStackLayout.IsVisible = false;
            }
            else if (livestock is Cow cow)
            {
                LivestockMilkLabel.Text = $"{cow.Milk} kg";
                MilkStackLayout.IsVisible = true;
                WoolStackLayout.IsVisible = false;
            }
            else
            {
                WoolStackLayout.IsVisible = false;
                MilkStackLayout.IsVisible = false;
            }
        }

        private async void OnDeleteClicked(object sender, EventArgs e)
        {
            if (int.TryParse(IdEntry.Text, out int enteredId))
            {
                var livestock = _viewModel.Livestock.FirstOrDefault(l => l.Id == enteredId);
                if (livestock != null)
                {
                    // Remove the livestock from the database
                    _database.DeleteItem(livestock);

                    // Remove the livestock from the collection
                    _viewModel.Livestock.Remove(livestock);

                    // Capture the details before removing
                    int id = livestock.Id;
                    float cost = livestock.Cost;
                    float weight = livestock.Weight;
                    string color = livestock.Colour ?? "";
                    string produce = "";
                    string type = livestock.GetType().Name;
                    if (livestock is Cow cow)
                    {
                        produce = cow.Milk.ToString();
                    }
                    else if (livestock is Sheep sheep)
                    {
                        produce = sheep.Wool.ToString();
                    }

                    await _parentPage.DisplayAlert("Success", $"Livestock Deleted:\n ID - {id}\n Type - {type}\n Cost - {cost:C2}\n Weight - {weight}\n Colour - {color}\n Produce - {produce}", "OK");
                    LivestockDetailsStackLayout.IsVisible = false; // Hide details stack layout
                    IdEntry.Text = string.Empty;

                    // Refresh the UI on the View All Livestock page
                    await RefreshViewAllLivestockPage();
                }
                else
                {
                    await _parentPage.DisplayAlert("Error", "Livestock not found", "OK");
                }
            }
            else
            {
                await _parentPage.DisplayAlert("Error", "Invalid ID", "OK");
            }
        }

        private async Task RefreshViewAllLivestockPage()
        {
            var viewAllLivestockPage = new ViewAllLivestock();

            // Ensure the current page is in the navigation stack
            if (Navigation.NavigationStack.Contains(this))
            {
                Navigation.InsertPageBefore(viewAllLivestockPage, this);
            }
            else
            {
                // Push the current page onto the stack first
                await Navigation.PushAsync(this);
                Navigation.InsertPageBefore(viewAllLivestockPage, this);
            }
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            RefreshData();
        }

        public void RefreshData()
        {
            var livestockData = _database.GetAllLivestock();
            _viewModel.Livestock.Clear();
            foreach (var livestock in livestockData)
            {
                _viewModel.Livestock.Add(livestock);
            }
        }

        private void OnResetClicked(object sender, EventArgs e)
        {
            IdEntry.Text = string.Empty;
            LivestockDetailsStackLayout.IsVisible = false; // Hide details stack layout
        }

        private void OnCloseDetailsClicked(object sender, EventArgs e)
        {
            LivestockDetailsStackLayout.IsVisible = false; // Hide details stack layout
        }
    }
}