# About

**Important** To ensure we will not connect to a database appsetting.json has an invalid connection string.

**Bad**

```json
{
  "ConnectionStrings": {
    "DatabaseConnection": "Data Source=.\\SQLEXPRESS;Initial Catalog=NorthWind2020_NOT;Integrated Security=True"
  }
}
```

**Good**

```json
{
  "ConnectionStrings": {
    "DatabaseConnection": "Data Source=.\\SQLEXPRESS;Initial Catalog=NorthWind2020_NOT;Integrated Security=True"
  }
}
```



