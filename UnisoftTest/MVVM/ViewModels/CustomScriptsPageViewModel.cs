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
        public bool modifVisible { get; set; }
        public string modifVisiblePNG { get; set; }
        public AppSettings AppSettingsAdministrator { get; set; }
        public AppSettings AppSettingsModificatorVisible { get; set; }
        public List<CustomScripts> CustomScripts { get; set; }
        public CustomScripts CurrentScript { get; set; }

        public ICommand AddOrUpdateCommand => new Command(AddOrUpdateComm);
        public ICommand DeleteCommand => new Command(DeleteComm);

        public ICommand RunScript => new Command(RunCustomScript);
        public ICommand ModificatorVisible => new Command(ModificatorVisibleChange);



        public string ResultLabel { get; set; }
        public CustomScriptsPageViewModel()
        {
            Refresh();
        }
        public async void Refresh()
        {
            //ResultLabel = "test";
            CustomScripts = await App.BaseRepo.GetAllCustomScripts();
            CurrentScript = new CustomScripts();

            AppSettingsAdministrator = await App.BaseRepo.GetSettings(1);
            AppSettingsModificatorVisible = await App.BaseRepo.GetSettings(3);

            if (AppSettingsAdministrator.SettingsValue == "0" || AppSettingsAdministrator == null)
            {
                isAdministrator = false;
                modifVisible = false;

                

                if (AppSettingsModificatorVisible == null || AppSettingsModificatorVisible.SettingsValue == "0")
                {
                    modifVisible = false;
                }
                else
                {
                    modifVisible = true;
                }
            }
            else
            {
                isAdministrator = true;
                modifVisible = true;

                if (AppSettingsModificatorVisible == null || AppSettingsModificatorVisible.SettingsValue == "0")
                {
                    modifVisiblePNG = "eyeon.png";
                }
                else
                {
                    modifVisiblePNG = "eyeoff.png";
                }
            }
            
            //AppSettingsExePath = new AppSettings();

        }

        private async void ModificatorVisibleChange(object obj)
        {
            if (AppSettingsModificatorVisible == null || AppSettingsModificatorVisible.SettingsValue == "0")
            {
                await App.BaseRepo.AddOrUpdateModificatorCUstomScriptStatus(false);
            }
            else
            {
                await App.BaseRepo.AddOrUpdateModificatorCUstomScriptStatus(true);
            }
            Refresh();
        }

        private async void AddOrUpdateComm()
        {


            if (CurrentScript == null)
            {
                CurrentScript = new CustomScripts();
            }

            if (!string.IsNullOrEmpty(CurrentScript.CustomScriptName) && !string.IsNullOrEmpty(CurrentScript.CustomScriptSQL) && !string.IsNullOrEmpty(CurrentScript.CustomScriptCMD))
            {
                await App.BaseRepo.AddOrUpdateCustomScript(CurrentScript);

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
            ResultLabel = "";
            if (CurrentScript == null)
            {
                return;
            }
            if (CurrentScript.CustomScriptCMD == null || CurrentScript.CustomScriptSQL == null)
            {
                ResultLabel = "Niepoprawny skrypt!";
                return;
            }

            if (CurrentScript.CustomScriptSQL.ToLower().Contains("drop") || CurrentScript.CustomScriptSQL.ToLower().Contains("delete"))
            {
                MessagingCenter.Send(this, "Alert", "Nic nie usuwaj z bazy!");
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
                            ResultLabel += e.Data + Environment.NewLine;
                    };

                    process.ErrorDataReceived += (sender, e) => {
                        if (!string.IsNullOrEmpty(e.Data))
                            ResultLabel += e.Data + Environment.NewLine;
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
                //    process.StartInfo.Arguments = $"/c {txtScript} @{sqlFilePath}";
                //    //process.StartInfo.Arguments = $"/c {txtScript}";
                //    process.StartInfo.RedirectStandardOutput = true;
                //    process.StartInfo.RedirectStandardError = true;
                //    process.StartInfo.UseShellExecute = false;
                //    process.StartInfo.CreateNoWindow = true;

                //    process.Start();

                //    // Czytaj dane wyjściowe na bieżąco
                //    while (!process.StandardOutput.EndOfStream)
                //    {
                //        string line = process.StandardOutput.ReadLine();
                //        ResultLabel += line + Environment.NewLine; // Dodanie do wyniku
                //        //PropertyChanged(nameof(ResultEditor)); // Powiadomienie o zmianie
                //    }

                //    process.WaitForExit();


                //});
            }
            catch (Exception ex)
            {
                ResultLabel = $"Błąd: {ex.Message}";
                return;
            }
            finally
            {
                // Wyłącz ikonę ładowania
                //LoadingIcon(false);
            }
            File.Delete(sqlFilePath);

        }



        //private async void KillTaskSqlPlus()
        //{
        //    await Task.Run(() =>
        //    {
        //        try
        //        {
        //            // Uzyskanie nazwy bieżącego użytkownika
        //            string username = Environment.UserName;

        //            // Uruchamiamy tasklist z parametrem -v
        //            string tasklistArgs = "/v /FI \"IMAGENAME eq sqlplus.exe\"";

        //            Process tasklistProcess = new Process();
        //            tasklistProcess.StartInfo.FileName = "tasklist";
        //            tasklistProcess.StartInfo.Arguments = tasklistArgs;
        //            tasklistProcess.StartInfo.RedirectStandardOutput = true;
        //            tasklistProcess.StartInfo.UseShellExecute = false;
        //            tasklistProcess.StartInfo.CreateNoWindow = true;

        //            tasklistProcess.Start();
        //            string output = tasklistProcess.StandardOutput.ReadToEnd();
        //            tasklistProcess.WaitForExit();

        //            // Analizowanie wyników tasklist
        //            string pid = null;
        //            foreach (var line in output.Split(Environment.NewLine))
        //            {
        //                // Szukamy linii zawierającej proces sqlplus
        //                if (line.Contains("sqlplus.exe") && line.Contains(username))
        //                {
        //                    // Proces sqlplus znaleziony - pobieramy PID (zwykle jest on w drugiej kolumnie)
        //                    var parts = line.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
        //                    pid = parts[1]; // PID procesu sqlplus
        //                    break;
        //                }
        //            }

        //            // Jeśli PID został znaleziony, użyj taskkill do zakończenia procesu
        //            if (pid != null)
        //            {
        //                Process taskkillProcess = new Process();
        //                taskkillProcess.StartInfo.FileName = "taskkill";
        //                taskkillProcess.StartInfo.Arguments = $"/PID {pid} /F";
        //                taskkillProcess.StartInfo.UseShellExecute = false;
        //                taskkillProcess.StartInfo.CreateNoWindow = true;

        //                taskkillProcess.Start();
        //                taskkillProcess.WaitForExit();
        //            }
        //            else
        //            {
        //                Console.WriteLine("Nie znaleziono procesu sqlplus uruchomionego przez bieżącego użytkownika.");
        //            }
        //        }
        //        catch (Exception ex)
        //        {
        //            Console.WriteLine($"Wystąpił błąd: {ex.Message}");
        //        }
        //    });

        //}

    }
}
