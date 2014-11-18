Marcello
========

Marcello is a  Xamarin/.net/mono in-process NoSql database, targetting mobile devices explicitly.

No more breaking down objets to fit them into sqlite, Marcello saves entire object graphs.

Simply store the entire object and retrieve them back later.

No more packaging exotic binaries neither, Marcello is pure C#.


It will be supported on Xamarin (iOS and Android), Windows 8 and Windows Phone 8


Current Status
=
Marcello is still in an experimental status, but a first alpha version is near.


Usage
=
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
var myObject = new WhateverObject(){Name="Value"};

//get the corresponding collection
var collection = marcello.Collection<WhateverObject>();

//and persist the object
collection.Persist(myObject);
```

Deleting objects

```cs
//Delete the article like this
_articles.Destroy(toiletPaper);
```

How Objects are Identified
=
Marcello needs a single property which uniquely identifies an object.
This value is needed to distinguish an insert from an update, and identify objects to delete.

You can use any of the following conventions, or the ID attribute
```cs
class Article
{
  public string ID {get;set;}
  
  public string Id {get;set;}
  
  public string id {get;set;}
  
  public string ArticleID {get;set;}
  
  public string ArticleId {get;set;}
  
  public string Articleid {get;set;}
  
  [Marcello.Attributes.ID]
  public string CustomIDProp {get;set;}
}
```

Transactions
=
Marcello supports transactions spanning multiple operations over multiple collections
```cs
_marcello.Transaction(() => {
    marcello.Collection<Article>().Persist(article);
    marcello.Collection<Clients>().Persist(client);
    marcello.Collection<Project>().Destroy(project);
}); 
```

Transactions roll back when an exception occurs within the block.
```cs
_marcello.Transaction(() => {
    marcello.Collection<Article>().Persist(article);
    marcello.Collection<Clients>().Persist(client);
    marcello.Collection<Project>().Destroy(project);
    throw new Exception("Nothing happened");
});
```


Indexing
=
TODO

Roadmap
=
First alpha version will include all of below
- ~~Persisting objects (done)~~
- ~~Deleting objects (done)~~
- ~~enumerating objects (done)~~
- ~~reusing storage of deleted objects (done)~~
- ~~cross-collection transaction support (done)~~
- ~~iOS/Android (Xamarin) support (done)~~
- Windows 8 Support
- Windows Phone 8 Support
- Indexing (ID and custom indexes) (todo)
- IOC for Serialization and Storage





