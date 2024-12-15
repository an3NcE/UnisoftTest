using UnisoftTest.MVVM.ViewModels;

namespace UnisoftTest.MVVM.Views;

public partial class ResultPage : ContentPage
{
	public ResultPage()
	{
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
}