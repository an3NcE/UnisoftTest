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

    //private void cbVisualStatePage(object sender, CheckedChangedEventArgs e)
    //{
    //    if (BindingContext is AdministratorPageViewModel viewModel)
    //    {
    //        //var module = viewModel.CurrentModule;
    //        //module.ModuleAccess = e.Value; // Ustawienie nowego stanu ModuleAccess

    //        // Aktualizacja bazy danych
    //        viewModel.ChangeVisualState();
    //    }
    //}
}