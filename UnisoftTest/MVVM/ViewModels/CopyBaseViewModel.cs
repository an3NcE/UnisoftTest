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

        string path = AppDomain.CurrentDomain.BaseDirectory + "\\Logs";
        public ICommand RunScript => new Command(RunCopyBaseScript);
        public string ResultEditor { get; set; }
        public bool ActIndIsRunning { get; set; }
        public bool modifVisibleRunBtn { get; set; }

        public CopyBasePageViewModel()
        {

            Refresh();
        }


        public async void Refresh()
        {
            BaseScripts = await App.BaseRepo.GetAllBaseScripts();
            if (ActIndIsRunning==false )
            {
                modifVisibleRunBtn = true;
            }
            else
            {
                modifVisibleRunBtn = false;
            }
                

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
                MessagingCenter.Send(this, "Alert", "Skrypt jest niepoprawny.");
                return;
            }

            string txtScript = currentCopyBaseScript.CopyBaseScriptCMD;

            string sqlFilePath = Path.Combine(Path.GetTempPath(), "script.sql");

            //ResultEditor = txtScript;
            try
            {
                // Włącz ikonę ładowania
                LoadingIcon(true);
                modifVisibleRunBtn = false;
                ResultEditor = "";
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
                modifVisibleRunBtn = true;
            }
            File.Delete(sqlFilePath);
            WriteToFile(ResultEditor);
            MessagingCenter.Send(this, "Alert", "Skrypt wykonany.");
        }

        public void LoadingIcon(bool isRunning)
        {
            ActIndIsRunning = isRunning;
        }

        public void WriteToFile(string cbLogs)
        {


            if (!Directory.Exists(path))

                Directory.CreateDirectory(path);

            string filepath = Path.Combine(path, "CopyBaseLog_" + DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss") + ".txt");

            if (!File.Exists(filepath))
            {

                // Create a file to write to.
                using (StreamWriter sw = File.CreateText(filepath))

                    sw.WriteLine(cbLogs);

            }
            else
            {

                using (StreamWriter sw = File.AppendText(filepath))
                {
                    sw.WriteLine(cbLogs);
                }
            }
        }

    }
}
