using System.ComponentModel.DataAnnotations;
using povidom.Models;

namespace povidom.ViewModels.Backoffice
{
    public class EditProblemViewModel
    {
        [Required]
        public string Description { get; set; }

        [Required]
        public string Lastname { get; set; }

        [Required]
        public string Firstname { get; set; }

        [Required]
        public string Middlename { get; set; }

        [Required]
        public string Phone { get; set; }

        [Required]
        public Status Status { get; set; }

        [Required]
        public string Street { get; set; }

        [Required]
        public string Building { get; set; }

        [Required]
        public string Flat { get; set; }

        public string Comment { get; set; }
    }
}