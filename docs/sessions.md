##Sessions

Using MarcelloDB starts with the creation of a session.

The session manages access to the filesystem as well as transactions and concurrency.
Interaction with MarcelloDB always requires a valid session.

MarcelloDB is a portable class library, but requires injection of implementations of platform specific features.
(Currently, MarcelloDB only relies on a platform specific implementation for reading and writing files.)
These platform specifics are implemented in ```Platform```

The Platform implementation for mono/.net (for Xamarin iOS and Android, and regular mono/.net) can be found in the Marcello.netfx assembly.

The implementation for Windows Phone 8.1 and Windows 8.1/10 Apps can be found in the Marcello.uwp assembly.

###Creating a session requires a platform
Create the platform object
```cs
var platform =  new Platform();
```

Then create the session using the platform and a path to a folder. MarcelloDB will read and write it's data to and from this folder.

(You can do this in a portable class library)
```cs
var session = new MarcelloDB.Session(platform, "/path/to/data/folder/");
```

The session will remain valid until it is garbage collected or disposed. A good approach would be to keep it around for a long time. Attach it to your App instance, keep it in a static or singleton or inside a IOC Container.

##CollectionFiles

MarcelloDB organizes it's data in collection-files and collections.
A session manages one or more collection-files. A collection-file contains one or more collections.

A collection-file maps directly to a file in the sessions folder.

This file contains the data and indexes of all collections in that collection-file.

> If it makes sense in your use-case - *a product catalog for instance* - you could generate a collection-file on the server and download it from your clients.

You access a collection-file by it's actual filename.

```cs
// products.data is the actual file name within the sessions folder
var productsFile = session["products.data"];
```

##Collections

A collection is a bit like a table in a SQL Database.

It manages objects of a specific type, including subclasses.

Deep down inside, MarcelloDB is a key-value store. It saves objects and allows you to retreive them back (or update/delete) by their ID.
Since this ID is so important, it has been made part of the construction of a collection.

A collection is a generic type with at least 2 generic type parameters. The first one is the type of the objects you want to store, the second one is the type of your object's ID value.
  > The third generic parameter is the index definition. Read about [indexes](indexes.html)


When constructing the collection, you need to give it a name - like you give tables a name - and you have to provide a function to retrieve the ID from an object.

like this:
```cs
//             Store instances of  [Book]  [with string ID]    [get ID from book]
//                                   ||      ||                       ||
var books = productsFile.Collection<Book, string>("books", book => book.BookId);

//             Store instances of  [Dvd] [with Guid ID] [get ID from dvd]
//                                  ||    ||                  ||
var dvds = productsFile.Collection<Dvd, Guid>("dvd", dvd => dvd.id);
```

If you use different collection-names you can have multiple collections for the same type within one collection-file.
```cs
var dvdCollection = productsFile.Collection<Dvd, Guid>("dvds", dvd => dvd.id);
var newDvdCollection = productsFile.Collection<Dvd, Guid>("new-dvds", dvd => dvd.id);
```
