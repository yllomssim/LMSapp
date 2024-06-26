namespace LMSGUIApp.Pages;

public partial class TaxProfits : ContentPage
{
    public TaxProfits()
    {
        InitializeComponent();
        BindingContext = new MainViewModel();
    }
}