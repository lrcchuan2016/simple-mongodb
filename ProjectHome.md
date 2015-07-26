A C# implementation of a driver that can be used to communicate with MongoDB. The focus lies in keeping it "dynamic" by using anonymous types and JSON.
  * No need of extending a certain base-class.
  * No need for mappings.
  * No need for using key-value dictionaries, it's up to you

Using a well known JSON/BSON-serializer (http://james.newtonking.com/projects/json-net.aspx).

# Examples #
## Getting started ##
There's a Getting started series here: http://daniel.wertheim.se/2010/04/12/simple-mongodb-part-1-getting-started/

## Insert ##
```
var sessionFactory = new SimoSessionFactory();
using (var session = sessionFactory.GetSession("Pls.Simo.GettingStarted"))
{
    var db = session["MyDatabase"];
    var collection = db.GetCollection<InterestingUrl>();

    var interestingUrl = new InterestingUrl(@"http://code.google.com/p/simple-mongodb") { Description = "The home of the Simple-MongoDB driver." };
    interestingUrl.SetTags("Simple-MongoDB", "Project site");
    collection.Insert(interestingUrl);
}
```
## Querying ##
```
var sessionFactory = new SimoSessionFactory();
using (var session = sessionFactory.GetSession("Pls.Simo.GettingStarted"))
{
    var entityStore = new SimoEntityStore(session, "MyDatabase");

    var interestingUrl = entityStore.FindOne<InterestingUrl>(new { Url = @"http://daniel.wertheim.se" });
}
```

# Latest info #
## Querying, Serialization ##
**v0.1.7, 21/4 2010**

There has been some changes in the Query-API and how serialization works. All to support the article written here: http://daniel.wertheim.se/2010/04/21/simple-mongodb-part-2-anonymous-types-json-embedded-entities-and-references/

## Reworked querying ##
**v0.1.6, 12/4 2010**

I have skipped the operator classes and instead built a fluent API that lets you generate query-expressions.

## JSON.dll and Querying using InOperator ##
**v0.1.5, 8/4 2010**

Looks like I had an incompatible JSON.dll checked in. I really need to get a buildserver up so that I can fetch these problems. It has now been fixed.

I keep getting questions about how to write queries. I will try to start document this, but untill then, <a href='http://daniel.wertheim.se/2010/04/08/simple-mongodb-querying/'>look here</a>.

### Summary ###
  * Updated to working Newtonsoft-JSON.dll
  * Added InOperator

### Examples ###

**$in-operator - Using JSON**
```
var persons = session[DbName][PersonsCollectionName].Find<Person>(@"{Name : { $in : [""Daniel"", ""Sue""] } }");
```

**$in-operator - Using the Simo-InOperator**
```
var persons = session[DbName][PersonsCollectionName].Find<Person>(new InOperator("Name", "Daniel", "Sue"));
```

## Verified against v1.4 of MongoDB and Improved BSON-writer ##
**30/3 2010**

Just checked in a new version of Simple-MongoDB that now runs against MongoDB v1.4.

It now also runs on an improved version of Newtonsoft's Json.Net library, which has gained a improved BSON-writer and built in support for regex, hence my custom version of this lib is no longer necessary.

## Implicit support for Cursors ##
**25/3 2010**

For the latest info and examples of this, see: http://daniel.wertheim.se/2010/03/25/simple-mongodb-implicit-support-for-cursors/

## Support for Regular expressions and Custom Id's in references ##
**22/3 2010**

For the latest info and examples of this, see: http://daniel.wertheim.se/2010/03/22/simple-mongodb-support-for-regex-and-custom-ids/