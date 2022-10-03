# rest-api-example
.NET 6 REST API example with OData, PATCH partial update and mapping between DTOs and EF Database models.

# Examples

## Address OData query example
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
            "orientationNumber": "333",
            "houseNubmer": "222",
            "created": null,
            "createdById": null,
            "updatedById": null,
            "note": null,
            "validFrom": null,
            "zipCode": "16000"
        },
        {
            "id": 66,
            "country": "Česko",
            "countryCode": "CZ",
            "street": "Matěchova",
            "countryId": 1,
            "orientationNumber": "111",
            "houseNubmer": "2222",
            "created": null,
            "createdById": null,
            "updatedById": null,
            "note": null,
            "validFrom": null,
            "zipCode": "13000"
        }
    ],
    "returnedCount": 2,
    "totalCount": 15001
}
```

## Address PATCH partial update example

Updates address with ID 10330 - sets orientation number to 25 and set Note to null.
```
PATCH /Addresses/10330
{
      "propsSetToNull": ["Note"],
      "orientationNumber" : "25"
}

```
