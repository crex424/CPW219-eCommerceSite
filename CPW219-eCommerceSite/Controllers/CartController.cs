using CPW219_eCommerceSite.Data;
using CPW219_eCommerceSite.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace CPW219_eCommerceSite.Controllers
{
    public class CartController : Controller
    {
        private readonly VideoGameContext _context;
        private const string Cart = "ShoppingCart";

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
            CartGameViewModel cartGame = new()
            {
                GameID = gameToAdd.GameID,
                Title = gameToAdd.Title,
                Price = gameToAdd.Price
            };

            List<CartGameViewModel> cartGames = GetExistingCartData();
            cartGames.Add(cartGame);

            WriteShoppingCartCookie(cartGames);

            TempData["Message"] = "Game added to cart successfully";
            return RedirectToAction("Index", "Games");
        }

        private void WriteShoppingCartCookie(List<CartGameViewModel> cartGames)
        {
            string cookieData = JsonConvert.SerializeObject(cartGames);

            // Add item to a shopping cart
            HttpContext.Response.Cookies.Append(Cart, cookieData, new CookieOptions()
            {
                Expires = DateTimeOffset.Now.AddYears(1)
            });
        }

        private List<CartGameViewModel> GetExistingCartData()
        {
            string? cookie = HttpContext.Request.Cookies[Cart];
            if (string.IsNullOrWhiteSpace(cookie))
            {
                return new List<CartGameViewModel>();
            }

            return JsonConvert.DeserializeObject<List<CartGameViewModel>>(cookie);
        }

        public IActionResult Summary()
        {
            // Read shopping cart data and convert to list of view model
            List<CartGameViewModel> cartGames = GetExistingCartData();
            return View(cartGames);
        }

        public IActionResult Remove(int id)
        {
            List<CartGameViewModel> cartGames = GetExistingCartData();
            CartGameViewModel? targetGame = 
                cartGames.Where(g => g.GameID == id).FirstOrDefault();

            cartGames.Remove(targetGame);

            WriteShoppingCartCookie(cartGames);

            return RedirectToAction(nameof(Summary));
        }
    }
}
