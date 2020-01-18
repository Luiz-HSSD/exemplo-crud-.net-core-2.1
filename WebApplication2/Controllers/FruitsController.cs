using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Data.Entity;
using WebApplication2.Models;
using System.Data.Entity.Infrastructure;

namespace WebApplication2.Controllers
{
    public class FruitsController : Controller
    {
        private readonly WebApplication2Context _context;

        public FruitsController(WebApplication2Context context)
        {
            _context = context;
            
        }

        // GET: Fruits
        public  IActionResult Index()
        {
            //_context.Fruit.ToList()
            return View( );
        }

        // GET: Fruits/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var fruit =  _context.Fruit
                .FirstOrDefault(m => m.id == id);
            if (fruit == null)
            {
                return NotFound();
            }

            return View(fruit);
        }

        // GET: Fruits/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Fruits/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("id,Nome")] Fruit fruit)
        {
            if (ModelState.IsValid)
            {
                _context.Fruit.Add(fruit);
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            return View(fruit);
        }

        // GET: Fruits/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var fruit =  _context.Fruit.Find(id);
            if (fruit == null)
            {
                return NotFound();
            }
            return View(fruit);
        }

        // POST: Fruits/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("id,Nome")] Fruit fruit)
        {
            if (id != fruit.id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                     _context.Fruit.Update(fruit);
                     _context.SaveChanges();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!FruitExists(fruit.id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(fruit);
        }

        // GET: Fruits/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var fruit =  _context.Fruit
                .FirstOrDefault(m => m.id == id);
            if (fruit == null)
            {
                return NotFound();
            }

            return View(fruit);
        }

        // POST: Fruits/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var fruit =  _context.Fruit.Find(id);
            _context.Fruit.Remove(fruit);
             _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }

        private bool FruitExists(int id)
        {
            return _context.Fruit.Any(e => e.id == id);
        }
    }
}
