using CPW219_eCommerceSite.Data;
using CPW219_eCommerceSite.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CPW219_eCommerceSite.Controllers
{
    public class GamesController : Controller
    {
        private readonly VideoGameContext _context;

        public GamesController(VideoGameContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(int? id)
        {
            const int NumGamesToDisplayPerPage = 3;
            const int PageOffset = 1; // need a page offset to use current page/figure out num games to display

            int currPag = id ?? 1; // set current page to id if it has a value, otherwise use 1

            // Commented out method syntax version, same code below as query syntax
            //List<Game> games = _context.Games
            //                           .Skip(NumGamesToDisplayPerPage * (currPag - PageOffset))
            //                           .Take(NumGamesToDisplayPerPage)
            //                           .ToList();

            List<Game> games = await (from game in _context.Games
                                      select game)
                                      .Skip(NumGamesToDisplayPerPage * (currPag - PageOffset))
                                      .Take(NumGamesToDisplayPerPage)
                                      .ToListAsync();

            // Show them on the page
            return View(games);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Game g)
        {
            if (ModelState.IsValid)
            {
                _context.Games.Add(g); // Prepares insert
                await _context.SaveChangesAsync(); // Executes pending insert

                ViewData["Message"] = $"{g.Title} was added successfully!";
                return View();
            }

            return View(g);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            Game? gameToEdit = await _context.Games.FindAsync(id);
            if (gameToEdit == null)
            {
                return NotFound();
            }

            return View(gameToEdit);
        }
        [HttpPost]

        public async Task<IActionResult> Edit(Game gameModel)
        {
            if (ModelState.IsValid)
            {
                _context.Games.Update(gameModel);
                await _context.SaveChangesAsync();

                TempData["Message"] = $"{gameModel.Title} was updated successfully!";
                return RedirectToAction("Index");
            }
            return View(gameModel);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        { 
            Game? gameToDelete = await _context.Games.FindAsync(id);
            if (gameToDelete == null)
            {
                return NotFound();
            }
            return View(gameToDelete);
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            Game gameToDelete = await _context.Games.FindAsync(id);
            // If a game is selected to be deleted, delete game.
            if (gameToDelete != null)
            {
                _context.Games.Remove(gameToDelete);
                await _context.SaveChangesAsync();
                TempData["Message"] = $"{gameToDelete.Title} was deleted successfully";
                return RedirectToAction("Index");
            }
            TempData["Message"] = "This game was already deleted";
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Details(int id) 
        { 
            Game? gameDetails = await _context.Games.FindAsync(id);
            if (gameDetails == null)
            {
                return NotFound();
            }

            return View(gameDetails);
        
        }   
    }
}
