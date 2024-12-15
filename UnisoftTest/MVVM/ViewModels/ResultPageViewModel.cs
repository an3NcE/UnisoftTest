using PropertyChanged;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
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
        public string result;

        public ResultPageViewModel()
        {
            CurrentFAVScript = new AutoItScript();
            Refresh();
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
                    result = result.Replace("\t", "\n");
                    process.WaitForExit();  // Czekanie na zakończenie procesu
                    Console.WriteLine("Ostatnia linia z pliku: " + result.Trim()); // Opcjonalnie wypisanie ostatniej linii
                }
                ////using var stream = await FileSystem.(resultPath);
                ////using var stream = await FileSystem.(resultPath);
                //using var reader = new StreamReader(resultPath);
                ////using var reader = new StreamReader(stream);

                //while (reader.Peek() != -1)
                //{
                //    result = await reader.ReadLineAsync();

                //}
            }
            catch (Exception ex)
            {

                Debug.WriteLine(ex);
            }
            
        }

        private void GoToConfigurationPage(object obj)
        {
            (Application.Current.MainPage as NavigationPage)?.PushAsync(new ConfigurationPage());
        }
    }
}
