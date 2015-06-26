using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BetsTheBest.Models
{
    public class News
    {
        [Key]
        public int Id { get; set; }
        public string title { get; set; }
        public string href { get; set; }
    }
}