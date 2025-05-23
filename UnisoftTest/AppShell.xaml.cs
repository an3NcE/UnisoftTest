﻿using CommunityToolkit.Maui.Views;
using PropertyChanged;
using unisofttest.MVVM.Views;
using UnisoftTest.MVVM.Models;
using UnisoftTest.MVVM.Services;
using UnisoftTest.MVVM.Views;
using UniToolbox.MVVM.Models;

namespace UnisoftTest
{
    [AddINotifyPropertyChangedInterface]
    public partial class AppShell : Shell
    {
        public bool isAdministrator { get; set; }
        public bool isAdministratorChecked { get; set; }
        public List<Modules> AllModules { get; set; }

        public AppSettings AppSettingsPassword { get; set; }
        string secondAdminPW = "brakHasla";
        string password = "";
        public AppShell()
        {
            InitializeComponent();
            BindingContext = this;
            //App.BaseRepo.InitializeDatabaseAsync();


            //Routing.RegisterRoute("ResultPageHome", typeof(ResultPage)); // Rejestracja trasy dla ResultPage
            //Routing.RegisterRoute("ConfigurationPageRoute", typeof(ConfigurationPage)); // Rejestracja trasy dla ConfigurationPage
            //App.BaseRepo.AddOrUpdateAppAdministrator(true);
            isAdministrator = false;

            CheckModules();
        }

        private async Task CheckModules()
        {
            try
            {
                await App.BaseRepo.AddOrUpdateAppAdministrator(false);
            }
            catch (Exception)
            {
                fiCustomScriptsPage.IsVisible = false;
                fiCopyBasePage.IsVisible = false;
                fiBackupServicePage.IsVisible = false;
                fiConfigurationPage.IsVisible = false;
                throw;
            }
            
            AllModules = await App.BaseRepo.GetAllModules();
            int countPage = 0;
            if (AllModules[1].ModuleAccess == true)
            {
                fiConfigurationPage.IsVisible = true;
                countPage++;
            }
            else
            {
                fiConfigurationPage.IsVisible = false;
            }

            if (AllModules[2].ModuleAccess == true)
            {
                fiCopyBasePage.IsVisible = true;
                countPage++;
            }
            else
            {
                fiCopyBasePage.IsVisible = false;

            }

            if (AllModules[3].ModuleAccess == true)
            {
                fiCustomScriptsPage.IsVisible = true;
                countPage++;
            }
            else
            {
                fiCustomScriptsPage.IsVisible = false;
            }

            if (AllModules[4].ModuleAccess == true)
            {
                fiBackupServicePage.IsVisible = true;
                countPage++;
            }
            else
            {

                fiBackupServicePage.IsVisible = false;
            }
            if (AllModules[5].ModuleAccess == true)
            {
                fiBackupServiceResultPage.IsVisible = true;
                countPage++;
            }
            else
            {

                fiBackupServiceResultPage.IsVisible = false;
            }

            if (countPage == 0)
            {
                fiStartPage.IsVisible = true;

            }
            else
            {
                fiStartPage.IsVisible = false;
            }
        }

        public async Task<string> ShowPopupPasword(LoginPopUpPage loginPopUpPage)
        {
            await DatabasePasswordManager.EnsurePasswordAsync();
            await UnisoftTest.Services.AesCredentialManager.EnsureAsync();
            await this.ShowPopupAsync(loginPopUpPage);

            
            return loginPopUpPage.GetPasswordEntry(); 
        }

        private async void SetAdministrator(object sender, EventArgs e)
        {
            GetSecondPW();
            if (isAdministrator == true)
            {
                await Shell.Current.GoToAsync("//AdministratorPage");
            }

            if (isAdministrator == false && isAdministratorChecked == true)
            {
                //string password = await Application.Current.MainPage.DisplayPromptAsync(
                //                "Autoryzacja",
                //                "Podaj hasło serwisanta:",
                //                "OK", "Anuluj",
                //                placeholder: "Hasło",
                //                maxLength: 20,
                //                keyboard: Keyboard.Text);

                //this.ShowPopup(new LoginPopUpPage());

                //string password =""; 
                //string password = this.ShowPopup(new LoginPopUpPage()); 
                string password = await ShowPopupPasword(new LoginPopUpPage());

                

                // //do usuniecia!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
                if (password == "!@#$" || password == secondAdminPW)
                {
                    isAdministrator = true;
                    Shell.SetTabBarIsVisible(this, isAdministrator);
                    lblAdministrator.Text = "Jesteś serwisantem! :)";
                    App.BaseRepo.AddOrUpdateAppAdministrator(true);
                    isAdministratorChecked = true;
                    fiCopyBasePage.IsVisible = true;
                    fiConfigurationPage.IsVisible = true;
                    fiCustomScriptsPage.IsVisible = true;
                    fiBackupServicePage.IsVisible = true;
                    await Shell.Current.GoToAsync("//AdministratorPage");
                    //await Shell.Current.Navigation.PopToRootAsync();
                }
                else if (password=="Close")
                {
                    FalseAdministrator();
                }
                else
                {
                    await Application.Current.MainPage.DisplayAlert(
                               "Autoryzacja",
                               "Chyba nie jesteś serwisantem. :)",
                               "OK");
                    isAdministratorChecked = false;

                }
            }
            else
            {

                FalseAdministrator();
            }

        }

        public async void FalseAdministrator()
        {
            isAdministrator = false;
            Shell.SetTabBarIsVisible(this, isAdministrator);
            isAdministratorChecked = false;
            lblAdministrator.Text = "Czy jesteś serwisantem?";
            await App.BaseRepo.AddOrUpdateAppAdministrator(false);
            fiCopyBasePage.IsVisible = false;
            fiConfigurationPage.IsVisible = false;
            fiBackupServicePage.IsVisible = false;
            fiCustomScriptsPage.IsVisible = false;
            CheckModules();
        }

        private async void GetSecondPW()
        {


            AppSettingsPassword = await App.BaseRepo.GetSettings(2);
            if (AppSettingsPassword != null && AppSettingsPassword.SettingsValue != null)
            {
                secondAdminPW = AppSettingsPassword.SettingsValue;
            }


        }
    }
}

//private async void SetAdministrator(object sender, EventArgs e)
//{


//    if (isAdministrator == false)
//    {
//        string password = await Application.Current.MainPage.DisplayPromptAsync(
//                        "Autoryzacja",
//                        "Podaj hasło administratora:",
//                        "OK", "Anuluj",
//                        placeholder: "Hasło",
//                        maxLength: 20,
//                        keyboard: Keyboard.Text);
//        if (password == "opat")
//        {
//            isAdministrator = true;
//            lblAdministrator.Text = "Jesteś Administratorem! :)";
//            App.BaseRepo.AddOrUpdateAppAdministrator(true);
//            isAdministratorChecked = true;
//        }
//        else
//        {
//            await Application.Current.MainPage.DisplayAlert(
//                       "Autoryzacja",
//                       "Chyba nie jesteś administratorem. :)",
//                       "OK");
//            isAdministratorChecked = false;

//        }
//    }
//    else
//    {
//        isAdministrator = false;
//        isAdministratorChecked = false;
//        lblAdministrator.Text = "Czy jesteś Administratorem?";
//        App.BaseRepo.AddOrUpdateAppAdministrator(false);
//    }

//}