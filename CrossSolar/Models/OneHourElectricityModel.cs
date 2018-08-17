using System;
using System.ComponentModel.DataAnnotations;

namespace CrossSolar.Models
{
    public class OneHourElectricityModel
    {
        public int Id { get; set; }

        [Required]
        public long KiloWatt { get; set; }

        [Required]
        public DateTime DateTime { get; set; }

    }
}