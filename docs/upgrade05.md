#Upgrading to 0.5
~~Code compatible with 0.4 is expected to compile and run without modifications with 0.5.~~

##Breaking changes
There is a breaking change in code between 0.4 and 0.5.

`IndexDefinition.Find` has been renamend to `IndexDefinition.Equals`.

##Data compatibility
Data from 0.4 remains readable, but existing indexes are no longer readable so they will have to be rebuilt.

Copying all objects to a new collection will do the trick. It is even better to copy the data to a new collection-file and delete the previous one to save space.