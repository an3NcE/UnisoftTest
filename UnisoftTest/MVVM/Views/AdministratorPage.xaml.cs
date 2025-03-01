using UnisoftTest.MVVM.ViewModels;
using UniToolbox.MVVM.ViewModels;

namespace UnisoftTest.MVVM.Views;


public partial class AdministratorPage : ContentPage
{
	public AdministratorPage()
	{
		InitializeComponent();

		BindingContext = new AdministratorPageViewModel();
	}
}