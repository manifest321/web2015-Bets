using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace BetsTheBest.Models
{
    static class eventTypes
    {   
        public static string football   = "футбол";
        public static string hockey     = "хоккей";
        public static string basketball = "баскетбол";

        public static List<string> toList()
        {
            return new List<string>()
            {
                "футбол",
                "хоккей",
                "баскетбол"
            };
        }
    }

    static class eventState
    {
        public static string preview    = "превью";
        public static string live       = "лайв";
        public static string finished   = "закончен";

        public static List<string> toList()
        {
            return new List<string>()
            {
                "превью",
                "лайв",
                "закончен",
            };
        }
    }



    public class SportEvent
    {
        [Key]
        [Display(Name="Ид")]
        [HiddenInput(DisplayValue=false)]
        public int Id { get; set; }
        [Display(Name="Команда 1")]
        public string command1 { get; set; }
        [Display(Name = "Команда 2")]
        public string command2 { get; set; }
        [Display(Name = "Игра")]
        public string gameType { get; set; }
        [Display(Name="Дата")]
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString="dd.MM.yyyy hh:mm")]
        public DateTime date { get; set; }
        [Display(Name="Время")]
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString="hh:mm")]
        public DateTime time { get; set; }
        [Display(Name="Соревнование")]
        public string tournament { get; set; }
        [Display(Name="Состояние")]
        public string state { get; set; }
        [Display(Name = "П1")]
        [UIHint("Decimal")]
        public float firstWin { get; set; }
        [Display(Name = "П2")]
        [UIHint("Decimal")]
        public float secondWin { get; set; }
        [Display(Name = "НН")]
        [UIHint("Decimal")]
        public float gameTie { get; set; }
        [Display(Name = "H1")]
        [UIHint("Decimal")]
        public float tieByFirst { get; set; }
        [Display(Name = "H2")]
        [UIHint("Decimal")]
        public float tieBySecond { get; set; }
        [Display(Name = "HН")]
        [UIHint("Decimal")]
        public float overallTie { get; set; }
        [Display(Name = "Тотал")]
        [UIHint("Decimal")]
        public float total { get; set; }
        [Display(Name = "Б")]
        [UIHint("Decimal")]
        public float b { get; set; }
        [Display(Name = "М")]
        [UIHint("Decimal")]
        public float m { get; set; }
    }
}