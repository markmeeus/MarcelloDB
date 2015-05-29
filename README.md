MarcelloDB
========

[![Build Status](https://travis-ci.org/markmeeus/MarcelloDB.svg?branch=master)](https://travis-ci.org/markmeeus/MarcelloDB)

MarcelloDB is a mobile NoSql database.
It is very light-weight with minimal memory footprint.

MarcelloDB saves plain C# objects, including child objects, lists and collections.
Not having to map your objects to the relational model can save you hundreds of lines of code.

MarcelloDB is a pure C# implementation, so there is no need to package other binaries.

MarcelloDB is mostly portable code, and will support Xamarin (iOS and Android), Windows 8.1 and Windows Phone 8.1 and Windows 10

Current Status
=
Marcello is still in an experimental phase.

Be carefull. Backwards compatibility with existing data will not be guaranteed untill v1.

Sessions
=
Using MarcelloDB starts with the creation of a session.
The session makes sure you have access to the actual files where the data is stored. 

But before you can create the session, you'll have to create a FileStorageStreamProvider.
MarcelloDB is a portable class library, but the actual interaction with file system is not the same on every platform.

So first: create the FileStorageStreamProvider in your platform specific project.

```cs
//Create a stream provider for the specific platform
var fileStreamProvider =  new FileStorageStreamProvider("path/to/storage_folder");
```

Then create the actual session. 
(You can do this in a portable class library)
```cs
//Create a Marcello session, this can be done in PCL code
var session = new MarcelloDB.Session(fileStreamProvider);
```

CollectionFiles and Collections
=
MarcelloDB organizes it's data in collections and collection files. A session can handle multiple collection files and a collection file can handle multiple collections.

(This way you can have all read-only data in a single file which you can download straight from your servers. A second file could be used to write data.)

You access collection files like this:
```cs
var productsFile = session["products"];
```

With this file, you can start accessing collections.
Collections are a bit like tables, the main difference is that they contain entire objects, not just rows with colums.
A collection can only handle objects of a specific type (including subclasses);

To start working with a collection, simple access it like this:
```cs
var bookCollection = productsFile.Collection<Book>(); //Deals with instances of Book or subclasses of Book
var dvdCollection = productsFile.Collection<Dvd>();
```

Persisting objects
=
This is where it gets really easy. Once you have this collection, just throw an instance at it:
```cs
var book = new Book(){ ID = "123",  Title = "The Girl With The Dragon Tattoo" };
bookCollection.Persist(book);
```

Enumerating a collection
=
A collection exposes an All property which implements IEnumerable.
You can use it iterate all objects in the collection, and of course use Linq on it.
(Note that when using Linq, it is still enumerating the collection)
```cs
foreach(var book in bookCollection.All){}
```

Finding objects
=
You can find a specific object by it's ID (more on ID's below) like this:
Find uses an index to search for the object. Depending on the size of your collection, a Find will be way faster than iterating the collection to find it.
```cs
var book = bookCollection.Find(123)
```
(In later versions, you'll be able to search for other properties using the index mechanism as well)

Deleting objects
=
Delete your objects like this:
```cs
bookCollection.Destroy(book);
```

How Objects are Identified
=
MarcelloDB needs a single property which uniquely identifies an object.
This value is needed to distinguish an insert from an update, and identify objects to delete.

You can use any of the following naming conventions, or add the MarcelloDB.Attributes.ID attribute.
```cs
class Article
{
  public string ID { get; set; }

  public string Id { get; set; }

  public string id { get; set; }

  public string ArticleID { get; set; }

  public string ArticleId { get; set; }

  public string Articleid { get; set; }

  [MarcelloDB.Attributes.ID]
  public string CustomIDProp { get; set; }
}
```

Transactions
=
To avoid data corruption, all changes are written to a write ahead journal and applied as a single atomic and durable action.  
MarcelloDB does this for calls to Persist and Destroy, but you can extend the transaction to make it span multiple data mutations.

A transaction runs on a session and can include changes in multiple collections from multiple collection files.

(Warning: only collections obtained from that session will be included in the transaction. If you start to mix multiple sessions, you're on your own.)
```cs
session.Transaction(() => {
    session["articles.dat"].Collection<Article>().Persist(article);
    session["project_management.dat"].Collection<Clients>().Persist(client);
    session["project_management.dat"].Collection<Project>().Destroy(project);
});
```

Transactions roll back when an exception occurs within the block.
```cs
session.Transaction(() => {
    session["articles.dat"].Collection<Article>().Persist(article);
    session["project_management.dat"].Collection<Clients>().Persist(client);
    session["project_management.dat"].Collection<Project>().Destroy(project);
    throw new Exception("Nothing happened");
});
```

Encryption
=
Data is stored in an unencrypted format. An encryption engine is available as a pro extension. Please contact mark.meeus at gmail.com for more info. 

Indexes
=
Will be implemented in 0.3.x and 0.4.x

Roadmap
=
0.1.0
-
- ~~Persisting objects~~
- ~~Deleting objects~~
- ~~enumerating objects~~
- ~~reusing storage of deleted objects~~
- ~~cross-collection transaction support~~
- ~~Indexing~~
- ~~iOS/Android (Xamarin) support~~
- ~~Polymorphic collections with Polymorphic children~~
- ~~Windows 8.1 Support~~
- ~~Windows Phone 8.1 Support~~

0.2.0
-
- Metadata Records (named records)

0.3.0
-
- Indexed Properties

0.4.0
-
- Indexed Properties of nested objects

1.0.0
-
- Stable file format, to be supported in all future 1.x versions






