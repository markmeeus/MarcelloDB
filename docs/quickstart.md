##Quickstart Guide
###Install  MarcelloDB using nuget.
MarcelloDB is released on nuget.org, so it is pretty easy to install.

```cs
PM > Install-Package MarcelloDB
```

###Create a platform object.
MarcelloDB's core library is platform independent. Platform specific details are implemented in so called `Platform` objects.

We can find a platform object for regular .net, which works for Xamarin (iOS and Android) in MarcelloDB.netfx.dll.

The implementation for Windows Universal Platform (10) in MarcelloDB.uwp.dll.


```cs
  //On Android, iOS and regular .net or mono:
  IPlatform platform = new MarcelloDB.netfx.Platform();

  //For Windows (Phone) 8.1 (Store apps)
  IPlatform platform = new MarcelloDB.WPA81.Platform();

  //For Windows Universal Platform (win 10)
  IPlatform platform = new MarcelloDB.uwp.Platform();

```

###Open a session
Before we can start accessing data, we should create a `Session` object.

Opening a session requires the platform instance (see previous step) and a directory path. MarcelloDB will store all data in this directory.

SpecialFolder.Personal isn't a bad choice, but any path where you have access should work.

  > If you are getting UnauthorizedAccessException, you are probably trying to store the data in a folder where you have no access

```cs
var dataPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);

//This code is platform independent so we can implement this in shared code.
//                              Injecting the platform here
//                                      ||
var session = new MarcelloDB.Session(platform, dataPath);
```
Sessions are designed to be long-lived. We can open the session when our app starts, and keep it open until it closes.

A static or singleton is perfectly fine, it also makes sense to register it in an IOC container.


###Access a collection
A `Collection` is the MarcelloDB equivalent of a table. Collections are kept together in a `CollectionFile`.

In other words, a `CollectionFile` is a file containing one or more collections.

These files are stored in the sessions directory.

We can get a collection-file from the session like this:
```cs
//                        File in the data directory
//                              ||
var personsFile = session["persons.dat"];
```

Then, we can get a collection from the collection-file.

It needs a name and an Object-To-ID mapping function which should return the unique ID for the given object.

```cs
//                        Store instances of  [Person][ID is a string] [map Person to ID]
//                                               ||      ||   Collection name   ||
//                                               ||      ||        ||           ||
var personsCollection = personsFile.Collection<Person, string>("persons", p => p.Id);

```
The two generic arguments are there so that the compiler will protect us from mistakes. The first one (Person) is the type of objects you can store in the collection. The second one (string) is the type of the ID. The Object-To-ID mapper takes a Person and should return a string.
> This collection will happily store subclasses of Person too.

> Also, the entire object is stored, including child objects, lists and dictionaries.

> Objects are serialized to Bson (Binary json) with json.net. If json.net can serialize it, MarcelloDB can store it.

###Saving an object

Let's create some object
```cs
var person = new Person{
  id = "1",
  Name = "Snow",
  FirstName = "Jon",
  Addresses = new List<Address>{
    new Address{ City = "Castle Black" },
    new Address{ City = "Winterfell" }
  }
}
```

And save it in the collection
```cs
personsCollection.Persist(person);
```

###Finding data
The simplest way to find data is by it's ID. The find method does just that.
```cs
var jon = personsCollection.Find("1");
```
> Remember the Collection*<Person, string>* ?

> Find now takes a *string* and returns a *Person*.

There are plenty more options to search and enumerate our objects if we define [indexes](indexes.html) on our collection, .

###Deleting data
If we are really sure Jon won't come back...
```cs
personsCollection.Delete("1");
```