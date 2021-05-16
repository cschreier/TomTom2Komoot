# TomTom2Komoot
.NET 5 console app to export cycling and hiking workouts from TomTom-MySports to Komoot.

## Installation
  1. clone the repository
  2. publish the project <br>
    ```dotnet publish -c Release -r [YOURE_RUNTIME_ID] --self-contained```
  3. create the appsettings.json
```json
{
  "Komoot": {
    "Username": "",
    "Password": ""
  },
  "TomTom": {
    "Username": "",
    "Password": ""
  },
  "LastSyncAt": ""
}
```
  4. start the application
  
## Logs
* /logs/logfile.log -> logfile from current day
* /logs/logfile_####.log -> daily archived logfile
