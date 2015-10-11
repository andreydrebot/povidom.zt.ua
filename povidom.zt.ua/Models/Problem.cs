using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace povidom.Models
{
    public enum Status
    {
        [Display(Name = "Нова")]
        New,
        [Display(Name = "Чекає на відповідь")]
        Waiting,
        [Display(Name = "Вирішена")]
        Resolved
    }

    public class Problem
    {
        public Problem()
        {
            CreatedOn = DateTime.Now;
        }

        public int Id { get; private set; }
        public DateTime CreatedOn { get; private set; }

        public string Description { get; set; }

        public Status Status { get; set; }

        public string Comment { get; set; }

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