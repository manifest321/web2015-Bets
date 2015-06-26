using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BetsTheBest.Models
{
    public class Bet
    {
        [Key]
        public int Id { get; set; }
        public int betEventId { get; set; }
        public string userId { get; set; }
        public float amount { get; set; }
        public string state { get; set; }
        public string betType { get; set; }
    }

    public class BetEventViewModel
    {
        public Bet currBet { get; set; }
        public SportEvent currEvent { get; set; }
        public float betСoefficient { get; set; }
    }
}