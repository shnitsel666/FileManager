## LanDocs.Web.FileAgent app build
#### Build Setup
#### 1) Install extension for Visual Studio - Wix Toolset Visual Studio 20** Extension
#### 2) Install extension for Visual Studio - Wax
#### 3) Open file \FileManager\FileManager\FileManager.sln
#### 4) Publish main application FileManager with params: windows x64, self-contained, produce single file
#### 5) Open in root folder LanDocs.Web.LanImageConnector folder. Open LanDocs.Web.LanImageConnector.sln and build this app.
#### 6) Copy builded files from LanDocs.Web.LanImageConnector to publish folder in FileManager publish folder
#### 7) Check files paths in FileManager.installer folder, in file Components.wxs (it's important!)
#### 8) In FileManager.sln opened solution select FileManager.installer project and build it.
#### 9) Done! You're gorgeous! 
#### 10) For additional info, you can see youtube guide (increase speed to 1.25 for better understanding) https://www.youtube.com/watch?v=6Yf-eDsRrnM

## FileManager API
### http://localhost:9000/index.html - Swagger
