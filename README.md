# SitesAdmin

Administration Web Api for website static generation and asset management

## Useful commands

Add migration (run from src/SitesAdmin folder)
```
dotnet ef migrations add <MigrationName> -o Data/Migrations

dotnet ef database update --connection "Server=localhost;Database=sitesadmin;Uid=sitesadmin;Pwd=testing123"

```