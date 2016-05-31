#Transactions

To avoid data corruption, all changes are written to a write ahead journal and applied as a single atomic and durable action.
MarcelloDB does this for calls to Persist and Destroy automatically.

You can extend the transaction to make it span multiple data mutations.

A transaction runs on a session and can include changes in multiple collections from multiple collection files.

> Warning: only collections obtained from that session will be included in the transaction. If you start to mix multiple sessions, you're on your own.


```cs
session.Transaction(() => {
    articleCollection.Persist(article);
    clientCollection.Persist(client);
    projectCollection.Destroy(project);
});
```

Transactions roll back when an exception occurs within the block.
```cs
session.Transaction(() => {
    articleCollection.Persist(article);
    clientCollection.Persist(client);
    projectCollection.Destroy(project);
    throw new Exception("Nothing happened");
});
```
