Dynamic Configuration (α)
==========

Simple configuration management library for dotnet core applications. It uses embedded database (LiteDB) for keeping data at local path. For remote store it uses super fast Redis.

![alt text](images/arch.png "Kafkaboard")

How to use?
===========

 - Reference DynamicConfiguration.Core library into your project
 - Call new reference and specify your redis address.

 ```csharp

var configurationManager = new ConfigurationReader("SERVICE-A", "redis:6379",15000);

```
-  Specify config key with type

 ```csharp

var configValue = configurationManager.GetValue<string>("SiteName");

```

**Docker Demo** : docker-compose up -d


## Libraries used : 
- LiteDB (http://www.litedb.org/)
- ServiceStack.Redis (https://github.com/ServiceStack/ServiceStack.Redis)
- React (https://reactjs.org/) 
- Reactstrap (https://reactstrap.github.io/)
