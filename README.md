# CosmicWar
[![AppVeyor Build Status](https://ci.appveyor.com/api/projects/status/github/TheLocky/cosmicwar?svg=true)](https://ci.appveyor.com/project/TheLocky/cosmicwar)

# Installing

## Deploy On Azure

### From GitHub repository
* Fork comicswar repository
* Create Web App (New -> Web + mobile -> Web App)
* Go into settings of created app
* Choose "Deployment options"
* Choose Github
* Choose your forked repository
* Set CWSERVER_DBPATH environment variable:
    * Choose "Application settings"
    * Scroll to "App settings" and set environment variable

### From VisualStudio
* Open project with Visual Studio
* Right click on "Project"
* Click on "Publish"
* Just follow the instructions

## On Windows
You need installed Visual Studio 2015 and IIS Express

### Hosting with IIS Express
* Compile project in VS
* Set CWSERVER_DBPATH environment variable
* Run iisexpress.exe as adminitrator (default path:"C:\Program Files\IIS Express") with parameter: "/path:<full path to your project folder>". More about running IIS Express from console: https://www.iis.net/learn/extensions/using-iis-express/running-iis-express-from-the-command-line
