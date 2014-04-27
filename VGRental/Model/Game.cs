
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace VGRental.Model
{
    public class Game
    {
        public string SystemName { get; set; }
        public virtual System System { get; set; }
        public string GameName { get; set; }
        public bool? Multiplayer { get; set; }
        public int? NumberOfPlayers { get; set; }
        public string Rating { get; set; }
        public string Genre { get; set; }
        public string Developer { get; set; }
        public string Publisher { get; set; }

        [DisplayFormat(DataFormatString = "{0:d}")]
        public DateTime? ReleaseDate { get; set; }

        public int QuantityAvailable { get; set; }
        public int TotalQuantity { get; set; }

        public virtual ICollection<Order> Orders { get; set; }
    }
}
