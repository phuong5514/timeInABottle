*Recommended Markdown Viewer: [Markdown Editor](https://marketplace.visualstudio.com/items?itemName=MadsKristensen.MarkdownEditor2)*

## Getting Started

The Core project contains code that can be [reused across multiple application projects](https://docs.microsoft.com/dotnet/standard/net-standard#net-5-and-net-standard).


## TO use api weather service:
+ create a config file: secret.config

+ add the following content:
```
<?xml version="1.0" encoding="utf-8" ?>
<configuration>
	<WeatherAPI>
		<APIkey>{your api key}</APIkey>
		<URl>{api base url, the service is designed based on tomorrow.io's api}</URl>
	</WeatherAPI>
</configuration>
```