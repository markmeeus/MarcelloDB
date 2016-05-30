##Quickstart Guide
###Step 1: Install  MarcelloDB using nuget.
MarcelloDB is released on nuget.org, so it is pretty easy to install.

```cs
PM > Install-Package MarcelloDB
```

Alternatively, you can use your IDE to install the package. It is called 'MarcelloDB'.

###Step 2: Create a platform object.
MarcelloDB needs a specific Platform object to interact with the specific OS.

You should create it in your platform specific code.

For instance, if you are building an iOS app, you need to instantiate the platform in the iOS code.

```cs
  //On Android, iOS and regular .net or mono:
  IPlatform platform =  new MarcelloDB.netfx.Platform();

  //In a windows (or phone) 8.1/10
  IPlatform platform =  new MarcelloDB.uwp.Platform();

```

###Step 3: Open a session
You access all your data - in that specific folder - via a Session instance.
Without a sesion, there is little you can do.

Opening a session requires the platform instance and a directory path.

MarcelloDB will store all data in this directory.

This code is platform independent and can be implemented in a PCL.

```cs
var session = new MarcelloDB.Session(platform, dataPath);
```
Sessions are designed to be long-lived. It's ok to open the session when your app starts, and keep it open until your app closes.

A static or singleton is perfectly fine, it also makes sense to register it in an IOC container.


###Step 4: Access a collection
A collection is the MarcelloDB equivalent of a table.

The session manages one or more collection-files. A collection-file manages one or more collections.

Get a collection-file from the session
```cs
//                        File in the datafolder
//                              ||
var personsFile = session["persons.dat"];
```

Then, get a collection from the collection-file. It needs a name, and an Object-To-ID mapping function.
```cs
//                        Store instances of  [Person][ID is a string] [map Person to ID]
//                                               ||      ||   Collection name   ||
//                                               ||      ||        ||           ||
var personsCollection = personsFile.Collection<Person, string>("persons", p => p.Id);

```
The two generic arguments are there so that the compiler will protect you from mistakes. The first one (Person) is the type of objects you can store in the collection. The second one (string) is the type of the ID. The Object-To-ID mapper takes a Person and should return a string.
> This collection will happily store subclasses Person too.

> Also, the entire object is stored, including child objects, lists and dictionaries.

> Bson serialization is done with json.net. If json.net can serialize it, MarcelloDB can store it.

###Step 5: Save you an object
Get you hands on some data
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

###Step 6: Find your data
The simplest way to find data is by it's ID. The find method does just that.
```cs
var jon = personsCollection.Find("1");
```
> Remember the Collection*<Person, string>* ?

> Find now takes a *string* and returns a *Person*.

You can also find your data using custom indexes, find out more on the indexing page. (coming soon.)


###Step 7: Delete your data
If you are really sure Jon won't come back...
```cs
personsCollection.Delete("1");
```