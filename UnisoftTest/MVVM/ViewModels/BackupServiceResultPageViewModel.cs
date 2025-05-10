using MailKit.Search;
using MailKit.Security;
using MailKit;
using MimeKit;
using PropertyChanged;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using unisofttest.MVVM.Models;
using UnisoftTest;
using MailKit.Net.Imap;
using System.Collections.ObjectModel;

namespace unisofttest.MVVM.ViewModels
{
    [AddINotifyPropertyChangedInterface]
    public class BackupServiceResultPageViewModel
    {
        public List<BackupServiceResult> Clients { get; set; }
        public BackupServiceResult CurrentClient { get; set; }

        public ICommand AddOrUpdateCommand => new Command(AddOrUpdateComm);
        public ICommand OpenCopyLogCommand => new Command<BackupServiceResult>(async (result) => await OpenLogFileAsync(result));



        public ICommand DownloadMessages => new Command(async () => await DownloadMessagesAsync());
        public ObservableCollection<string> MailSubjects { get; } = new();

        public BackupServiceResultPageViewModel()
        {
            //CurrentScript = new CopyBaseScripts();
            Refresh();
        }

        private async void AddOrUpdateComm()
        {


            if (CurrentClient == null)
            {
                CurrentClient = new BackupServiceResult();
            }

            //if (!string.IsNullOrEmpty(CurrentClient.SourceBaseName) & !string.IsNullOrEmpty(CurrentScript.DestinationBaseName) & !string.IsNullOrEmpty(CurrentScript.CopyBaseScript))
            //{
            await App.BaseRepo.AddOrUpdateBackupServiceResult(CurrentClient);

            Debug.WriteLine(App.BaseRepo.StatusMessage);
            await Refresh();


            //}
            //else
            //{
            //    MessagingCenter.Send(this, "Alert", "Wypełnij wszystkie pola.");
            //}

        }

        private async Task Refresh()
        {
            Clients = await App.BaseRepo.GetAllBackupServiceResults();
            CurrentClient = new BackupServiceResult();
        }
        private async Task OpenLogFileAsync(BackupServiceResult result)
        {
            if (result?.backupserviceresult_resultlog == null || result.backupserviceresult_resultlog.Length == 0)
                return;

            try
            {
                var fileName = $"{result.backupserviceresult_clientsymbol}_{DateTime.Now:yyyyMMdd_HHmmss}.txt";
                var filePath = Path.Combine(FileSystem.CacheDirectory, fileName);
                File.WriteAllBytes(filePath, result.backupserviceresult_resultlog);


                var psi = new System.Diagnostics.ProcessStartInfo
                {
                    FileName = "notepad.exe",
                    Arguments = filePath,
                    UseShellExecute = true
                };
                System.Diagnostics.Process.Start(psi);
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Błąd", $"Nie udało się otworzyć logu: {ex.Message}", "OK");
            }
        }

        private async Task DownloadMessagesAsync()
        {
            await Refresh();
            var config = await App.BaseRepo.GetMailConfiguration(1);
            if (config == null)
                return;

            var results = await GetSentLogsAsync(config);
            results = results.OrderBy(r => r.SentDate).ToList();

            MailSubjects.Clear();
            foreach (var (subject, _, _) in results)
                MailSubjects.Add(subject);
            foreach (var client in Clients)
            {
                if (string.IsNullOrWhiteSpace(client.backupserviceresult_clientsymbol))
                    continue;

                // Znajdź pierwszy mail, który zawiera symbol klienta w temacie
                var match = results.FirstOrDefault(r =>
                    r.Subject.Contains(client.backupserviceresult_clientsymbol, StringComparison.OrdinalIgnoreCase));

                //if (match == default)
                //    continue;

                if (string.IsNullOrEmpty(match.Subject))
                {
                    client.backupserviceresult_result = "BRAK INFORMACJI!";
                    client.backupserviceresult_resultimage = "confirm_wrong.png";
                    await App.BaseRepo.AddOrUpdateBackupServiceResult(client);
                    continue;
                }
                    

                if (match.Subject.Contains("NIEPRAWIDŁOWA"))
                {
                    client.backupserviceresult_result = "Błąd kopii";
                    client.backupserviceresult_resultimage = "confirm_wrong.png";
                }
                else if (match.Subject.Contains("PRAWIDŁOWA"))
                {
                    client.backupserviceresult_result = "Poprawna";
                    client.backupserviceresult_resultimage = "confirm.png";
                }
                // Przypisz log i datę
                client.backupserviceresult_resultlog = match.LogAttachment;
                client.backupserviceresult_resultlogDate = match.SentDate;

                // Zaktualizuj w bazie
                await App.BaseRepo.AddOrUpdateBackupServiceResult(client);
            }
            await Refresh();

        }

        private List<DateTime> GetExpectedMailDates()
        {
            var today = DateTime.Today;

            if (today.DayOfWeek == DayOfWeek.Monday)
            {
                return new List<DateTime>
                {
                    today,                         // poniedziałek
                    today.AddDays(-1),             // niedziela
                    today.AddDays(-2),             // sobota
                    today.AddDays(-3)              // piątek
                };
            }

            // pozostałe dni: dziś + wczoraj
            return new List<DateTime>
                {
                    today,
                    today.AddDays(-1)
                };
        }

        private async Task<List<(string Subject, byte[] LogAttachment, DateTime SentDate)>> GetSentLogsAsync(MailConfiguration config)
        {
            var results = new List<(string Subject, byte[] LogAttachment, DateTime SentDate)>();
            //var imapserver = config.mailconf_smtpserver.Replace("smtp","imap");
            using var client = new ImapClient();
            await client.ConnectAsync(config.mailconf_smtpserver.Replace("smtp", "imap"), 993, SecureSocketOptions.SslOnConnect);
            await client.AuthenticateAsync(config.mailconf_smtpclientaddresss, config.DecryptedPassword);

            var sent = client.GetFolder(SpecialFolder.Sent) ?? client.GetFolder("Sent");
            await sent.OpenAsync(MailKit.FolderAccess.ReadOnly);

            // 🔹 Pobierz daty, które nas interesują
            var dates = GetExpectedMailDates();

            // 🔹 Zbuduj zapytanie IMAP z OR (DeliveredOn dla każdej daty)
            SearchQuery query = SearchQuery.DeliveredOn(dates[0]);
            for (int i = 1; i < dates.Count; i++)
            {
                query = query.Or(SearchQuery.DeliveredOn(dates[i]));
            }

            var uids = await sent.SearchAsync(query);

            foreach (var uid in uids)
            {
                var msg = await sent.GetMessageAsync(uid);

                var attachment = msg.Attachments
                    .OfType<MimePart>()
                    .FirstOrDefault(a => a.FileName.EndsWith(".txt", StringComparison.OrdinalIgnoreCase));

                if (attachment != null)
                {
                    using var ms = new MemoryStream();
                    await attachment.Content.DecodeToAsync(ms);
                    var subject = msg.Subject;
                    var logBytes = ms.ToArray();
                    var sentDate = msg.Date.LocalDateTime; // dokładna data + godzina

                    results.Add((subject, logBytes, sentDate));
                }
            }

            await client.DisconnectAsync(true);
            return results;
        }

    }
}
