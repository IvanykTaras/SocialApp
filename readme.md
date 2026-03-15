

To show info about current version of dotnet
```
dotnet --info
```

Available project to create
```
dotnet list
```

Create new solution file
```
dotnet new sln
```

Craete api project with controllers
```
dotnet new webapi -controllers -n API
```

Add project to solution file
```
dotnet sln add API
```

Show sln list
```
dotnet sln list
```

restore packages 
```
dotnet resotre
```

to watch dotnet 
```
dotnet watch
```

create in dotnet ignore file 
```
dotnet new gitignore
```


### EF tool

to see dotnet tools
```
dotnet tool list -g
```

add migraiton 
```
dotnet ef migrations add InitialCreate -o Data/Migrations
```

update database
```
dotnet ef database update
```

to open sqlite explored 

> sqlite open database# SocialApp
