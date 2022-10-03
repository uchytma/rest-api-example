using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.OData.ModelBuilder;
using RestApiExample.Db;

/// <summary>
/// DTO modely, které vrací/přijímá API.
/// Tvoří rozhraní pro komunikaci a reflektují i business omezení (např. nelze nastavit ID při vytváření entity)
/// Mohou se lišit od DB modelů. Například:
/// - DB model obsahuje password, které ale nechceme posílat přes API
/// - DB model obsahuje číselníky jako string, kdežto my je chceme poskytovat jako enum
/// - atp. ...
/// </summary>
/// 

namespace RestApiExample.Dtos
{

    public class GetAddressDto : AddressDto
    {
        public int Id { get; set; }

        public string? Country { get; set; }

        public string? CountryCode { get; set; }
    }

    public class PatchAddressDto : AddressDto
    {
        /// <summary>
        /// Pole názvů položek, které mají být nastavené na null.
        /// Skutečné položky, které jsou nastavené na null (či nevyplněné), se v rámci PATCH ignorují.
        /// Case-sensitive. Pokud je zadána neexistující položka, tak je ignorována.
        /// </summary>
        public string[]? PropsSetToNull { get; set; } = default!;
    }

    public class AddressDto
    {
        public string? Street { get; set; }

        public int? CountryId { get; set; }

        public string? OrientationNumber { get; set; }

        public string? HouseNubmer { get; set; }

        public DateTime? Created { get; set; }

        public int? CreatedById { get; set; }

        public int? UpdatedById { get; set; }

        public string? Note { get; set; }

        public DateTime? ValidFrom { get; set; }

        public string? ZipCode { get; set; }
    }
}
