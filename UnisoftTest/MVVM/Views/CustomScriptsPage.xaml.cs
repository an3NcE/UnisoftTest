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
        base.OnAppearing();

        if (BindingContext is CustomScriptsPageViewModel viewModel)
        {
            viewModel.Refresh();
        }
    }
}