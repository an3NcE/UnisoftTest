using PropertyChanged;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.ServiceProcess;
using UnisoftTest.MVVM.Models;

namespace UnisoftTest.MVVM.ViewModels
{
    [AddINotifyPropertyChangedInterface]
    public class BackupServicePageViewModel
    {
        public ICommand InstallService => new Command(RunInstallService);
        public ICommand TurnOnService => new Command(RunService);
        public ICommand SaveBackupServConfiguration => new Command(SaveBackupServConf);

        public BackupServiceConfiguration BackupServiceConfiguration { get; set; }

        public TimeSpan BackupServiceScheduleTime { get; set; }

        private bool _backupServiceDaysOfWeekToggled;
        public bool BackupServiceDaysOfWeekToggled
        {
            get => _backupServiceDaysOfWeekToggled;
            set
            {
                if (_backupServiceDaysOfWeekToggled != value)
                {
                    _backupServiceDaysOfWeekToggled = value;

                    ChangeLabelOnToggle(value);
                }
            }
        }
        public string BackupServiceDaysOfWeeklabel { get; set; }

        public string ScriptViewLabel { get; set; }
        

        string servicePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"BackupServiceZSI\BackupServiceZSI.exe");
        string serviceName = "BackupServiceZSIfk";
        public string serviceStatus { get; set; }
        public string serviceButtonInstall { get; set; }
        public string serviceStartStopBtn { get; set; }
        public bool serviceStartStopBtnStatus { get; set; }

        public BackupServicePageViewModel()
        {
            IsServiceInstalled(serviceName);
            Refresh();
        }

        private async void SaveBackupServConf(object obj)
        {
            if (BackupServiceConfiguration.backupserviceconf_loginserver != null && BackupServiceConfiguration.backupserviceconf_passwordserverDecryptedPassword != null &&
                BackupServiceConfiguration.backupserviceconf_directory != null && BackupServiceConfiguration.backupserviceconf_dumpfile != null &&
                BackupServiceConfiguration.backupserviceconf_logfile != null && BackupServiceConfiguration.backupserviceconf_schemas != null &&
                BackupServiceConfiguration.backupserviceconf_mailreceiver != null && BackupServiceConfiguration.backupserviceconf_mailtitle != null &&
                BackupServiceConfiguration.backupserviceconf_instance != null)
            {
                BackupServiceConfiguration.backupserviceconf_scheduletime_hour= BackupServiceScheduleTime.Hours;
                BackupServiceConfiguration.backupserviceconf_scheduletime_minutes= BackupServiceScheduleTime.Minutes;
                if (BackupServiceDaysOfWeekToggled==false)
                {
                    BackupServiceConfiguration.backupserviceconf_daysofweek = 0;//Pn-Pt
                }
                else
                {
                    BackupServiceConfiguration.backupserviceconf_daysofweek = 1;//Pn-Nd
                }
                    await App.BaseRepo.AddOrUpdateBackupServiceConfiguration(BackupServiceConfiguration);
                await Shell.Current.DisplayAlert("Sukces", "Konfiguracja zapisana.", "OK");
                Refresh();
            }
        }

        private async void Refresh()
        {
            BackupServiceConfiguration = await App.BaseRepo.GetBackupServiceConfiguration(1);
            if (BackupServiceConfiguration == null)
            {
                BackupServiceConfiguration = new BackupServiceConfiguration();
            }
            else
            {
                BackupServiceScheduleTime= new TimeSpan(BackupServiceConfiguration.backupserviceconf_scheduletime_hour, BackupServiceConfiguration.backupserviceconf_scheduletime_minutes,0);
                if(BackupServiceConfiguration.backupserviceconf_daysofweek == 1)
                {
                    BackupServiceDaysOfWeekToggled = true;
                    ChangeLabelOnToggle(true);
                }
                else
                {
                    BackupServiceDaysOfWeekToggled = false;
                    ChangeLabelOnToggle(false);
                }
                ScriptViewLabel = $"expdp.exe {BackupServiceConfiguration.backupserviceconf_loginserver}/****@{BackupServiceConfiguration.backupserviceconf_instance} directory={BackupServiceConfiguration.backupserviceconf_directory} dumpfile={BackupServiceConfiguration.backupserviceconf_dumpfile} logfile={BackupServiceConfiguration.backupserviceconf_logfile} schemas={BackupServiceConfiguration.backupserviceconf_schemas} reuse_dumpfiles=y consistent=y";

            }

        }

