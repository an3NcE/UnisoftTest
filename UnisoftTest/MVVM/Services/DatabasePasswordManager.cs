using Meziantou.Framework.Win32;
using System.Security.Principal;
using System.Threading.Tasks;

namespace UnisoftTest.MVVM.Services
{
    public static class DatabasePasswordManager
    {
        private const string DbPasswordCredentialTarget = "UniToolbox.PW";

        public static bool PasswordExists()
        {
            return CredentialManager.ReadCredential(DbPasswordCredentialTarget) != null;
        }

        public static string GetPassword()
        {
            var credential = CredentialManager.ReadCredential(DbPasswordCredentialTarget);
            if (credential == null)
                throw new InvalidOperationException("Brak zapisanych poświadczeń hasła do bazy danych.");

            return credential.Password;
        }

        public static void SavePassword(string password)
        {
            EnsureRunningAsAdministrator();

            CredentialManager.WriteCredential(
                DbPasswordCredentialTarget,
                "UniToolbox_pw",
                password,
                CredentialPersistence.LocalMachine);
        }

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
