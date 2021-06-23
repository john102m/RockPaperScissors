/**
 * GameStats class used to model the properties of game statistics
 * 
 * 
 * 
 * 
 * John McKinney  23/06/2021
 * 
 * 
 */



using System;
using System.ComponentModel.DataAnnotations;


namespace RockPaperScissors.Models
{
    class GameStats
    {
        [Key]
        public int GameId { get; set; }
        public string Winner { get; set; }

        public string MostMoves { get; set; }

        public int Turns { get; set; }

        public DateTime Date { get; set; }
    }
}
