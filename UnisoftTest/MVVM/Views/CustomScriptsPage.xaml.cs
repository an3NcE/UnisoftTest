using UnisoftTest.MVVM.ViewModels;

namespace UnisoftTest.MVVM.Views;

public partial class CustomScriptsPage : ContentPage
{
	public CustomScriptsPage()
	{
		InitializeComponent();
        BindingContext = new CustomScriptsPageViewModel();
    }
}