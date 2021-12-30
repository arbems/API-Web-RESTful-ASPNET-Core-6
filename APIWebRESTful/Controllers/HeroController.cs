using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using APIWebRESTful.Data;
using APIWebRESTful.Models;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace APIWebRESTful.Controllers
{
    [ApiController]
    [Route("api/v1")]
    public class HeroController : ControllerBase
    {
        private readonly ILogger<HeroController> _logger;
        private readonly MyContext _context;
        private readonly IMapper _mapper;

        public HeroController(ILogger<HeroController> logger, MyContext context, IMapper mapper)
        {
            _logger = logger;
            _context = context;
            _mapper = mapper;
        }

        [HttpGet("heroes")]
        public async Task<IEnumerable<HeroDTO>> Get()
        {
            return await _mapper.ProjectTo<HeroDTO>(_context.Heroes)
                .ToListAsync();
        }

        [HttpGet("hero/{id}")]
        public async Task<ActionResult<HeroDTO>> Get(int id)
        {
            var hero = await _context.Heroes.FindAsync(id);
            
            if (hero is null)
                return NotFound();

            var heroDto = _mapper.Map<HeroDTO>(hero);

            return Ok(heroDto);
        }

        [HttpPost]
        public async Task<ActionResult<HeroDTO>> Post([FromBody] HeroDTO heroDto)
        {
            var hero = _mapper.Map<Hero>(heroDto);

            _context.Heroes.Add(hero);

            var result = await _context.SaveChangesAsync();
            if(result <= 0)
                return BadRequest("Your changes have not been saved.");

            return CreatedAtAction(nameof(Get), new { id = hero.Id }, _mapper.Map<HeroDTO>(hero));
        }
        
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Put(int id, HeroDTO heroDto)
        {
            if (id != heroDto.Id)
                return BadRequest("Identifier is not valid or Identifiers don't match.");

            var hero = _mapper.Map<Hero>(heroDto);

            _context.Entry(hero).State = EntityState.Modified;

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

        [HttpPatch("{id}")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Patch(int id, [FromBody] JsonPatchDocument<HeroDTO> patchDoc)
        {
            if (patchDoc is null)
                return BadRequest();

            var hero = await _context.Heroes.FindAsync(id);
            if (hero is null)
                return NotFound();

            var heroDto = _mapper.Map<HeroDTO>(hero);

            patchDoc.ApplyTo(heroDto, ModelState);

            var isValid = TryValidateModel(hero);
            if (!isValid)
                return BadRequest(ModelState); 

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

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<IActionResult> Delete(int id)
        {
            var hero = await _context.Heroes.FindAsync(id);

            if (hero is null)
                return NotFound();

            _context.Heroes.Remove(hero);

            var result = await _context.SaveChangesAsync();
            if (result <= 0)
                return BadRequest();

            return NoContent();
        }

    }
}

