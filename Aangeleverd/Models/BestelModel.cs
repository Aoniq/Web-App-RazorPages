using System;
using System.ComponentModel.DataAnnotations;

namespace Aangeleverd.Models
{
    public class BestelModel
    {
        [Required(ErrorMessage = "Selecteer een maat")]
        public Maat Maat { get; set; }
    }

    public enum Maat
    {
        S,
        M,
        L,
        XL
    }
}
