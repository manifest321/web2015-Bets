using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BetsTheBest.Models
{
    public class Payment
    {
        [Key]
        public int Id { get; set; }
        public string userId { get; set; }
        public DateTime paymentDate { get; set; }
        public float amount { get; set; }
    }
}