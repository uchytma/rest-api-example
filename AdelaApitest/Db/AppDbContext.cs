using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RestApiExample.Db
{
    public class AppDbContext : DbContext
    {
        public DbSet<Address> Addresses { get; set; } = default!;

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            options.UseMySQL($"server=127.0.0.1;port=53407;database=adela;user=user;password=pass");
            options.LogTo(Console.WriteLine);
        }
    }

    /// <summary>
    /// Definice DB modelu. Musí odpovídat realitě, co je aktuálně v DB struktuře.
    /// </summary>
    [Table("address")]
    public class Address
    {
        [Key]
        [Column("id")]
        public int? Id { get; set; }

        [Column("street")]
        public string? Street { get; set; }

        [Column("country_id")]
        public int? CountryId { get; set; }

        public Country? Country { get; set; }

        [Column("orientation_number")]
        public string? OrientationNumber { get; set; }

        [Column("house_number")]
        public string? HouseNubmer { get; set; }

        [Column("created")]
        public DateTime? Created { get; set; }

        [Column("created_by_id")]
        public int? CreatedById { get; set; }

        [Column("updated_by_id")]
        public int? UpdatedById { get; set; }

        [Column("note")]
        public string? Note { get; set; }

        [Column("valid_from")]
        public DateTime? ValidFrom { get; set; }

        [Column("zip_code")]
        public string? ZipCode { get; set; }
    }

    [Table("country")]
    public class Country
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Column("name")]
        public string Name { get; set; } = default!;

        [Column("short")]
        public string Short { get; set; } = default!;
    }
}
