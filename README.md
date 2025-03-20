Aplikacja umożliwia zarządzanie skryptami SQL do kopiowania baz danych oraz testowanie wydajności aplikacji poprzez automatyczne uruchamianie zdefiniowanych operacji na bazach SQLite. Użytkownik może dodawać, edytować i uruchamiać skrypty, a także monitorować ich wykonanie w czasie rzeczywistym. 

Kluczowe funkcjonalności:
Tworzenie, edycja i przechowywanie skryptów SQL w bazie SQLite.
Uruchamianie skryptów SQL(Oracle) w tle przy użyciu Process w .NET.
Przechwytywanie i wyświetlanie danych wyjściowych skryptu w czasie rzeczywistym.
Obsługa błędów i logowanie wyników.
Wskaźnik ładowania sygnalizujący trwające operacje.
Praca z danymi w czasie rzeczywistym poprzez powiązania BindingContext.

Technologie i narzędzia:
.NET MAUI - multiplatformowy framework do budowy interfejsu użytkownika.
SQLite - lokalna baza danych do przechowywania skryptów SQL.
MVVM (Model-View-ViewModel) - wzorzec architektoniczny zapewniający rozdzielenie logiki aplikacji od interfejsu.
Procesy w tle (System.Diagnostics.Process) - uruchamianie skryptów SQL poprzez CMD.
Binding i aktualizacja UI - dynamiczne przekazywanie wyników do interfejsu.
Asynchroniczne operacje (Task.Run()) - zapobieganie blokowaniu interfejsu podczas wykonywania długotrwałych operacji.

Wykorzystanie w praktyce:
Automatyzacja kopiowania baz danych między schematami.
Testowanie wydajności poprzez seryjne uruchamianie skryptów SQL.
Monitorowanie i analiza wyników operacji SQL w czasie rzeczywistym.


![unitoolbox](https://github.com/ImMan3NcE/UniToolbox/blob/master/UniToolbox.gif)
