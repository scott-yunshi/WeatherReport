# weatherreport

How to build and run the API service:
1. Clone repository or download files to your working space.
2. Use commond line cd to \CurrentWeather directory.
3. Find "appsettings.json" file
- Replace "APIKey": "KeyVault-Override" with "8b7535b42fe1c551f18028f64e8688f7" or "9f933451cebf1fa39de168a29a4d9a79" (at line 13)
- Replace "WeatherReportAPIAuthKey": "KeyVault-Override" with "key1,key2,key3,key4,key5" (at line 28)
4. Run "dotnet restore"
5. Run "dotnet build"
6. Run "dotnet publish"
7. cd to directory: \CurrentWeather\CurrentWeather\bin\Debug\netcoreapp3.1\publish
8. Run "CurrentWeather.exe"
9. Now CurrentWeather API is up and running, server is listening on http://localhost:5000

How to access the weatherreport service from front end:
1. Go to \CurrentWeatherClient directory.
2. Open Index.html file with browser.
3. Now, you can put city and country, click button to access API service.
