using System.Windows.Input;

namespace LMSGUIApp.ViewModels
{
    public class UpdateLivestockViewModel : ViewModelBase
    {
        private readonly Database _database;
        private readonly MainViewModel _viewModel;

        private float? _cost;
        public float? Cost
        {
            get => _cost;
            set => SetProperty(ref _cost, value);
        }

        private float? _weight;
        public float? Weight
        {
            get => _weight;
            set => SetProperty(ref _weight, value);
        }

        private float? _produce;
        public float? Produce
        {
            get => _produce;
            set => SetProperty(ref _produce, value);
        }

        private Livestock _selectedLivestock;
        public Livestock SelectedLivestock
        {
            get => _selectedLivestock;
            set => SetProperty(ref _selectedLivestock, value);
        }

        private ObservableCollection<string> _colours;
        public ObservableCollection<string> Colours
        {
            get => _colours;
            set => SetProperty(ref _colours, value);
        }

        private string _selectedColour;
        public string SelectedColour
        {
            get => _selectedColour;
            set => SetProperty(ref _selectedColour, value);
        }

        public ICommand UpdateLivestockCommand { get; }

        public UpdateLivestockViewModel(Database database, MainViewModel viewModel)
        {
            _database = database;
            _viewModel = viewModel;
            Colours = new ObservableCollection<string>();
            UpdateLivestockCommand = new Command(async () => await UpdateLivestockAsync());
        }

        private async Task UpdateLivestockAsync()
        {
            if (SelectedLivestock == null)
            {
                await Application.Current.MainPage.DisplayAlert("Error", "No livestock selected.", "OK");
                return;
            }

            if (Cost == null || Weight == null || Produce == null || Cost <= 0 || Weight <= 0 || Produce <= 0)
            {
                await Application.Current.MainPage.DisplayAlert("Error", "Cost, weight, and produce must be positive numbers.", "OK");
                return;
            }

            SelectedLivestock.Cost = (float)Cost;
            SelectedLivestock.Weight = (float)Weight;
            SelectedLivestock.Colour = SelectedColour;

            if (SelectedLivestock is Cow cow)
            {
                cow.Milk = (float)Produce;
            }
            else if (SelectedLivestock is Sheep sheep)
            {
                sheep.Wool = (float)Produce;
            }

            _database.UpdateItem(SelectedLivestock);

            string successMessage = $"Livestock has been updated:\n" +
                                    $"ID - {SelectedLivestock.Id}\n" +
                                    $"Cost - {Cost:C2}\n" +
                                    $"Weight - {Weight} kg\n" +
                                    $"Produce - {Produce} kg\n" +
                                    $"Colour - {SelectedColour}";

            await Application.Current.MainPage.DisplayAlert("Success", successMessage, "OK");

            // Reset the form after successful update
            ResetForm();
        }

        private void ResetForm()
        {
            Cost = null;
            Weight = null;
            Produce = null;
            SelectedColour = null;
            SelectedLivestock = null;
        }

        public void UpdateAvailableColors()
        {
            Colours.Clear();
            if (SelectedLivestock is Sheep)
            {
                Colours.Add("Black");
                Colours.Add("White");
            }
            else if (SelectedLivestock is Cow)
            {
                Colours.Add("Black");
                Colours.Add("White");
                Colours.Add("Red");
            }
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
    }
}