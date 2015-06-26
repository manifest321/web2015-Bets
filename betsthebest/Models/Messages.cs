using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BetsTheBest.Models
{
    public class Messages
    {
        [Key]
        public int Id { get; set; }
        public string src { get; set; }
        public string dest { get; set; }
        public string text { get; set; }
        public DateTime date { get; set; }
    }
}