# rest-api-example
.NET 6 REST API example, with features:
- OData query
- PATCH partial update with support of settings null values
- mapping between DTOs and EF Database models with automapper
- benchmark request execution time

# Examples

## OData query example
Returns first two addresses ordered by Street, where istreet is not null and street starts with 'Matě'.
```
GET /Addresses?$top=2&$orderby=Street asc&$filter=street NE null AND startswith(street , 'Matě')
```
Example result:
```
{
    "items": [
        {
            "id": 52,
            "country": "Česko",
            "countryCode": "CZ",
            "street": "Matěchova",
            "countryId": 1,
            "orientationNumber": "333"
        },
        {
            "id": 66,
            "country": "Česko",
            "countryCode": "CZ",
            "street": "Matěchova",
            "countryId": 1,
            "orientationNumber": "111"
        }
    ],
    "returnedCount": 2,
    "totalCount": 15001
}
```

## PATCH partial update example

Updates address with ID 10330 - sets orientation number to 25 and set Note to null.
```
PATCH /Addresses/10330
{
      "propsSetToNull": ["Note"],
      "orientationNumber" : "25"
}

```
# Page performance (execution time)

Because of custom middleware **HeaderPerformanceMiddleware**, every HTTP response contains header **performanceMilliseconds** with execution time in milliseconds.
The execution time is counted between startion execution pipeline and starting response flush.

Example of response headers:

```
...

performanceMilliseconds: 309

...
```
