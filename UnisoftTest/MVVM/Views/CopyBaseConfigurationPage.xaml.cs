using UniTest.MVVM.ViewModels;

namespace UnisoftTest.MVVM.Views;

public partial class CopyBaseConfigurationPage : ContentPage
{
	public CopyBaseConfigurationPage()
	{
		InitializeComponent();
        MessagingCenter.Subscribe<CopyBaseConfigurationPageViewModel, string>(this, "Alert", async (sender, message) =>
        {
            await DisplayAlert("B³¹d", message, "OK");
        });


        BindingContext = new CopyBaseConfigurationPageViewModel();
	}

    protected override void OnDisappearing()
    {
        base.OnDisappearing();
        MessagingCenter.Unsubscribe<CopyBaseConfigurationPageViewModel, string>(this, "Alert");
    }
}