## To use api weather service:
+ create a config file: secret.config

+ add the following content:
```
<?xml version="1.0" encoding="utf-8" ?>
<configuration>
	<WeatherAPI>
		<APIkey>{your api key}</APIkey>
		<URl>{api base url, the service is designed based on tomorrow.io's api}</URl>
		<StartTime>05:00:00</StartTime> // weather data start point
		<EndTime>22:00:00</EndTime>  // weather data end point
		<TimeZone>+07:00</TimeZone> // timezone
	</WeatherAPI>

	<Dao>
		<bgComFile>{name of the json file that will communicate with background task}</bgComFile>
		<connectionStrFilename>{database file name}</connectionStrFilename>
	</Dao>
</configuration>
```