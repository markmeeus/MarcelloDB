#Indexing one or more fields with ```IndexedValue```

##Matching property
The simplest way to define an index is to define a matching property on the ```IndexDefinition```

This property has to
* have the same name as the property you want to index
* be of type `IndexedValue<T, TAttribute>` (where T equals the type of objects in the collection)
* have the same type for TAttribute as the type of the property you want to index
* has to defined as ```{ get; set; }``` (or at least behave similar)

If any of these requirements is not met, the Collection method on the collection-file will throw as soon as you try to get a reference to it. So, you either get a collection with valid and working indexes, or you get an error.

Once you have this collection, all index functionality is typed correctly. The compiler will prevent you from errors.

An example:
```cs
///
/// This IndexDefinition can be used to index
//                            instances of Book
///                                        ||
class BookIndexDefinition : IndexDefinition<Book>
{
   //          Create an index on property Title
   //                                 ||
   public  IndexedValue<Book, string> Title { get; set; }
   //                   ||     ||
   //             of Book which is a string
}
```

##Custom Value
If indexing a property is not enough, custom indexes can be defined.
In this case you also define a property like above, except you return an instance of IndexedValue, created with a function that returns the value to be indexed.

```cs
class BookIndexDefinition : IndexDefinition<Book>
{
    public  IndexedValue<Book, string> MainAuthorName
    {
        get{
          //index the name of the first autor
          return base.IndexedValue(
            (book) => book.Authors.First().Name // function returning the value to be indexed.
          );
        }
        /* setter not needed, nor usefull */
    }
}
```


##Compound Values
An IndexedValue can also be created with multiple fields.

In this case we should declare the property using ```IndexedValue<T, TAttr1, TAttr2, ...>```

```cs
class BookIndexDefinition : IndexDefinition<Book>
{
    public  IndexedValue<Book, int, string> CategoryIdAndMainAuthorName
    {
        get{
            //index the CategoryId and Name of the first autor
            return IndexedValue(
              (book) => CompoundValue(book.CategoryId, book.Authors.First().Name)
            );
        }
        /* setter not needed, nor usefull */
    }
}
```

##Unique Index Values
Unique indexes make sure that there are no 2 keys with the same value in the index.
If we would try to `Persist` an object that has a duplicate value for a given index, MarcelloDB will throw an exception.

We can define a unique index by using `UniqueIndexedValue` instead of `IndexedValue`.
```cs
//With a simple property
class BookIndexDefinition : IndexDefinition<Book>
{
    public  UniqueIndexedValue<Book, string> ISBN {get;set;}
}
//OR with a custom value
class BookIndexDefinition : IndexDefinition<Book>
{
    public  UniqueIndexedValue<Book, string> ISBN
    {
        get{
            return UniqueIndexedValue(
              book => book.ISBN
            );
        }
    }
}
```

Besides the regular enumeration methods `All`, `GreaterThan`, `SmallerThan` and `Between`, a unique indexed value also has a `Find` method which returns a single object or null;

```cs
var aSongOfIceAndFire = bookCollection.Indexes.ISBN.Find("9780553386790");
//is the equivalent of
var aSongsOfIceAndFire = bookCollection.Indexes.ISBN.Equals("9780553386790").FirstOrDefault();
```

##Conditional indexes
Conditional indexes allow us to specify the circumstances under which an entry to the index is made.
For instance, it makes no sense to add books to the ISBN index if an ISBN number is not available.
MarcelloDB allows us to define an IndexedValue (or UniqueIndexedValue) with a `Func<T, bool>` to decide whether this object should be added to the index.

Like this:
```cs
class BookIndexDefinition : IndexDefinition<Book>
{
    public  UniqueIndexedValue<Book, string> ISBN
    {
        get{
            return UniqueIndexedValue(
              book => book.ISBN,
              book => book.ISBN != null //Only index this book if the ISBN isn't null
            );
        }
    }
}
```

##If it is comparable, it can be indexed
Although base types are preferred as indexed values for performance reasons, any type can be used as index entry.

The only requirement is that it implements IComparable.

```cs
class DeliveryZone : IComparable
{
    public int PostalCode { get; set; }
    public string CityName { get; set; }

    // DeliveryZone compared first by cityName, then by postalCode
    public int CompareTo(object obj)
    {
        var otherDeliveryZone = (DeliveryZone)obj;
        var cityCompared = this.CityName.CompareTo(otherDeliveryZone.CityName);
        if(cityCompared  == 0)
        {
          return this.PostalCode.CompareTo(otherDeliveryZone.PostalCode);
        }
        return cityCompared ;
     }
}
```

```cs
class OrderIndexDefinition : IndexDefinition<Order>
{
  public  IndexedValue<Order, DeliveryZone> DeliveryZone {
    get {
      return IndexedValue(
        (order)=>order.DeliveryAddress.Zone;
      );
    }
  }
}
```


```cs
//Get all orders, ordered by their delivery zone
orderCollection.Indexes.DeliveryZone.All
```

Please note that cases similar to the example above, are better implement using a compound index.
Custom objects in indexes are serialized as Bson while Compound indexes are using a specialized serializer which performs a lot better.

```cs
class OrderIndexDefinition : IndexDefinition<Order>
{
  public  IndexedValue<Order, int, string> DeliveryZone {
    get {
      return IndexedValue(
        (order)=>CompoundValue(order.DeliveryAddress.Zone.ZipCode, order.deliveryAddress.Zone.City);
      );
    }
  }
}
``