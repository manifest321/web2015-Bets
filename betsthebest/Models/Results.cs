using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BetsTheBest.Models
{
    public class Results
    {
        [Key]
        public int Id { get; set; }
        [Display(Name = "ID события")]
        public int eId { get; set; }
        [Display(Name = "Результат")]
        public int result { get; set; }
    }
}