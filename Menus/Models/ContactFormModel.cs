using System.ComponentModel.DataAnnotations;

namespace Menus.Models
{
    public class ContactFormModel
    {
        [Required(ErrorMessage = "Името е задължително")]
        [StringLength(100, ErrorMessage = "Името трябва да е по-малко от 100 символа")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "Имейлът е задължителен")]
        [EmailAddress(ErrorMessage = "Невалиден имейл адрес")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Съобщението е задължително")]
        [StringLength(1000, ErrorMessage = "Съобщението трябва да е по-малко от 1000 символа")]
        public string Message { get; set; } = string.Empty;
    }
}
