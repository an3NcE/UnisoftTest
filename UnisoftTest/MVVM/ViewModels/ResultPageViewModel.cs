
using PropertyChanged;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Input;
using UnisoftTest.MVVM.Models;
using UnisoftTest.MVVM.Views;
using static SQLite.SQLite3;

namespace UnisoftTest.MVVM.ViewModels
{
    [AddINotifyPropertyChangedInterface]
    public class ResultPageViewModel
    {
        public List<AutoItScript> FavScripts { get; set; }
        public AutoItScript CurrentFAVScript { get; set; }

        public AppSettings AppSettingsExePath { get; set; }

        public ICommand GoToConfPage => new Command(GoToConfigurationPage);
        public ICommand RunScript => new Command(RunTestScript);

        public string resultPath;
        public string result = "Proszę ustawić się w odpowiednim module ZSI przed rozpoczęciem testu.";
        public string ResultEditor { get; set; }
        

        public ResultPageViewModel()
        {
            
            ResultEditor = result;
            CurrentFAVScript = new AutoItScript();
            Refresh();
            //rejestracja strony
            //Routing.RegisterRoute(nameof(ConfigurationPage), typeof(ConfigurationPage));
            
        }

        public void Refresh()
        {
            FavScripts = App.BaseRepo.GetAllFav();
        }
        private async void RunTestScript(object obj)
        {
            AppSettingsExePath = App.BaseRepo.GetPathExe(0);
            var currentFavScript = obj as AutoItScript;

            try
            {
                // Ścieżka do AutoIt3.exe (interpretator AutoIt)
                string autoItInterpreterPath = AppSettingsExePath.SettingsValue;

                // Ścieżka do skryptu .au3
                string autoItScriptPath = currentFavScript.ScriptPath;

                // Przygotowanie procesu do uruchomienia skryptu
                var processStartInfo = new ProcessStartInfo
                {
                    FileName = autoItInterpreterPath,
                    Arguments = $"\"{autoItScriptPath}\"",
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    CreateNoWindow = true
                };

                // Uruchomienie procesu
                using (var process = Process.Start(processStartInfo))
                {
                    if (process != null)
                    {

                        resultPath = await process.StandardOutput.ReadToEndAsync();
                        resultPath = resultPath ;
                        ReadResultFile();
                        //await DisplayAlert("Wynik", $"Plik zapisano pod ścieżką: {result}", "OK");
                        MessagingCenter.Send(this, "Alert", $"Plik zapisano pod ścieżką: {resultPath}");

                        
                    }
                }
            }
            catch (Exception ex)
            {

                //await DisplayAlert("Błąd", $"Nie udało się uruchomić skryptu: {ex.Message}", "OK");
                MessagingCenter.Send(this, "Alert", $"Nie udało się uruchomić skryptu: {ex.Message}");
            }
        }

        async Task ReadResultFile()
        {
            try
            {
                var processStartInfo = new ProcessStartInfo
                {
                    FileName = "powershell.exe", // Uruchomienie PowerShella
                    Arguments = $"-Command Get-Content \"{resultPath}\" -Encoding UTF8 | Select-Object -Last 1", // Polecenie do odczytania ostatniej linii
                    UseShellExecute = false,
                    RedirectStandardOutput = true, // Przechwycenie wyniku z PowerShella
                    CreateNoWindow = true
                };

                using (var process = Process.Start(processStartInfo))
                {
                    // Przechwycenie standardowego wyjścia (ostatnia linia pliku)
                    result = process.StandardOutput.ReadToEnd();
                    
                    
                    process.WaitForExit();  // Czekanie na zakończenie procesu
                    Console.WriteLine("Ostatnia linia z pliku: " + result.Trim()); // Opcjonalnie wypisanie ostatniej linii
                    result = result.Replace(";", "\n");
                    ResultEditor = result;
                    

                    
                }
                
            }
            catch (Exception ex)
            {

                Debug.WriteLine(ex);
            }

        }

        //public string ResultEditor
        //{
        //    get => result;
        //    set
        //    {

        //        result = value;
        //        //OnPropertyChanged(nameof(ResultEditor));
        //    }

        //}

        //public event PropertyChangedEventHandler PropertyChanged;

        //protected virtual void OnPropertyChanged(string propertyName)
        //{
        //    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        //}

        private async void GoToConfigurationPage(object obj)
        {
            //(Application.Current.MainPage as NavigationPage)?.PushAsync(new ConfigurationPage());
            //await Shell.Current.GoToAsync(nameof(ConfigurationPage));
            await Shell.Current.GoToAsync("ConfigurationPageRoute");

        }
    }
}
