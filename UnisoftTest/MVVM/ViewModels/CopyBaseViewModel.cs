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


        public async void Refresh()
        {
            BaseScripts = await App.BaseRepo.GetAllBaseScripts();

        }
        private async void RunCopyBaseScript(object obj)
        {
            if (obj == null)
            {
                return;
            }

            var currentCopyBaseScript = obj as CopyBaseScripts;

            if (currentCopyBaseScript.CopyBaseScript == null || currentCopyBaseScript.CopyBaseScriptCMD == null)
            {
                ResultEditor = "Niepoprawny skrypt!";
                return;
            }

            string txtScript = currentCopyBaseScript.CopyBaseScriptCMD;

            string sqlFilePath = Path.Combine(Path.GetTempPath(), "script.sql");

            //ResultEditor = txtScript;
            try
            {
                // Włącz ikonę ładowania
                LoadingIcon(true);

                File.WriteAllText(sqlFilePath, currentCopyBaseScript.CopyBaseScript);

                await Task.Run(() =>
                {
                    Process process = new Process();
                    process.StartInfo.FileName = "cmd.exe";
                    process.StartInfo.Arguments = $"/c {txtScript} @{sqlFilePath}";
                    process.StartInfo.RedirectStandardOutput = true;
                    process.StartInfo.RedirectStandardError = true;
                    process.StartInfo.UseShellExecute = false; // WAŻNE: NIE MOŻNA MIEĆ TEGO NA TRUE, GDY JEST REDIRECT
                    process.StartInfo.CreateNoWindow = true;

                    process.OutputDataReceived += (sender, e) => {
                        if (!string.IsNullOrEmpty(e.Data))
                            ResultEditor += e.Data + Environment.NewLine;
                    };

                    process.ErrorDataReceived += (sender, e) => {
                        if (!string.IsNullOrEmpty(e.Data))
                            ResultEditor += e.Data + Environment.NewLine;
                    };

                    process.Start();
                    process.BeginOutputReadLine();
                    process.BeginErrorReadLine();
                    process.WaitForExit();
                });

                //// Wykonaj ciężkie zadanie w tle
                //await Task.Run(() =>
                //{
                //    // Symulacja ciężkiego zadania
                //    Process process = new Process();
                //    process.StartInfo.FileName = "cmd.exe";
                //    //process.StartInfo.Arguments = $"/c {txtScript} @{sqlFilePath}";
                //    process.StartInfo.Arguments = $"/k {txtScript} @{sqlFilePath}";
                //    //process.StartInfo.Arguments = $"/c {txtScript}  pause";
                //    //process.StartInfo.Arguments = $"/k {txtScript}";
                //    //process.StartInfo.RedirectStandardOutput = true;
                //    //process.StartInfo.RedirectStandardError = true;
                //    process.StartInfo.UseShellExecute = true;
                //    process.StartInfo.CreateNoWindow = false;

                //    process.Start();

                //    //// Czytaj dane wyjściowe na bieżąco
                //    //while (!process.StandardOutput.EndOfStream)
                //    //{
                //    //    string line = process.StandardOutput.ReadLine();
                //    //    ResultEditor += line + Environment.NewLine; // Dodanie do wyniku
                //    //    //PropertyChanged(nameof(ResultEditor)); // Powiadomienie o zmianie
                //    //}


                //    process.WaitForExit();
                //});

                //await Task.Run(async () =>
                //{
                //    Process process = new Process();
                //    process.StartInfo.FileName = "cmd.exe";
                //    //process.StartInfo.Arguments = $"/c {txtScript} @{sqlFilePath}";
                //    process.StartInfo.Arguments = $"/c {txtScript} ";
                //    process.StartInfo.RedirectStandardOutput = true;
                //    process.StartInfo.RedirectStandardError = true;
                //    process.StartInfo.UseShellExecute = false;
                //    process.StartInfo.CreateNoWindow = true;

                //    process.OutputDataReceived += (sender, e) =>
                //    {
                //        if (!string.IsNullOrEmpty(e.Data))
                //        {
                //            MainThread.BeginInvokeOnMainThread(() =>
                //            {
                //                ResultEditor += e.Data + Environment.NewLine;
                //            });
                //        }
                //    };

                //    process.ErrorDataReceived += (sender, e) =>
                //    {
                //        if (!string.IsNullOrEmpty(e.Data))
                //        {
                //            MainThread.BeginInvokeOnMainThread(() =>
                //            {
                //                ResultEditor +=  e.Data + Environment.NewLine;
                //            });
                //        }
                //    };

                //    process.Start();
                //    process.BeginOutputReadLine();
                //    process.BeginErrorReadLine();

                //    await process.WaitForExitAsync();
                //});

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
