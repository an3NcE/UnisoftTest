using PropertyChanged;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.ServiceProcess;

namespace UnisoftTest.MVVM.ViewModels
{
    [AddINotifyPropertyChangedInterface]
    public class BackupServicePageViewModel
    {
        public ICommand InstallService => new Command(RunInstallService);
        string servicePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"BackupServiceZSI\BackupServiceZSI.exe");
        string serviceName = "BackupServiceZSI";
        public string serviceStatus { get; set; }

        public BackupServicePageViewModel()
        {
            IsServiceInstalled(serviceName);
        }

        private void RunInstallService(object obj)
        {
            Process process = new Process();
            if (IsServiceInstalled(serviceName))
            {
                ProcessStartInfo psi = new ProcessStartInfo
                {
                    FileName = "sc",
                    Arguments = $"delete {serviceName}",
                    Verb = "runas",
                    UseShellExecute = true
                };

                 process = Process.Start(psi);
            }
            else
            {
                try
                {


                    ProcessStartInfo psi = new ProcessStartInfo
                    {
                        FileName = "sc",
                        Arguments = $"create {serviceName} binPath=\"{servicePath}\" start=auto",
                        Verb = "runas", // Wymusza uruchomienie jako administrator
                        UseShellExecute = true
                    };

                     process = Process.Start(psi);
                    //await DisplayAlert("Sukces", "Usługa została zainstalowana!", "OK");


                }
                catch (Exception ex)
                {
                    //await DisplayAlert("Błąd", $"Wystąpił problem: {ex.Message}", "OK");
                }
            }
            if (process != null)
            {
                process.WaitForExit();  // Czeka na zakończenie procesu
            }
            IsServiceInstalled(serviceName);

        }



        public  bool IsServiceInstalled(string serviceN)
        {
            ProcessStartInfo psi = new ProcessStartInfo
            {
                FileName = "sc",
                Arguments = $"query {serviceN}",
                RedirectStandardOutput = true,
                UseShellExecute = false,
                CreateNoWindow = true
            };

            using (Process process = Process.Start(psi))
            {
                using (System.IO.StreamReader reader = process.StandardOutput)
                {
                    string output = reader.ReadToEnd();

                    if (output != null)
                    {
                        if (output.ToLower().Contains("stopped") || output.ToLower().Contains("zatrzymana"))
                            serviceStatus = "Status usługi: Zatrzymana";
                        else if (output.ToLower().Contains("running") || output.ToLower().Contains("działa"))
                            serviceStatus = "Status usługi: Uruchomiona";
                        else if (output.ToLower().Contains("paused"))
                            serviceStatus = "Status usługi: Wstrzymana";
                        else if (output.ToLower().Contains("nie istnieje"))
                            serviceStatus = "Status usługi: Niezainstalowana!";
                        else
                            serviceStatus = "Nieznany stan";
                    }

                    return !output.Contains("FAILED 1060"); // 1060 = brak usługi
                }
            }
        }



    }
}
