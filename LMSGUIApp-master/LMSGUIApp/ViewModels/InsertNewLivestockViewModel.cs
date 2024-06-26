using System.Windows.Input;

namespace LMSGUIApp.ViewModels
{
    public class InsertNewLivestockViewModel : ViewModelBase
    {
        private readonly Database _database;
        private readonly MainViewModel _mainViewModel;

        public InsertNewLivestockViewModel(Database database, MainViewModel mainViewModel)
        {
            _database = database;
            _mainViewModel = mainViewModel;
            InsertNewLivestockCommand = new Command(async () => await InsertNewLivestockAsync());
        }

        private float _cost;
        public float Cost
        {
            get => _cost;
            set => SetProperty(ref _cost, value);
        }

        private float _weight;
        public float Weight
        {
            get => _weight;
            set => SetProperty(ref _weight, value);
        }

        private float _produce;
        public float Produce
        {
            get => _produce;
            set => SetProperty(ref _produce, value);
        }

        public ICommand InsertNewLivestockCommand { get; }

        private async Task InsertNewLivestockAsync()
        {
            if (Cost <= 0 || Weight <= 0 || Produce <= 0)
            {
                await Application.Current.MainPage.DisplayAlert("Error", "Cost, weight, and produce must be positive numbers.", "OK");
                return;
            }

            var livestock = new Livestock
            {
                Cost = Cost,
                Weight = Weight
            };

            // Call the database insert method
            _database.InsertItem(livestock);

            // Refresh the data in the MainViewModel
            _mainViewModel.RefreshData();

            // Show success message
            await Application.Current.MainPage.DisplayAlert("Success", "Livestock inserted successfully!", "OK");
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