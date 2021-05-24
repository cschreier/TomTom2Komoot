# TomTom2Komoot
.NET 5 console app to export cycling and hiking workouts from TomTom-MySports to Komoot.


## Getting started
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


## Environment variables
### SYNC_WORKOUT_TYPES
Workout types, separated by commas, which should be synchronized
  * Cycling
  * Hiking
  * Jogging


## Logs
* /logs/logfile.log -> logfile from current day
* /logs/logfile_####.log -> daily archived logfile
