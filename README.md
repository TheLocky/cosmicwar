# Installing
## Deploy On Azure
### From GitHub repository
* Fork comicswar repository
* Create Web App
* Go into settings of created app
* Choose "Deployment options"
* Choose Github
* Choose your forked repository
* Check your Web App root path, if it different from "D:\home\site\wwwroot\", change "DB_PATH" variable in Web.config file
### From VisualStudio
* Open project with Visual Studio
* Right click on "Project"
* Click on "Publish"
* Just follow the instructions
## On Windows
You need installed Visual Studio 2015 and IIS Express
### Hosting with IIS Express
* Compile project in VS
* Change "DB_PATH" variable in Web.config to path where database will be placed
* Run iisexpress.exe as adminitrator (default path:"C:\Program Files\IIS Express") with parameter: "/path:<full path to your project folder>". More about running IIS Express from console: https://www.iis.net/learn/extensions/using-iis-express/running-iis-express-from-the-command-line
