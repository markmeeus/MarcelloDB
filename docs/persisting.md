##Persisting objects

Once you have a collection, you can save your objects using the Persis method.

```cs
var book = new Book(){ bookId = "123",  Title = "The Girl With The Dragon Tattoo" };
bookCollection.Persist(book);
```

Persist inserts if there is no object associated with the ID. Otherwise it will perform an update.

##Destroying objects

The Destroy method will remove the object with the given ID from the collection.

```cs
bookCollection.Destroy("123");
```
