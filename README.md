## Exclousive Ecommerce Wep Application


# Database configuration
appsettings.json file

```
{
  "ConnectionStrings": {
    "DefaultConnection": "Data Source=YourDatabase.db"
  },
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft": "Warning",
      "Microsoft.Hosting.Lifetime": "Information"
    }
  },
  "AllowedHosts": "*"
}
```

### Command lines

## Database migration

Make sure no local database in your project folder
```
dotnet ef migrations add InitialCreate
dotnet ef database update
```

## Build 

```
dotnet build
```

## Run without live demo

```
dotnet run
```

## Run watch for live demo whenever your code updated it will auto refresh.

```
dotnet watch run
```