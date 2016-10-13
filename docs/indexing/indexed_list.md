#Indexing a list of fields with ```IndexedList```
MarcelloDB allows multiple index entries per object in the same index.

Let's say for instance you have a ```Book``` class which has a property ```List<string> Tags```

Using an ```IndexedList``` allows you to index every tag in the Tags property.

##Custom Value
If indexing a property is not enough, custom indexes can be defined.
In this case you also define a property like above, except you return an instance of IndexedValue, created with a function that returns the value to be indexed.

```cs
class BookIndexDefinition : IndexDefinition<Book>
{
   public IndexedList<Book, string> Tags
   {
      get {
          return IndexedList((o) => o.Tags);
      }
   }
}
```