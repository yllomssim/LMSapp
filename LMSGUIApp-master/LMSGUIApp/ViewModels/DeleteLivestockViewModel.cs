namespace LMSGUIApp.ViewModels
{
    public class DeleteLivestockViewModel : ViewModelBase
    {
        private readonly Database _database;
        private readonly MainViewModel _viewModel;

        public DeleteLivestockViewModel(Database database, MainViewModel viewModel)
        {
            _database = database;
            _viewModel = viewModel;
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