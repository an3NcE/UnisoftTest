using UnisoftTest.MVVM.ViewModels;

namespace UnisoftTest.MVVM.Views;

public partial class CopyBasePage : ContentPage
{
	public CopyBasePage()
	{
		InitializeComponent();
		BindingContext = new CopyBasePageViewModel();
	}

    protected override void OnAppearing()
    {
        base.OnAppearing();
        if (BindingContext is CopyBasePageViewModel viewModel)
        {

            viewModel.Refresh();

        }
    }
}