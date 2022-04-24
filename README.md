# mvc-2021-22
Repozitorij za predmet ASP.NET MVC 2021/22

Podaci za spajanje na Microsoft SQL bazu podataka
- Adresa: 127.0.0.1:1433
- Username: sa
- Password: Rootpass1

Dodavanje migracijske skripte
- dotnet ef migrations add <ime_migracije> --startup-project ..\Vjezba.Web --context ClientManagerDbContext

Izvrsavanje migracijskih skripti
- dotnet ef database update --startup-project ..\Vjezba.Web --context ClientManagerDbContext