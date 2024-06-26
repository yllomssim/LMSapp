namespace LMSGUIApp.Pages
{
    public partial class LivestockQuery : ContentPage
    {
        private readonly LivestockQueryViewModel viewModel;

        public LivestockQuery()
        {
            InitializeComponent();
            viewModel = new LivestockQueryViewModel();
            BindingContext = viewModel;
        }

        private void TypePicker_SelectedIndexChanged(object sender, EventArgs e)
        {
            var selectedType = typePicker.SelectedItem as string;
            viewModel.SelectedType = selectedType;
        }

        private void ColorPicker_SelectedIndexChanged(object sender, EventArgs e)
        {
            var selectedColor = colorPicker.SelectedItem as string;
            viewModel.SelectedColor = selectedColor;
        }
    }
}