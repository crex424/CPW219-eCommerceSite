using CPW219_eCommerceSite.Data;
using CPW219_eCommerceSite.Models;
using Microsoft.AspNetCore.Mvc;

namespace CPW219_eCommerceSite.Controllers
{
    public class CartController : Controller
    {
        private readonly VideoGameContext _context;

        public CartController(VideoGameContext context)
        {
            _context = context;
        }

        public IActionResult Add(int id)
        {
            Game? gameToAdd = _context.Games.Where(g => g.GameID == id).SingleOrDefault();

            if (gameToAdd == null)
            {
                // games with specified id does not exist
                TempData["Message"] = "Sorry that game no longer exists on this website";
                RedirectToAction("Index", "Games");
            }
            // ToDo: Add item to a shopping cart
            TempData["Message"] = "Game added to cart successfully";
            return RedirectToAction("Index", "Games");
        }
    }
}
