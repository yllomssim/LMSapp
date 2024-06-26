namespace LMSGUIApp.Pages
{
    public partial class InsertNewLivestock : ContentPage
    {
        private readonly Database _database;
        private readonly ContentPage _parentPage;
        private readonly MainViewModel _mainViewModel;
        private readonly InsertNewLivestockViewModel _viewModel;
        public InsertNewLivestockViewModel InsertNewLivestockViewModel { get; }
        public ObservableCollection<string> Animals { get; set; }
        public ObservableCollection<string> Colours { get; set; }

        public InsertNewLivestock(ContentPage parentPage, MainViewModel mainViewModel)
        {
            InitializeComponent();
            _parentPage = parentPage;
            _database = new Database();
            _mainViewModel = mainViewModel;
            _viewModel = new InsertNewLivestockViewModel(_database, _mainViewModel);
            Animals = new ObservableCollection<string> { "Cow", "Sheep" };
            Colours = new ObservableCollection<string> { "Black", "White", "Red" };
            BindingContext = this;
        }

        private void OnResetClicked(object sender, EventArgs e)
        {
            // Clear all input fields and reset pickers
            AnimalPicker.SelectedIndex = -1;
            CostEntry.Text = string.Empty;
            WeightEntry.Text = string.Empty;
            ColourPicker.SelectedIndex = -1;
            ProduceEntry.Text = string.Empty;
        }

        private async void OnSubmitClicked(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(CostEntry.Text) || string.IsNullOrEmpty(WeightEntry.Text) || ColourPicker.SelectedItem == null || AnimalPicker.SelectedItem == null)
                {
                    await _parentPage.DisplayAlert("Error", "Please fill in all fields.", "OK");
                    return;
                }

                if (!float.TryParse(CostEntry.Text, out float cost) || cost <= 0 ||
                    !float.TryParse(WeightEntry.Text, out float weight) || weight <= 0)
                {
                    await _parentPage.DisplayAlert("Error", "Cost and Weight must be positive numbers.", "OK");
                    return;
                }

                var selectedAnimal = AnimalPicker.SelectedItem.ToString();
                var selectedColor = ColourPicker.SelectedItem.ToString();

                if (selectedAnimal == "Cow")
                {
                    if (float.TryParse(ProduceEntry.Text, out float milk) && milk > 0)
                    {
                        var newCow = new Cow { Cost = cost, Weight = weight, Milk = milk, Colour = selectedColor };
                        SaveLivestock(newCow, "Cow");
                    }
                    else
                    {
                        await _parentPage.DisplayAlert("Error", "Produce (Milk) must be a positive number.", "OK");
                    }
                }
                else if (selectedAnimal == "Sheep")
                {
                    if (selectedColor.Equals("Red", StringComparison.OrdinalIgnoreCase))
                    {
                        await _parentPage.DisplayAlert("Error", "Red sheep are not allowed.", "OK");
                        return;
                    }

                    if (float.TryParse(ProduceEntry.Text, out float wool) && wool > 0)
                    {
                        var newSheep = new Sheep { Cost = cost, Weight = weight, Wool = wool, Colour = selectedColor };
                        SaveLivestock(newSheep, "Sheep");
                    }
                    else
                    {
                        await _parentPage.DisplayAlert("Error", "Produce (Wool) must be a positive number.", "OK");
                    }
                }
                else
                {
                    await _parentPage.DisplayAlert("Error", "Unknown animal selected.", "OK");
                }
            }
            catch (Exception ex)
            {
                await _parentPage.DisplayAlert("Error", $"An unexpected error occurred: {ex.Message}", "OK");
            }
        }

        private async void SaveLivestock(Livestock livestock, string animalType)
        {
            int newId = _database.InsertItem(livestock);
            if (newId > 0)
            {
                await _parentPage.DisplayAlert("Success", $"New {animalType} added: \nID - {newId} \nType - {animalType}\nColour - {livestock.Colour}\nCost - {livestock.Cost:C2} \nWeight - {livestock.Weight} kg \nProduce - {ProduceEntry.Text} kg", "OK");
                OnResetClicked(null, null); // Reset the form after displaying the success alert
            }
            else
            {
                await _parentPage.DisplayAlert("Error", $"Failed to add {animalType}. Please try again.", "OK");
            }
        }

        private void OnAnimalPickerSelectedIndexChanged(object sender, EventArgs e)
        {
            string selectedAnimal = AnimalPicker.SelectedItem?.ToString();
            if (selectedAnimal == "Sheep")
            {
                Colours.Clear();
                Colours.Add("Black");
                Colours.Add("White");
            }
            else
            {
                Colours.Clear();
                Colours.Add("Black");
                Colours.Add("White");
                Colours.Add("Red");
            }
        }
        public void RefreshData()
        {
            var livestockData = _database.GetAllLivestock();
            _mainViewModel.Livestock.Clear();
            foreach (var livestock in livestockData)
            {
                _mainViewModel.Livestock.Add(livestock);
            }
        }
    }
}