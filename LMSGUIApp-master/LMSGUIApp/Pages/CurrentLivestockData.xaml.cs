using System.Xml.Schema;

namespace LMSGUIApp.Pages;

public partial class CurrentLivestockData : ContentPage
{
    public CurrentLivestockData()
    {
        InitializeComponent();
        BindingContext = new MainViewModel();
    }
}