using PropertyChanged;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using UnisoftTest.MVVM.Models;
using System.Diagnostics;

namespace UnisoftTest.MVVM.ViewModels
{
    [AddINotifyPropertyChangedInterface]
    public class CustomScriptsPageViewModel
    {
        public bool isAdministrator { get; set; }
        public AppSettings AppSettingsAdministrator { get; set; }
        public List<CustomScripts> CustomScripts { get; set; }
        public CustomScripts CurrentScript { get; set; }

        public ICommand AddOrUpdateCommand => new Command(AddOrUpdateComm);
        public ICommand DeleteCommand => new Command(DeleteComm);

        public ICommand RunScript => new Command(RunCustomScript);
        public string ResultLabel { get; set; }
        public CustomScriptsPageViewModel()
        {
            Refresh();
        }
        public void Refresh()
        {
            //ResultLabel = "test";
            CustomScripts = App.BaseRepo.GetAllCustomScripts();
            CurrentScript = new CustomScripts();

            AppSettingsAdministrator = App.BaseRepo.GetAdministratorStatus();
            
            if (AppSettingsAdministrator.SettingsValue == "0")
            {
                isAdministrator = false;
            }
            else
            {
                isAdministrator = true;
            }
            //AppSettingsExePath = new AppSettings();

        }

        private async void AddOrUpdateComm()
        {


            if (CurrentScript == null)
            {
                CurrentScript = new CustomScripts();
            }

            if (!string.IsNullOrEmpty(CurrentScript.CustomScriptName)  && !string.IsNullOrEmpty(CurrentScript.CustomScriptSQL) && !string.IsNullOrEmpty(CurrentScript.CustomScriptCMD))
            {
                App.BaseRepo.AddOrUpdateCustomScript(CurrentScript);

                Debug.WriteLine(App.BaseRepo.StatusMessage);
                MessagingCenter.Send(this, "Alert", "Dodano skrypt!");
                Refresh();
                

            }
            else
            {
                MessagingCenter.Send(this, "Alert", "Wypełnij wszystkie pola.");
            }

        }

        private void DeleteComm(object obj)
        {
            App.BaseRepo.DeleteCustomScript(CurrentScript.CustomScriptId);
            MessagingCenter.Send(this, "Alert", "Usunięto skrypt!");
            Refresh();
        }

        private async void RunCustomScript()
        {
            if (CurrentScript == null)
            {
                return;
            }

            if (CurrentScript.CustomScriptSQL.ToLower().Contains("drop") || CurrentScript.CustomScriptSQL.ToLower().Contains("delete"))
            {
                MessagingCenter.Send(this, "Alert", "Niewłaściwy skrypt!");
                return;
            }

            

            if (CurrentScript.CustomScriptCMD == null || CurrentScript.CustomScriptSQL == null)
            {
                ResultLabel = "Niepoprawny skrypt!";
                return;
            }

            string txtScript = CurrentScript.CustomScriptCMD;

            string sqlFilePath = Path.Combine(Path.GetTempPath(), "customScript.sql");

            //ResultEditor = txtScript;
            try
            {
                // Włącz ikonę ładowania
                //LoadingIcon(true);

                File.WriteAllText(sqlFilePath, CurrentScript.CustomScriptSQL);

                // Wykonaj ciężkie zadanie w tle
                await Task.Run(() =>
                {
                    // Symulacja ciężkiego zadania
                    Process process = new Process();
                    process.StartInfo.FileName = "cmd.exe";
                    process.StartInfo.Arguments = $"/c {txtScript} @{sqlFilePath}";
                    //process.StartInfo.Arguments = $"/c {txtScript}";
                    process.StartInfo.RedirectStandardOutput = true;
                    process.StartInfo.RedirectStandardError = true;
                    process.StartInfo.UseShellExecute = false;
                    process.StartInfo.CreateNoWindow = true;

                    process.Start();

                    // Czytaj dane wyjściowe na bieżąco
                    while (!process.StandardOutput.EndOfStream)
                    {
                        string line = process.StandardOutput.ReadLine();
                        ResultLabel += line + Environment.NewLine; // Dodanie do wyniku
                        //PropertyChanged(nameof(ResultEditor)); // Powiadomienie o zmianie
                    }

                    process.WaitForExit();
                });
            }
            catch (Exception ex)
            {
                ResultLabel = $"Błąd: {ex.Message}";
            }
            finally
            {
                // Wyłącz ikonę ładowania
                //LoadingIcon(false);
            }
            File.Delete(sqlFilePath);
            ResultLabel += "Skrypt wykonany prawidłowo, baza zaktualizowana!";
        }
    }
}
