using System;
using povidom.Models;

namespace povidom.ViewModels.Backoffice
{
    public class ProblemViewModel
    {
        public int Id { get; set; }
        public DateTime CreatedOn { get; set; }

        public string Description { get; set; }

        public Status Status { get; set; }

        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string Middlename { get; set; }

        public string Phone { get; set; }

        public string Street { get; set; }
        public string Building { get; set; }
        public string Flat { get; set; }

        public bool IsDelegated { get; set; }
    }
}