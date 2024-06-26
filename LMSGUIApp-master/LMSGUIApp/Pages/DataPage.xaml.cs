//Main Menu Page when opening the program

namespace LMSGUIApp.Pages;
public partial class DataPage : ContentPage
{
    public DataPage(MainViewModel vm)
    {
        InitializeComponent();
        BindingContext = vm;
    }
    //Navigate to View All Livestock 
    private async void OnNavigateButtonClickedViewAll(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new Pages.ViewAllLivestock());
    }
    //Navigate to Tax & Profits
    private async void OnNavigateButtonClickedTaxProfits(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new Pages.TaxProfits());
    }
    //Navigate to Livestock Data
    private async void OnNavigateButtonClickedCurrentLivestockData(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new Pages.CurrentLivestockData());
    }
    //Navigate to Livestock Query
    private async void OnNavigateButtonClickedLivestockQuery(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new Pages.LivestockQuery());
    }
    //Navigate to Figure Estimations
    private async void OnNavigateButtonClickedFutureEstimations(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new Pages.FutureEstimations());
    }
    //Navigate to Edit Livestock
    private async void OnNavigateButtonClickedEditLivestock(object sender, EventArgs e)
    {
        MainViewModel mainViewModel = (MainViewModel)BindingContext;
        await Navigation.PushAsync(new Pages.EditLivestock(mainViewModel));
    }
    //Display Sign Up message
    private void ButtonClicked(object sender, EventArgs e)
    {
        DisplayAlert("Great!", " ' Welcome to Livestock Management '", "Exit");
    }
}