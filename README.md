# UserVault — Notatnik użytkowników

## Opis

Aplikacja **UserVault** to prosty „notatnik użytkowników” służący do przechowywania i edycji danych osób. Zawiera główny widok z tabelą użytkowników oraz modal do dodawania i edycji rekordów.

## Demo

Live demo: [https://uservault.tojest.dev/](https://uservault.tojest.dev/)

## Stos technologiczny

* Backend: ASP.NET
* Frontend: Quasar + Vue.js
* Sposób przechowywania danych: MSSQL

## Funkcjonalności

* Dodawanie nowych użytkowników (Imię, Nazwisko, Data urodzenia, Płeć)
* Możliwość zdefiniowania dodatkowych atrybutów per użytkownik (np. numer telefonu, stanowisko)
* Edycja wcześniej zapisanych danych (kliknięcie imienia/nazwiska lub przycisku "ikony edycji"
* Usuwanie użytkowników
* Walidacje pól:

  * Imię: wymagane, max 50 znaków, litery + spacja
  * Nazwisko: wymagane, max 150 znaków, litery + myślnik
  * Data urodzenia: wymagana, nie może być z przyszłości
  * Płeć: wymagana, wartość z podzbioru ("Male", "Female")
* Generowanie raportu (.xlsx) z wybranych użytkowników zawierającego: Imię, Nazwisko, Data urodzenia, Płeć, Tytuł (Pan/Pani na podstawie płci), Wiek (wyliczony z daty urodzenia).`

## Uruchomienie lokalne (przykładowe)

1. Backend:

```bash
cd backend
dotnet restore
dotnet run
```

2. Frontend (Quasar):

```bash
cd frontend
npm install
# tryb deweloperski
quasar dev
```

3. Baza danych:
   aplikacja oczekuje bazy danych mssql dostępnej na porcie 1433.

4. Lub przy użyciu docker-compose (jeśli chcesz uruchomić całości):

```bash
docker-compose up --build
```

## Struktura repo (krótki przegląd)

Repo zawiera separację frontend/backend oraz skrypty do uruchomienia w kontenerach i CI:

* `backend/` — kod aplikacji .NET (API / logika serwera).
* `frontend/` — aplikacja Quasar + Vue (UI, obsługa formularzy, walidacje klienta).
* `.github/workflows/` — definicje workflow (CI/CD) dla GitHub Actions.
* `docker-compose.yml` — przykładowa kompozycja do lokalnego uruchomienia (backend + db + frontend / proxy itp.).
* `init/` — skrypt sql do inicjalizacji i wprowadzenia przykładowych danych.

## CI / CD — automatyczny deploy na self-hosted runner
Repozytorium zawiera workflow GitHub Actions umożliwiające automatyczne budowanie i wdrażanie aplikacji. Wdrożony scenariusz:

1. Budowane są frontend i backend
2. Tworzone są nowe kontenery dla backendu i frontendu
3. Obrazy kontenerów umieszczane są w repozytorium Docker Hub
4. Na self-hosted runnerze uruchamiana jest aplikacja (frontend, backend, baza danych)
   

