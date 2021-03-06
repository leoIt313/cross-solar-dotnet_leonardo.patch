﻿using System.ComponentModel.DataAnnotations;

namespace CrossSolar.Models
{
    public class PanelModel
    {
        public int Id { get; set; }

        [Required]
        [Range(-90, 90)]
        //[RegularExpression(@"-?\d{1,2}\.\d{6}")]
        public double Latitude { get; set; }

        [Range(-180, 180)]
        //[RegularExpression(@"-?\d{1,2}\.\d{6}")]
        public double Longitude { get; set; }

        [Required]
        [MinLength(16)]
        [MaxLength(16)]
        public string Serial { get; set; }

        public string Brand { get; set; }
    }
}