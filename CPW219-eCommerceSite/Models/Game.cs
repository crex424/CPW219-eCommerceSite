using System.ComponentModel.DataAnnotations;

namespace CPW219_eCommerceSite.Models
{
    /// <summary>
    /// Represents a single game available for purchase
    /// </summary>
    public class Game
    {
        /// <summary>
        /// Represents the unique idenitfier for each game
        /// </summary>
        [Key]
        public int GameID { get; set; }

        /// <summary>
        /// The game's official title
        /// </summary>
        [Required]
        public string Title { get; set; }

        /// <summary>
        /// The game's sales price
        /// </summary>
        [Range(0, 1000)]
        public double Price { get; set; }

        //TODO: Add rating
    }

    /// <summary>
    /// A single video game that has been added
    /// to the users shopping cart cookie
    /// </summary>
    public class CartGameViewModel
    {
        public int GameID { get; set; }

        public string Title { get; set; }

        public double Price { get; set; }
    }
}
