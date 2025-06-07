namespace Menus.Models;

    public class Menu
    {
        public int Id { get; set; }
        public string RestaurantName { get; set; }
        public string Description { get; set; }
        public ICollection<Dish> Dishes { get; set; }
    }