using UniTest.MVVM.ViewModels;

namespace UnisoftTest.MVVM.Views;

public partial class CopyBaseConfigurationPage : ContentPage
{
	public CopyBaseConfigurationPage()
	{
		InitializeComponent();

		BindingContext = new CopyBaseConfigurationPageViewModel();
	}
}