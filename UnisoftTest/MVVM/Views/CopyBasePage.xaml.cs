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
        MessagingCenter.Subscribe<CopyBasePageViewModel, string>(this, "Alert", async (sender, message) =>
        {
            await DisplayAlert("Informacja - Kopiowanie bazy.", message, "OK");
        });
        base.OnAppearing();
        if (BindingContext is CopyBasePageViewModel viewModel)
        {

            viewModel.Refresh();

        }
    }
    protected override void OnDisappearing()
    {
        base.OnDisappearing();
        MessagingCenter.Unsubscribe<CopyBasePageViewModel, string>(this, "Alert");
    }

    private void FocusOnLastLine(object sender, TextChangedEventArgs e)
    {

        resultEditorFocus.CursorPosition = resultEditorFocus.Text.Length - 1;
        resultEditorFocus.Focus();

    }
}