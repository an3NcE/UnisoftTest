using PropertyChanged;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using unisofttest.MVVM.Models;
using System.Diagnostics;

namespace UnisoftTest.MVVM.ViewModels
{
    [AddINotifyPropertyChangedInterface]
    public class MailConfPageViewModel
    {
        public MailConfiguration MailConfiguration { get; set; }

        public ICommand SaveMailConfiguration => new Command(SaveMailConf);
        public ICommand SendTestMailMessage => new Command(async () => await SendTestMailMsgAsync());



        public string testMailAddress { get; set; }
        public string imgResultSource { get; set; }


        public MailConfPageViewModel()
        {
            Refresh();
        }

        private async void SaveMailConf(object obj)
        {
            if (MailConfiguration.mailconf_smtpclientaddresss != null && MailConfiguration.mailconf_smtpserver != null && MailConfiguration.mailconf_smtpclientpassword != null && MailConfiguration.mailconf_smtpport != null)
            {
                await App.BaseRepo.AddOrUpdateMailConfiguration(MailConfiguration);
                await Shell.Current.DisplayAlert("Sukces", "Konfiguracja zapisana.", "OK");
            }

        }

        //private void SendTestMailMsg(object obj)
        //{
        //    if (testMailAddress!=null && testMailAddress !="")
        //    {

        //    }
        //}

        private async Task SendTestMailMsgAsync()
        {
            if (testMailAddress != null && testMailAddress != "")
            {


                var smtpClient = new SmtpClient(MailConfiguration.mailconf_smtpserver)
                {
                    Port = MailConfiguration.mailconf_smtpport,
                    Credentials = new NetworkCredential(MailConfiguration.mailconf_smtpclientaddresss, MailConfiguration.mailconf_smtpclientpassword),
                    EnableSsl = true,
                };

                var mailMessage = new MailMessage
                {
                    From = new MailAddress(MailConfiguration.mailconf_smtpclientaddresss),
                    Subject = "Testowa Wiadomość z UniToolbox",

                    Body = "Wiadomość testowa"
                    //IsBodyHtml = true, // Jeśli chcesz wysłać HTML
                };
                mailMessage.To.Add(testMailAddress);//odbiorca

                try
                {
                    await smtpClient.SendMailAsync(mailMessage);
                    Debug.WriteLine("Mail wysłany pomyślnie.");
                    imgResultSource = "confirm.png";
                    await Shell.Current.DisplayAlert("Sukces", "Wiadomość została wysłana pomyślnie.", "OK");

                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"Błąd przy wysyłce maila: {ex.Message}");
                    imgResultSource = "confirm_wrong.png";
                    await Shell.Current.DisplayAlert("Błąd", $"Nie udało się wysłać wiadomości.\n{ex.Message}", "OK");
                }
            }
            else
            {
                await Shell.Current.DisplayAlert("Uwaga", "Nie podano adresu e-mail.", "OK");
            }
        }

        private async void Refresh()
        {
            MailConfiguration = await App.BaseRepo.GetMailConfiguration(1);
            if (MailConfiguration == null)
            {
                MailConfiguration = new MailConfiguration();
            }
            imgResultSource = "confirm_question.png";

        }
    }
}
