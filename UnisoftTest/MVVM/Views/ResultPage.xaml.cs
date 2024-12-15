using UnisoftTest.MVVM.ViewModels;

namespace UnisoftTest.MVVM.Views;

public partial class ResultPage : ContentPage
{
	public ResultPage()
	{
        MessagingCenter.Subscribe<ResultPageViewModel, string>(this, "Alert", async (sender, message) =>
        {
            await DisplayAlert("Informacja", message, "OK");
        });

        InitializeComponent();
		BindingContext = new ResultPageViewModel();
        NavigationPage.SetHasNavigationBar(this, false);
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        if (BindingContext is ResultPageViewModel viewModel)
        {
            
            viewModel.Refresh();
            
        }
    }

    protected override void OnDisappearing()
    {
        base.OnDisappearing();
        MessagingCenter.Unsubscribe<ResultPageViewModel, string>(this, "Alert");
    }
}