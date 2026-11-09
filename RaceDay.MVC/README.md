# RaceDay - Part 3 MVC

MVC app that calls the Part 2 API.

## Run
1. Set Startup Projects to Multiple: RaceDay.API + RaceDay.MVC
2. Press Play
3. Register as Organiser -> Create Event
4. Register as Participant -> Enrol -> View Results

## API
Base URL: https://localhost:7018

## Docker
docker build -t raceday-mvc .
docker run -p 8080:8080 raceday-mvc