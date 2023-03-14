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

            int currPage = id ?? 1; // set current page to id if it has a maxNumPages, otherwise use 1

            int totalNumOfProducts = await _context.Games.CountAsync();
            double maxNumPages = Math.Ceiling((double)totalNumOfProducts / NumGamesToDisplayPerPage);
            int lastPage = Convert.ToInt32(maxNumPages); // rounding pages up to next whole page number

            /* 
             * Commented out method syntax version, same code below as query syntax
             * List<Game> games = _context.Games
             *                             .Skip(NumGamesToDisplayPerPage * (currPage - PageOffset))
             *                             .Take(NumGamesToDisplayPerPage)
             *                             .ToList();
            */

            List<Game> games = await (from game in _context.Games
                                      select game)
                                      .Skip(NumGamesToDisplayPerPage * (currPage - PageOffset))
                                      .Take(NumGamesToDisplayPerPage)
                                      .ToListAsync();

            GameCatalogViewModel catalogModel = new(games, lastPage, currPage);

            // Show them on the page
            return View(catalogModel);
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