        public void ChangeLabelOnToggle(bool checkValue)
        {
            if (checkValue)
            {
                BackupServiceDaysOfWeeklabel = " Dni: Pn-Nd";
            }
            else
            {
                BackupServiceDaysOfWeeklabel = " Dni: Pn-Pt";
            }
        }

        private void RunInstallService(object obj)
        {
            Process process = new Process();
            if (serviceStatus.ToLower().Contains("uruchomiona") || serviceStatus.ToLower().Contains("running"))
            {
                ProcessStartInfo psi = new ProcessStartInfo
                {
                    FileName = "sc",
                    Arguments = $"stop {serviceName}",
                    Verb = "runas",
                    UseShellExecute = true
                };

                process = Process.Start(psi);
                process.WaitForExit();
            }

            //Process process = new Process();
            if (IsServiceInstalled(serviceName))
            {
                //RunService();
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



        public bool IsServiceInstalled(string serviceN)
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
                        {
                            serviceStatus = "Status usługi: Zatrzymana";
                            serviceButtonInstall = "Odinstaluj usługę";
                            serviceStartStopBtn = "Uruchom usługę";
                            serviceStartStopBtnStatus = true;
                        }
                        else if (output.ToLower().Contains("running") || output.ToLower().Contains("działa"))
                        {
                            serviceStatus = "Status usługi: Uruchomiona";
                            serviceButtonInstall = "Odinstaluj usługę";
                            serviceStartStopBtn = "Zatrzymaj usługę";
                            serviceStartStopBtnStatus = true;
                        }
                        else if (output.ToLower().Contains("paused"))
                        {
                            serviceStatus = "Status usługi: Wstrzymana";
                            serviceButtonInstall = "Odinstaluj usługę";
                            serviceStartStopBtn = "Uruchom usługę";
                            serviceStartStopBtnStatus = true;
                        }
                        else if (output.ToLower().Contains("nie istnieje"))
                        {
                            serviceStatus = "Status usługi: Niezainstalowana!";
                            serviceButtonInstall = "Zainstaluj usługę";
                            serviceStartStopBtnStatus = false;
                        }
                        else
                        {
                            serviceStatus = "Nieznany stan";
                            serviceButtonInstall = "Zainstaluj usługę";
                            serviceStartStopBtnStatus = false;
                        }

                    }

                    return !output.Contains("FAILED 1060"); // 1060 = brak usługi
                }
            }
        }


        private void RunService()
        {
            Process process = new Process();
            IsServiceInstalled(serviceName);
            if (serviceStatus.ToLower().Contains("zatrzymana") || serviceStatus.ToLower().Contains("wstrzymana") || serviceStatus.ToLower().Contains("stopped"))
            {
                ProcessStartInfo psi = new ProcessStartInfo
                {
                    FileName = "sc",
                    Arguments = $"start {serviceName}",
                    Verb = "runas",
                    UseShellExecute = true
                };

                process=Process.Start(psi);
                process.WaitForExit();
            }
            else if (serviceStatus.ToLower().Contains("uruchomiona") || serviceStatus.ToLower().Contains("running"))
            {
                ProcessStartInfo psi = new ProcessStartInfo
                {
                    FileName = "sc",
                    Arguments = $"stop {serviceName}",
                    Verb = "runas",
                    UseShellExecute = true
                };

                process=Process.Start(psi);
                process.WaitForExit();
            }
            
            IsServiceInstalled(serviceName);
        }

    }
}
