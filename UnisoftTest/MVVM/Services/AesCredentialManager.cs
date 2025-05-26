
using Meziantou.Framework.Win32;
using System.Security.Principal;

using System.Text;
using System.Threading.Tasks;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Storage;

namespace UnisoftTest.Services
{
    public static class AesCredentialManager
    {
        private const string AesKeyTarget = "UniToolbox.AES_KEY";
        private const string AesIvTarget = "UniToolbox.AES_IV";

        public static bool CredentialsExist()
        {
#if WINDOWS
            return CredentialManager.ReadCredential(AesKeyTarget) != null &&
                   CredentialManager.ReadCredential(AesIvTarget) != null;
#else
            var key = SecureStorage.Default.GetAsync(AesKeyTarget).GetAwaiter().GetResult();
            var iv = SecureStorage.Default.GetAsync(AesIvTarget).GetAwaiter().GetResult();
            return key != null && iv != null;
#endif
        }

        public static (string Key, string IV) GetCredentials()
        {
#if WINDOWS
            var keyCred = CredentialManager.ReadCredential(AesKeyTarget);
            var ivCred = CredentialManager.ReadCredential(AesIvTarget);

            if (keyCred == null || ivCred == null)
                throw new Exception("Brak zapisanych poświadczeń AES.");

            return (keyCred.Password, ivCred.Password);
#else
            var key = SecureStorage.Default.GetAsync(AesKeyTarget).GetAwaiter().GetResult();
            var iv = SecureStorage.Default.GetAsync(AesIvTarget).GetAwaiter().GetResult();

            if (string.IsNullOrEmpty(key) || string.IsNullOrEmpty(iv))
                throw new Exception("Brak zapisanych poświadczeń AES.");

            return (key, iv);
#endif
        }

        public static void SaveCredentials(string key, string iv)
        {
#if WINDOWS
            EnsureRunningAsAdministrator(); // ⛔ wymóg administratora

            CredentialManager.WriteCredential(
                AesKeyTarget,
                "aes_UniToolbox",
                key,
                CredentialPersistence.LocalMachine);

            CredentialManager.WriteCredential(
                AesIvTarget,
                "aes_UniToolbox",
                iv,
                CredentialPersistence.LocalMachine);
#else
            SecureStorage.Default.SetAsync(AesKeyTarget, key).Wait();
            SecureStorage.Default.SetAsync(AesIvTarget, iv).Wait();
#endif
        }
#if WINDOWS
        public static bool IsRunningAsAdministrator()
        {
            var identity = WindowsIdentity.GetCurrent();
            var principal = new WindowsPrincipal(identity);
            return principal.IsInRole(WindowsBuiltInRole.Administrator);
        }


        public static void EnsureRunningAsAdministrator()
        {
            if (!IsRunningAsAdministrator())
            {
                throw new UnauthorizedAccessException(
                    "Aplikacja musi być uruchomiona jako administrator, aby zapisać poświadczenia systemowe (LocalComputer).");
            }
        }
#else
        public static void EnsureRunningAsAdministrator() { }
#endif

        //#if WINDOWS
        public static async Task EnsureAsync()
        {
            if (!CredentialsExist())
            {
                await Application.Current.MainPage.DisplayAlert("Uwaga",
                    "Poświadczenia AES nie istnieją. Musisz je skonfigurować.", "OK");

                string key = await Application.Current.MainPage.DisplayPromptAsync(
                    "Klucz AES", "Podaj 16-znakowy klucz AES:", maxLength: 16, keyboard: Keyboard.Text);

                string iv = await Application.Current.MainPage.DisplayPromptAsync(
                    "IV AES", "Podaj 16-znakowy wektor inicjalizacji (IV):", maxLength: 16, keyboard: Keyboard.Text);

                if (string.IsNullOrWhiteSpace(key) || key.Length != 16 ||
                    string.IsNullOrWhiteSpace(iv) || iv.Length != 16)
                {
                    await Application.Current.MainPage.DisplayAlert("Błąd",
                        "Klucz i IV muszą mieć dokładnie 16 znaków.", "OK");
                    return;
                }

                try
                {
                    SaveCredentials(key, iv);

                    await Application.Current.MainPage.DisplayAlert("Sukces",
                        "Poświadczenia zapisano systemowo (LocalComputer).", "OK");
                    await Application.Current.MainPage.DisplayAlert("Sukces",
                        "Uruchom ponownie aplikację.", "OK");
                    System.Diagnostics.Process.GetCurrentProcess().Kill();
                }
                catch (UnauthorizedAccessException ex)
                {
                    await Application.Current.MainPage.DisplayAlert("Błąd",
                        "Aplikacja musi być uruchomiona jako administrator, aby zapisać poświadczenia systemowe.\n\n" + ex.Message,
                        "OK");
                    System.Diagnostics.Process.GetCurrentProcess().Kill();
                }
                catch (Exception ex)
                {
                    await Application.Current.MainPage.DisplayAlert("Błąd",
                        "Wystąpił błąd zapisu poświadczeń: " + ex.Message, "OK");
                }
            }
        }
//#endif
    }
}
