# File Uploader
This is an API to upload, save files on an IIS server and return their respective urls
to access each file.

Before running this project, we need to setup our storage location for our files and 
create a virtual directory to access this files.

#### File Storage Location
Create your folder to store the uploaded files, get the absolute path and use that to 
set your `RootFilePath` under the `FilesConfig` in your appsetting
```json
{   
    "RootFilePath": "D:\\Research\\wisecompany"
}
```

#### IIS and Virtual Directory (Windows)
Let's setup our virtual directory on IIS to access our uploaded files.
1. Open `Internet Information Services (IIS) Manager`
2. Right click on `Sites` in the left panel and click `Add Website`
3. Provide the name of your site, eg, `Wise Company` and proceed to choose the `Physical Path`. 
The physical path should be the same path as what we have in our appsettings `"RootFilePath": "D:\\Research\\wisecompany"`.
This physical path is where our uploaded files will be saved and where our generated urls will be directed to access the files
4. We proceed to create our `Host Name`, eg, `wisecompany.localhost` and click OK. This will be our base url for the links
to access the uploaded files. We then add our base path to our appsettings under `BaseUrl`.
```json
{
  "BaseUrl": "http://wisecompany.localhost"
}
```
