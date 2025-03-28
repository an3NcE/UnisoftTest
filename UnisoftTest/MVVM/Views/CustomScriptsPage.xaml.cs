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
            await DisplayAlert("Informacja - Dodatkowe skrypty.", message, "OK");
        });

        base.OnAppearing();

        if (BindingContext is CustomScriptsPageViewModel viewModel)
        {
            viewModel.Refresh();
        }
    }

    //protected override void OnNavigatedTo(NavigatedToEventArgs args)
    //{
    //    base.OnNavigatedTo(args);

    //    if (BindingContext is CustomScriptsPageViewModel viewModel)
    //    {
    //        viewModel.Refresh();
    //    }
    //}



    protected override void OnDisappearing()
    {
        base.OnDisappearing();
        MessagingCenter.Unsubscribe<CustomScriptsPageViewModel, string>(this, "Alert");
    }
}