namespace Menus.Models;
using System.ComponentModel.DataAnnotations;

public class Reservation
{
        public int Id { get; set; }

        [Required(ErrorMessage = "Моля, въведете име")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Моля, въведете телефонен номер")]
        [Phone]
        public string Phone { get; set; }

        [Required(ErrorMessage = "Моля, изберете дата и час")]
        public DateTime ReservationDate { get; set; }

        [Range(1, 20, ErrorMessage = "Броят гости трябва да е между 1 и 20")]
        public int Guests { get; set; }

        public string Note { get; set; }
}