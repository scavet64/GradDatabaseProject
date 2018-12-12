#GradDatabaseProject

The web application is built using ASP.Net Core and can be found is under the kinabalu folder. In order for it to make a connection to the database you must create a secrets.json file under the kinabalu/kinabalu folder. Modify the "DefaultConnection" so that is has matching values for your own local database.

**Sample secrets.json file**
```
{
  "ConnectionStrings": {
    "DefaultConnection": "server=localhost; userid=CHANGEME; password=CHANGEME; port=3306; database=CHANGEME; Convert Zero Datetime=True; TreatTinyAsBoolean=false;"
  }
}

```