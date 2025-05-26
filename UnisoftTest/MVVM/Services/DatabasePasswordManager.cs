using Meziantou.Framework.Win32;
using System.Security.Principal;
using System.Threading.Tasks;

#if WINDOWS
using Meziantou.Framework.Win32;
#else
using Microsoft.Maui.Storage;
#endif

namespace UnisoftTest.MVVM.Services
{
    public static class DatabasePasswordManager
    {
        private const string DbPasswordCredentialTarget = "UniToolbox.PW";

        public static bool PasswordExists()
        {
#if WINDOWS
            return CredentialManager.ReadCredential(DbPasswordCredentialTarget) != null;
#else
            var value = SecureStorage.Default.GetAsync(DbPasswordCredentialTarget).GetAwaiter().GetResult();
            return value != null;
#endif
        }

        public static string GetPassword()
        {
#if WINDOWS
            var credential = CredentialManager.ReadCredential(DbPasswordCredentialTarget);
            if (credential == null)
                throw new InvalidOperationException("Brak zapisanych poświadczeń hasła do bazy danych.");

            return credential.Password;
#else 
            return SecureStorage.Default.GetAsync(DbPasswordCredentialTarget).GetAwaiter().GetResult();
#endif
        }

        public static void SavePassword(string password)
        {
#if WINDOWS
            EnsureRunningAsAdministrator();

            CredentialManager.WriteCredential(
                DbPasswordCredentialTarget,
                "UniToolbox_pw",
                password,
                CredentialPersistence.LocalMachine);
#else
            SecureStorage.Default.SetAsync(DbPasswordCredentialTarget, password).Wait();
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
                throw new UnauthorizedAccessException("Aplikacja musi być uruchomiona jako administrator, aby zapisać poświadczenia systemowe.");
            }
        }
#else
        public static void EnsureRunningAsAdministrator() { }
#endif

        public static async Task EnsurePasswordAsync()
        {
            if (!PasswordExists())
            {
                string password = await Application.Current.MainPage.DisplayPromptAsync(
                    "Hasło do bazy", "Podaj hasło do bazy danych SQLite:", maxLength: 128, keyboard: Keyboard.Text);

                if (string.IsNullOrWhiteSpace(password))
                {
                    await Application.Current.MainPage.DisplayAlert("Błąd", "Hasło nie może być puste.", "OK");
                    return;
                }

                try
                {
                    SavePassword(password);

                    await Application.Current.MainPage.DisplayAlert("Zapisano",
                        "Hasło zostało zapisane w Credential Managerze (LocalMachine).", "OK");
                }
                catch (UnauthorizedAccessException ex)
                {
                    await Application.Current.MainPage.DisplayAlert("Błąd",
                        "Aplikacja musi być uruchomiona jako administrator.\n\n" + ex.Message, "OK");

                    System.Diagnostics.Process.GetCurrentProcess().Kill();
                }
                catch (Exception ex)
                {
                    await Application.Current.MainPage.DisplayAlert("Błąd",
                        "Wystąpił błąd zapisu hasła: " + ex.Message, "OK");
                }
            }
        }

    }
}
