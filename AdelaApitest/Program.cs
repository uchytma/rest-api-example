using RestApiExample.Db;
using RestApiExample.Dtos;
using RestApiExample.Middlewares;
using AutoMapper;
using Microsoft.AspNetCore.OData;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OData.Edm;
using Microsoft.OData.ModelBuilder;

namespace RestApiExample
{
    public class Program
    {
        public class Mapper : Profile
        {
            public Mapper()
            {
                CreateMap<Address, AddressDto>(MemberList.Destination);
                CreateMap<AddressDto, Address>(MemberList.Destination);

                CreateMap<Address, GetAddressDto>(MemberList.Destination)
                    .ForMember(d => d.Country, opt => opt.MapFrom(src => GetCountryName(src)))
                    .ForMember(d => d.CountryCode, opt => opt.MapFrom(src => GetCountryCode(src)));
            }

            private static string? GetCountryName(Address src) => src.Country?.Name ?? null;
            private static string? GetCountryCode(Address src) => src.Country?.Short ?? null;
        
        }

        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddControllers().AddOData(opt => {
                opt.Select().Filter().SetMaxTop(10000).OrderBy();
            });

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(opt => {
                opt.ResolveConflictingActions(apiDescriptions => apiDescriptions.First());
            });
            builder.Services.AddDbContext<AppDbContext>();

            var mapperConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new Mapper());
            });

            builder.Services.AddSingleton<IMapper>(mapperConfig.CreateMapper());

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseMiddleware<HeaderPerformanceMiddleware>();
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();
            app.UseAuthorization();
            app.MapControllers();

            app.Run();
        }
    }
}