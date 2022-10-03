using RestApiExample.Db;
using RestApiExample.Dtos;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Query.Validator;
using Microsoft.EntityFrameworkCore;
using System.Linq.Dynamic.Core;
using System.Reflection;

namespace RestApiExample.Controllers
{
    [ApiController]
    [Route("addresses")]
    public class AddressesController : ControllerBase
    {
        private readonly AppDbContext _dbContext;
        private readonly IMapper _mapper;

        public AddressesController(AppDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        [HttpGet]
        [Route("")]
        public IActionResult Get(ODataQueryOptions<GetAddressDto> queryOptions)
        {
            ODataValidationSettings s = new ODataValidationSettings()
            {
                AllowedQueryOptions = AllowedQueryOptions.Top | AllowedQueryOptions.OrderBy | AllowedQueryOptions.Filter | AllowedQueryOptions.Select,
                MaxTop = 2000,
            };

            queryOptions.Validate(s);
            var dbData = _dbContext.Addresses.Include(d => d.Country).AsNoTracking();
            var totalCount = dbData.Count();
            var mappedToDto = _mapper.ProjectTo<GetAddressDto>(dbData);
            var queryResults = queryOptions.ApplyTo(mappedToDto).ToDynamicList();
            return Ok(new Result(queryResults, queryResults.Count(), totalCount));
        }

        [HttpPost]
        [Route("")]
        public async Task<IActionResult> Create(AddressDto model)
        {
            var dbModel = _mapper.Map<Address>(model);
            _dbContext.Addresses.Add(dbModel);
            await _dbContext.SaveChangesAsync();
            return Ok();
        }

        [HttpPut]
        [Route("{id}")]
        public async Task<IActionResult> Update([FromRoute] int id, AddressDto model)
        {
            var dbModel = _dbContext.Addresses.Where(d => d.Id == id).SingleOrDefault() ?? throw new ApplicationException("entity not found.");
            var _ = _mapper.Map<AddressDto, Address>(model, dbModel);
            await _dbContext.SaveChangesAsync();
            return Ok();
        }

        [HttpPatch]
        [Route("{id}")]
        public async Task<IActionResult> Patch([FromRoute] int id, PatchAddressDto model)
        {
            var dbModel = _dbContext.Addresses.Where(d => d.Id == id).SingleOrDefault() ?? throw new ApplicationException("entity not found.");
            foreach (PropertyInfo propertyInfo in model.GetType().GetProperties())
            {
                var value = propertyInfo.GetValue(model, null);
                var name = propertyInfo.Name;

                if (name == "PropsSetToNull") continue;

                if (value != null)
                {
                    if (model.PropsSetToNull != null && model.PropsSetToNull.Contains(name))
                        throw new ApplicationException("U položky označené jako 'nastavit na null' je zároveň nastavena hodnota.");
                    _dbContext.Entry(dbModel).Property(name).CurrentValue = value;
                }

                if (model.PropsSetToNull != null && model.PropsSetToNull.Contains(name))
                {
                    _dbContext.Entry(dbModel).Property(name).CurrentValue = null;
                }
            }

            await _dbContext.SaveChangesAsync();
            return Ok();
        }

        record Result(IEnumerable<dynamic> Items, long? ReturnedCount, long? TotalCount);
    }
}