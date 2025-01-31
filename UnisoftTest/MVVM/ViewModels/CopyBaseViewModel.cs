using PropertyChanged;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using UniTest.MVVM.Models;


namespace UnisoftTest.MVVM.ViewModels
{
    [AddINotifyPropertyChangedInterface]
    public class CopyBasePageViewModel
    {
        public List<CopyBaseScripts> BaseScripts { get; set; }
        public CopyBaseScripts CurrentScript { get; set; }

        public ICommand RunScript => new Command(RunCopyBaseScript);
        public string ResultEditor { get; set; }
        public bool ActIndIsRunning { get; set; }


        public CopyBasePageViewModel()
        {

            Refresh();
        }


        public void Refresh()
        {
            BaseScripts = App.BaseRepo.GetAllBaseScripts();

        }
        private async void RunCopyBaseScript(object obj)
        {
            if (obj == null)
            {
                return;
            }

            var currentCopyBaseScript = obj as CopyBaseScripts;

            string txtScript = currentCopyBaseScript.CopyBaseScriptCMD;

            string sqlFilePath = Path.Combine(Path.GetTempPath(), "script.sql");

            //ResultEditor = txtScript;
            try
            {
                // Włącz ikonę ładowania
                LoadingIcon(true);

                File.WriteAllText(sqlFilePath, currentCopyBaseScript.CopyBaseScript);

                // Wykonaj ciężkie zadanie w tle
                await Task.Run(() =>
                {
                    // Symulacja ciężkiego zadania
                    Process process = new Process();
                    process.StartInfo.FileName = "cmd.exe";
                    process.StartInfo.Arguments = $"/c {txtScript} @{sqlFilePath}";
                    process.StartInfo.RedirectStandardOutput = true;
                    process.StartInfo.RedirectStandardError = true;
                    process.StartInfo.UseShellExecute = false;
                    process.StartInfo.CreateNoWindow = true;

                    process.Start();

                    // Czytaj dane wyjściowe na bieżąco
                    while (!process.StandardOutput.EndOfStream)
                    {
                        string line = process.StandardOutput.ReadLine();
                        ResultEditor += line + Environment.NewLine; // Dodanie do wyniku
                        //PropertyChanged(nameof(ResultEditor)); // Powiadomienie o zmianie
                    }

                    process.WaitForExit();
                });
            }
            catch (Exception ex)
            {
                ResultEditor = $"Błąd: {ex.Message}";
            }
            finally
            {
                // Wyłącz ikonę ładowania
                LoadingIcon(false);
            }
            File.Delete(sqlFilePath);
        }

        public void LoadingIcon(bool isRunning)
        {
            ActIndIsRunning = isRunning;
        }

    }
}
