MarcelloDB
========

[![Build Status](https://travis-ci.org/markmeeus/MarcelloDB.svg?branch=master)](https://travis-ci.org/markmeeus/MarcelloDB)

MarcelloDB is a mobile NoSql Object database.
It is light-weight and has a minimal memory footprint.

MarcelloDB saves plain C# objects, including child objects, lists and collections. 
Not having to map your objects to the relational model can save you hundreds of lines of code. 

Marcello is pure C#, so there is no need to package other binaries.

Marcello will support Xamarin (iOS and Android), Windows 8.1 and Windows Phone 8.1 and Windows 10

Current Status
=
Marcello is still experimental.

Be carefull. Backwards compatibility with existing data will not be guaranteed untill v1.

Usage
=
MarcelloDB is a Portable Class Library, but you need to create and inject a StreamProvider on the specific platform.
This is because there isn't a platform independent way to interact with the FileSystem.

For iOS and Android you need to use the FileStorageStreamProvider included in the MarcelloDB.netfx assembly.
Currently there is no implementation for Windows 8 and Windows Phone. 
Feel free to contribute... 

Creating the session
```cs
//Create a steam provider for the specific platform
var fileStreamProvider =  new FileStorageStreamProvider("path/to/storage_folder");

//Create a Marcello session, this can be done in PCL code
var marcello = new Marcello(fileStreamProvider);
```

Persisting objects
```cs
//Create your objects however you please
var myObject = new WhateverObject(){ Name = "Value" };

//get the corresponding collection
var collection = marcello.Collection<WhateverObject>();

//and persist the object
collection.Persist(myObject);
```

Find and enumerate your objects
```cs
//get the corresponding collection
var collection = marcello.Collection<WhateverObject>();

//and enumerate
foreach(var obj in collection.All)
{
  DoSomethingWithTheObject(obj);
}

//or find by the ID property
var o = collection.Find(1234);
```

Deleting objects

```cs
//Delete the object
_articles.Destroy(myObject);
```

How Objects are Identified
=
Marcello needs a single property which uniquely identifies an object.
This value is needed to distinguish an insert from an update, and identify objects to delete.

You can use any of the following naming conventions, or add the Marcello.Attributes.ID attribute.
```cs
class Article
{
  public string ID { get; set; }

  public string Id { get; set; }

  public string id { get; set; }

  public string ArticleID { get; set; }

  public string ArticleId { get; set; }

  public string Articleid { get; set; }

  [Marcello.Attributes.ID]
  public string CustomIDProp { get; set; }
}
```

Transactions
=
Marcello supports transactions spanning multiple operations over multiple collections
```cs
marcello.Transaction(() => {
    marcello.Collection<Article>().Persist(article);
    marcello.Collection<Clients>().Persist(client);
    marcello.Collection<Project>().Destroy(project);
});
```

Transactions roll back when an exception occurs within the block.
```cs
marcello.Transaction(() => {
    marcello.Collection<Article>().Persist(article);
    marcello.Collection<Clients>().Persist(client);
    marcello.Collection<Project>().Destroy(project);
    throw new Exception("Nothing happened");
});
```


Indexing
=
Will be implemented in 0.2.x and 0.3.x

Roadmap
=
0.1.0
-
- ~~Persisting objects (done)~~
- ~~Deleting objects (done)~~
- ~~enumerating objects (done)~~
- ~~reusing storage of deleted objects (done)~~
- ~~cross-collection transaction support (done)~~
- ~~iOS/Android (Xamarin) support (done)~~
- ~~Indexing (ID)~~
- Windows 8 Support
- Windows Phone 8 Support
- 

0.2.0
-
- Indexed Properties

0.3.0
-
- Indexed Properties of nested objects

 






