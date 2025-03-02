using UnisoftTest.MVVM.ViewModels;

namespace UnisoftTest.MVVM.Views;

public partial class CustomScriptsPage : ContentPage
{
    public CustomScriptsPage()
    {
        
        InitializeComponent();
        BindingContext = new CustomScriptsPageViewModel();
    }
    protected override void OnAppearing()
    {
        MessagingCenter.Subscribe<CustomScriptsPageViewModel, string>(this, "Alert", async (sender, message) =>
        {
            await DisplayAlert("Informacja", message, "OK");
        });

        base.OnAppearing();

        if (BindingContext is CustomScriptsPageViewModel viewModel)
        {
            viewModel.Refresh();
        }
    }

    protected override void OnDisappearing()
    {
        base.OnDisappearing();
        MessagingCenter.Unsubscribe<CustomScriptsPageViewModel, string>(this, "Alert");
    }
}