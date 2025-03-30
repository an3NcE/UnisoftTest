using CommunityToolkit.Maui.Views;
using Microsoft.Maui.Controls;

namespace unisofttest.MVVM.Views;

public partial class LoginPopUpPage : Popup
{
	public LoginPopUpPage()
	{
		InitializeComponent();
	}

    private void Button_Close(object sender, EventArgs e)
    {
        pwEntry.Text = "Close";

        Close();
    }

    public string GetPasswordEntry()
    {
        return pwEntry.Text;
    }

    private void Button_Loggin(object sender, EventArgs e)
    {
        Close();
    }
}