using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using APIWebRESTful.Data;
using APIWebRESTful.Filters;
using APIWebRESTful.Models;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace APIWebRESTful.Controllers
{
    [ApiController]
    [Route("api/v1/[Controller]")]
    [ApiConventionType(typeof(DefaultApiConventions))]
    public class HeroesController : ControllerBase
    {
        private readonly ILogger<HeroesController> _logger;
        private readonly MyContext _context;
        private readonly IMapper _mapper;

        public HeroesController(ILogger<HeroesController> logger, MyContext context, IMapper mapper)
        {
            _logger = logger;
            _context = context;
            _mapper = mapper;
        }

        // GET: api/v1/heroes
        [HttpGet]
        [ResponseCache(Duration = 10)]
        [ServiceFilter(typeof(FilterAction))]
        // [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> Get()
        {
            var heroesDto = await _mapper.ProjectTo<HeroDTO>(_context.Heroes)
                .ToListAsync();

            return Ok(heroesDto);
        }
        // GET: api/v1/heroes/5
        [HttpGet("{id:int}")]// Name = "GetHero"
        public async Task<IActionResult> Get(int id)
        {
            var hero = await _context.Heroes.FindAsync(id);
            
            if (hero is null)
                return NotFound($"Hero with Id = {id} not found.");

            var heroDto = _mapper.Map<HeroDTO>(hero);

            return Ok(heroDto);
        }
        // GET: api/v1/heroes/superman
        [HttpGet("{name}")]
        public async Task<IActionResult> Get([FromRoute] string name, [FromQuery] bool populate)
        {
            var hero = await _context.Heroes.FirstOrDefaultAsync(h => h.Name.Contains(name) && h.IsPopulate == populate);

            if (hero is null)
                return NotFound($"Hero with Name = {name} not found.");

            var heroDto = _mapper.Map<HeroDTO>(hero);

            return Ok(heroDto);
        }
        // GET: api/v1/heroes/populate
        [HttpGet("Populate")]
        public async Task<IActionResult> Populate()
        {
            var heroesDto = await _mapper.ProjectTo<HeroDTO>(_context.Heroes.Where(h => h.IsPopulate == true))
                .ToListAsync();

            return Ok(heroesDto);
        }
        // POST: api/v1/heroes
        [HttpPost]
        public async Task<ActionResult<HeroDTO>> Post([FromForm] HeroDTO heroDto)
        {
            if (heroDto is null)
                return BadRequest(ModelState);

            var hero = _mapper.Map<Hero>(heroDto);

            _context.Heroes.Add(hero);

            var result = await _context.SaveChangesAsync();
            if(result <= 0)
                return BadRequest("Your changes have no[t been saved.");

            return CreatedAtAction(nameof(Get), new { id = hero.Id }, _mapper.Map<HeroDTO>(hero));
        }
        // PUT: api/v1/heroes/5
        [HttpPut("{id:int}")]
        public async Task<IActionResult> Put(int id, [FromBody]HeroDTO heroDto)
        {
            if (heroDto is null)
                return BadRequest(ModelState);

            if (id != heroDto.Id)
                return BadRequest("Identifier is not valid or Identifiers don't match.");

            var hero = _context.Heroes.FirstOrDefault(h => h.Id == id);

            if (hero is null)
                return NotFound($"Hero with Id = {id} not found.");

            _mapper.Map(heroDto, hero);

            try
            {
                var result = await _context.SaveChangesAsync();
                if (result <= 0)
                    return BadRequest("Your changes have not been saved.");
            }
            catch (DbUpdateConcurrencyException)
            {
                throw;
            }

            return NoContent();
        }
        // PATCH: api/v1/heroes/5
        [HttpPatch("{id:int}")]
        public async Task<IActionResult> Patch(int id, [FromBody] JsonPatchDocument<HeroDTO> patchDoc)
        {
            if (patchDoc is null)
                return BadRequest(ModelState);

            var hero = await _context.Heroes.FindAsync(id);
            if (hero is null)
                return NotFound($"Hero with Id = {id} not found");

            var heroDto = _mapper.Map<HeroDTO>(hero);

            patchDoc.ApplyTo(heroDto, ModelState);

            var isValid = TryValidateModel(heroDto);
            if(!isValid)
                return BadRequest(ModelState);

            _mapper.Map(heroDto, hero);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                throw;
            }

            return NoContent();
        }
        // DELETE: api/v1/heroes/5
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var hero = await _context.Heroes.FindAsync(id);

            if (hero is null)
                return NotFound($"Hero with Id = {id} not found");

            _context.Heroes.Remove(hero);

            var result = await _context.SaveChangesAsync();
            if (result <= 0)
                return BadRequest();

            return NoContent();
        }

    }
}



/*
 BODY LLAMADA A PATCH:
    [
      {
        "op": "operationName",
        "path": "/propertyName",
        "value": "newPropertyValue"
      }
    ] 
 */