using PlantRater.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlantRater.Models
{
    public class PlantListItem
    {
        public int PlantId { get; set; }
        public string Name { get; set; }
        [ForeignKey(nameof(Color))]
        [Display(Name="Color")]
        public int ColorId { get; set; }
        public virtual Color Color { get; set; }
        public int Rating { get; set; }

        [Display(Name = "Created")]
        public DateTimeOffset CreatedUtc { get; set; }
    }
}
