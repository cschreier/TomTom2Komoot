# TomTom2Komoot
.NET 5 console app to export cycling and hiking workouts from TomTom-MySports to Komoot.


## Getting started
### Docker
  1. clone the repository
  3. check runtime in prod.Dockerfile
  4. check timezone settings in prod.Dockerfile
  5. set following environment variables in .env-file or in prod.docker-compose.yml
      * KOMOOT_USERNAME
      * KOMOOT_PASSWORD
      * TOMTOM_USERNAME
      * TOMTOM_PASSWORD
      * SYNC_WORKOUT_TYPES
  6. run ``` docker-compose -f prod.docker-compose.yml up```

### Console app
  1. clone the repository
  2. run ```dotnet publish -c Release -o <TARGET_DIRECTORY> -r <YOURE_RUNTIME_ID>```
  3. go to your target directory
  4. run the app with all arguments

#### Linux example
```./TomTom2Komoot ku=christian kp=bla tu=christian tp=bla w=Cycling,Hiking```

## Environment variables
### SYNC_WORKOUT_TYPES
Workout types, separated by commas, which should be synchronized
  * Cycling
  * Hiking
  * Jogging

## Console arguments
  * ku=KOMOOT_USER
  * kp=KOMOOT_PASSWORD
  * tu=TOMTOM_USER
  * tp=TOMTOM_PASSWORD
  * w=SYNC_WORKOUT_TYPES
## Logs
* /logs/logfile.log -> logfile from current day
* /logs/logfile_####.log -> daily archived logfile
