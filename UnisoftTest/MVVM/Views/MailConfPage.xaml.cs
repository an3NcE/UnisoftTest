using UnisoftTest.MVVM.ViewModels;

namespace UnisoftTest.MVVM.Views;

public partial class MailConfPage : ContentPage
{
	public MailConfPage()
	{
		InitializeComponent();
		BindingContext = new MailConfPageViewModel();

    }
}