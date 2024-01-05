# MongoDB Common Repo Package

## Table of Contents
1. [Introduction](#introduction)
2. [Getting Started](#getting-start)
3. [Usage Guide](#usage-guide)
4. [Links](#links)

### Introduction
***
This package is aimed at simplifying connection to, and performing CRUD operation on MongoDB.

## Getting Started
***
To use this package, it is required to have MongoDbSettings section in the appsettings.json with DatabaseName and ConnectionString
properties.
* All the database models must implement the ```IBaseEntity``` interface with include the ```Id: Guid```, ```IsDeprecated: bool```, ```UpdatedAt: DateTime``` and ```CreatedAt: DateTime``` properties.
* The name of the model class would be the collection name.
* In the ```Program.cs``` class, register the ```ConfigureMongoConnection()``` to the service pipeline. In addition to the ```IServiceCollections```, the extension method also accepts the ```IConfiguration``` interface.
* Create a repository class that implements the ```Repository<TCollection>``` class. The ```TCollection``` should be a model class that implements the ```IBaseEntity```. The base class constructor accepts an instance of ```IOptions<MongoDbSettings>```.

## Usage Guide
***
Say you want to have a collection named ```Person``` on MongoDB, create a class with that name Person, and implement the ```IBaseEntity``` interface. This class will have the Name property in addition to properties implemented from the interface:

```public class Person : IBaseEntity
{
    public Guid Id { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public bool IsDeprecated { get; set; }

    public string Name { get; set; }
}
```

Add this section to the ```appsettings.json```

```
 "MongoDbSettings": {
    "ConnectionString": "https://localhost:27017",
    "DatabaseName": "DatabaseName"
  }
```
Add this to the container in the ```Program.cs```

```builder.Services.ConfigureMongoConnection(builder.Configuration);```

Add a repository class and an interface to implement. This repository class will inherit from the ```Repository``` class which has implementations of methods to perform operations with Mongo DB.
Let's call this class and interface, PersonRepository and IPersonRepository respectively.
We are only going to  show the content for the PersonRepository.cs

```
public class PersonRepository : Repository<Person>, IPersonRepository
{
    public PersonRepository(IOptions<MongoDbSettings> options) : base(options) {}

    public async Task AddAsync(Person person) =>
        await CreateAsync(person);

    public async Task AddRangeAsync(ICollection<Person> persons) =>
        await CreateManyAsync(persons);

    public async Task EditAsync(Expression<Func<Person, bool>> expression, Person person) =>
        await UpdateAsync(expression, person);

    public async Task DeleteAsync(Expression<Func<Person, bool>> expression) =>
        await RemoveAsync(expression);

    public async Task<Person?> FindAsync(Expression<Func<Person, bool>> expression) =>
        await GetAsync(expression);

    public async Task<ICollection<Person>> FindManyAsync(Expression<Func<Person, bool>> expression) =>
        await GetManyAsync(expression);

    public IQueryable<Person> FindAsQueryable() =>
        GetAsQueryable();

    public async Task<long> Count(Expression<Func<Person, bool>> expression) =>
        await CountAsync(expression);

    public async Task<bool> Exists(Expression<Func<Person, bool>> expression) =>
        await ExistsAsync(expression);
}
```
You can then, through the IPersonRepository, call this method. Say you injected this interface into a class with the name ```repository```, you can call the method that gets a Person by id as such:
```var person = await repository.FindAsync(p => p.Id.Equals(id));```

## Links
***
To view the source code or get in touch:
* [Github Repository Link](https://github.com/ojotobar/Mongo.Common.Repo)
* [My Email Address](ojotobar@gmail.com)