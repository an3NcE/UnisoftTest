using UnisoftTest.MVVM.ViewModels;

namespace UnisoftTest.MVVM.Views;

public partial class BackupServicePage : ContentPage
{
	public BackupServicePage()
	{
		InitializeComponent();
		BindingContext = new BackupServicePageViewModel();
	}
}