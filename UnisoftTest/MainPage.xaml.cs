using System.Diagnostics;
using System.Runtime.InteropServices;

namespace UnisoftTest
{
    public partial class MainPage : ContentPage
    {
        

        public MainPage()
        {
            InitializeComponent();
            
        }

        private void OnCounterClicked(object sender, EventArgs e)
        {
            RunAutoItScript();
        }



        private async void RunAutoItScript()
        {
            try
            {
                // Ścieżka do AutoIt3.exe (interpretator AutoIt)
                string autoItInterpreterPath = @"Q:\Programy\autoIt\AutoIt3\AutoIt3.exe";

                // Ścieżka do skryptu .au3
                string autoItScriptPath = @"Q:\Programy\autoIt\script\test.au3";

                // Przygotowanie procesu do uruchomienia skryptu
                var processStartInfo = new ProcessStartInfo
                {
                    FileName = autoItInterpreterPath,
                    Arguments = $"\"{autoItScriptPath}\"",
                    UseShellExecute = false, // Umożliwia przekazywanie danych przez standardowe wyjście
                    RedirectStandardOutput = true, // Przechwytywanie wyjścia z procesu
                    CreateNoWindow = true // Ukrywa okno procesu
                };

                // Uruchomienie procesu
                using (var process = Process.Start(processStartInfo))
                {
                    if (process != null)
                    {
                        // Odbieranie wyniku (ścieżki do pliku) z wyjścia standardowego
                        string result = await process.StandardOutput.ReadToEndAsync();
                        await DisplayAlert("Wynik", $"Plik zapisano pod ścieżką: {result}", "OK");
                    }
                }
            }
            catch (Exception ex)
            {
                // Obsługa błędów
                await DisplayAlert("Błąd", $"Nie udało się uruchomić skryptu: {ex.Message}", "OK");
            }
        }


    }

}
