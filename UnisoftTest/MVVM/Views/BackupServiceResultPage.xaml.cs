using unisofttest.MVVM.ViewModels;

namespace UnisoftTest.MVVM.Views;

public partial class BackupServiceResultPage : ContentPage
{
	public BackupServiceResultPage()
	{
		InitializeComponent();
		BindingContext = new BackupServiceResultPageViewModel();
	}
}