# MapperNet
MapperNet is a simple and little ORM written in C#. It Requires **.NET Framework 4.5.1**.<br/>
Up to now it has been tested with **SQL Server 2014**.

MapperNet is based on the Data Mapper desing Pattern (see http://martinfowler.com/eaaCatalog/dataMapper.html).

The library files are placed in the MapperNet folder.
The MapperNet.Console folder contains a simple Console Application used for demo purpose.

## Usage
First you have to configure the App.config (or Web.config) of your application, adding the connection string to your database. The file should appear like this:
```
<?xml version="1.0" encoding="utf-8"?>
<configuration>
    <startup> 
        <supportedRuntime version="v4.0" sku=".NETFramework,Version=v4.5.1"/>
    </startup>
  <connectionStrings>
    <add name="DefaultConnection" connectionString="Data Source=yourServerName;Initial Catalog=yourDatabaseName;Integrated Security=True" providerName="System.Data.SqlClient"/>
  </connectionStrings>
</configuration>
```
Where the provider name depends on the database engine you are using (SQL Server, MySQL, etc.).

Next you have to extend the EntityTable<TModel> class like this:
```
public class PersonMapper : EntityTable<Person>
{
  public PersonMapper() : base() { }
}
```
If your connection string's name is different from "DefaultCollection", your class should be:
```
public class PersonMapper : EntityTable<Person>
{
  public PersonMapper() : base("YourConnectionStringName") { }
}
```
To map your entity class, you have to use the TableAttribute and the ColumnAttribute, writing a class like this:
```
[Table]
public class Person
{
  [Column(IsPrimaryKey=true)]
  public int Id { get; set; }

  [Column]
  public string FirstName { get; set; }

  [Column]
  public string LastName { get; set; }

  [Column]
  public DateTime DateOfBirth { get; set; }

  [Column]
  public int Age { get; set; }
}
```
By default the table name is the same of the entity you're mapping (Person) and the columns name are the same of the object properties. If you want to specify different table's or columns name, you could write this:
```
[Table(Name="YourDatabaseTableName")]
public class Person
{
  [Column(Name="YourIdColumnName", IsPrimaryKey=true)]
  public int Id { get; set; }

  [Column(Name=YourFirstNameColumnName")]
  public string FirstName { get; set; }

  // ... other properties
}
```
Properties which are not decorated with the Column attribute are not mapped in the Mapper class.

Once you have mapped your entity you could start querying it using the mapper class.<br />
If you want to retrieve all entities in the table, you could call the Query() method and the use LINQ to query your collection:
```
var personMapper = new PersonMapper();
var people = personMapper.Query();

people.Where(p => p.FirstName == "John").OrderBy(p => p.Age);
```
If you need you could specify a sql string and optionally some parameters:
```
var personMapper = new PersonMapper();

var people = personMapper.Query("Your query here"); // Without parameters

// With parameters
IDictionary<string, object> parameters = new Dictionary<string, object>(){
  {"name", value},
  // ...other parameters
};
var peopleWithParameters = personMapper.Query("Your query here", parameters);
```

To add an entity simply create a new instance and the call the Insert method, like this:
```
Person newPerson = new Person();
// Here populate the object properties

var personMapper = new PersonMapper();
personMapper.Insert(newPerson);
```

To update or delete an entity you have to call the Update and the Delete method. For example:
```
var personMapper = new PersonMapper();

var person = personMapper.Query.Where(p => p.Id == 1).FirstOrDefault();

// Update the person's first name
person.FirstName = "Your new first name";
personMapper.Update(person);

// Delete the person found
personMapper.Delete(person);
```
