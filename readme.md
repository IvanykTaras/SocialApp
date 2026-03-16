
# Dotnet

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

drop database
```
dotnet ef database drop
```

to open sqlite explored 

> sqlite open database# SocialApp


# Angular

To install angular 
```
npm install -g @angular/cli
```

check angular version
```
ng version
```

to start project 
```
ng serve
```

to generate component

1. adjust angular.json

```
"schematics": {
        "@schematics/angular:component": {
          "path": "src"
        }
      },
```

2. check sample output

```
ng g c nav --dry-run
```

3. generate

```
ng g c layout/nav 
```

to generate service

1. adjust angular.json

```
"@schematics/angular:service": {
  "path": "src/core/services"
}
```

2. check sample output
```
ng g s account-service --dry-run
```

3. generate
```
ng g s account-service 
```

to generate guard
```
ng g g auth
```

### How to add local cert to your front

link to resource: https://github.com/FiloSottile/mkcert

1. to install mkcert 
```
choco install mkcert
```
2. add folder ssl and `mkcert localhost`
3. add this piece of code to angular.json

```
"serve": {
          "options": {
            "ssl": true,
            "sslCert": "./ssl/localhost.pem",
            "sslKey": "./ssl/localhost-key.pem"
          },
}
```