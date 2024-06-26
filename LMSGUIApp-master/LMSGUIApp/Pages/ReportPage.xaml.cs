namespace LMSGUIApp.Pages;

public partial class ReportPage : ContentPage
{
    MainViewModel vm;
    public ReportPage(MainViewModel vm)
    {
        InitializeComponent();
        this.vm = vm;

        //how many drop down options there are for the PICKER -- static option.
        LivestockPicker.ItemsSource = new string[] { "Cow", "Sheep", };
    }

    private void OnLivestockTypeSelectionChange(object sender, EventArgs e)
    {
        var picker = (Picker)sender;
        int selectedIndex = picker.SelectedIndex;
        if (selectedIndex == -1) return;
        string type = (string)picker.ItemsSource[selectedIndex];
        ResultLabel.Text = vm.QueryByLivestockType(type);
    }
}