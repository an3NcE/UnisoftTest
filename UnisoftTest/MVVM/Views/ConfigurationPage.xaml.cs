using Microsoft.Maui.Controls;
using System.Diagnostics;
using UnisoftTest.MVVM.ViewModels;

namespace UnisoftTest.MVVM.Views;

public partial class ConfigurationPage : ContentPage
{
	public ConfigurationPage()
    {
        InitializeComponent();

        MessagingCenter.Subscribe<ConfigurationPageViewModel, string>(this, "Alert", async (sender, message) =>
        {
            await DisplayAlert("B³¹d", message, "OK");
        });

        BindingContext = new ConfigurationPageViewModel();
	}

    protected override void OnDisappearing()
    {
        base.OnDisappearing();
        MessagingCenter.Unsubscribe<ConfigurationPageViewModel, string>(this, "Alert");
    }

    
}