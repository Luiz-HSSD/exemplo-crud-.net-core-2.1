using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApplication2.Models;
using Newtonsoft.Json;
using LinqKit;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;

namespace WebApplication2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FruitspagedController : ControllerBase
    {
        private readonly WebApplication2Context _context;

        public FruitspagedController(WebApplication2Context context)
        {
            _context = context;
        }

        // GET: api/Fruitspaged
        [HttpGet]
        public dynamic GetFruit(DataTableAjaxPostModel model)
        {
            // action inside a standard controller
            int filteredResultsCount;
            int totalResultsCount;
            var res = new dev().YourCustomSearchFunc(_context,model, out filteredResultsCount, out totalResultsCount);

            var result = new List<Fruit>(res.Count);
            foreach (var s in res)
            {
                // simple remapping adding extra info to found dataset
                result.Add(new Fruit
                {
                    id = s.id,
                    Nome = s.Nome
                 
                });
            };

            return new
            {
                // this is what datatables wants sending back
                draw = model.draw,
                recordsTotal = totalResultsCount,
                recordsFiltered = filteredResultsCount,
                data = result
            };
        }


        /*
        // GET: api/Fruitspaged
        [HttpGet]
        public IEnumerable<Fruit> GetFruit()
        {
            return _context.Fruit.Take(10);
        }
        */
        // GET: api/Fruitspaged/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetFruit([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var fruit = await _context.Fruit.FindAsync(id);

            if (fruit == null)
            {
                return NotFound();
            }

            return Ok(fruit);
        }

        // PUT: api/Fruitspaged/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutFruit([FromRoute] int id, [FromBody] Fruit fruit)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != fruit.id)
            {
                return BadRequest();
            }

            _context.Entry(fruit).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!FruitExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Fruitspaged
        [HttpPost]
        public async Task<IActionResult> PostFruit([FromBody] Fruit fruit)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.Fruit.Add(fruit);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetFruit", new { id = fruit.id }, fruit);
        }

        // DELETE: api/Fruitspaged/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteFruit([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var fruit = await _context.Fruit.FindAsync(id);
            if (fruit == null)
            {
                return NotFound();
            }

            _context.Fruit.Remove(fruit);
            await _context.SaveChangesAsync();

            return Ok(fruit);
        }

        private bool FruitExists(int id)
        {
            return _context.Fruit.Any(e => e.id == id);
        }
    }
}