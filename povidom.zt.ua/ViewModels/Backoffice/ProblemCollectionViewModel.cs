using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace povidom.ViewModels.Backoffice
{
    public enum SortBy
    {
        [Display(Name = "Найстаріші зверху")]
        Oldest,
        [Display(Name = "Найновіші зверху")]
        Newest
    }

    public class ProblemCollectionViewModel
    {
        public ProblemCollectionViewModel()
        {
            ShowNew = true;
            ShowWaiting = true;
            ShowResolved = false;

            ShowDelegated = true;
            ShowNotDelegated = true;
        }

        public IEnumerable<ProblemViewModel> Problems { get; set; }

        public bool ShowNew { get; set; }
        public bool ShowWaiting { get; set; }
        public bool ShowResolved { get; set; }

        public bool ShowDelegated { get; set; }
        public bool ShowNotDelegated { get; set; }

        public string Street { get; set; }
        public string Building { get; set; }
        public string Flat { get; set; }

        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string Middlename { get; set; }
        public string Phone { get; set; }

        public SortBy SortBy { get; set; }
    }
}