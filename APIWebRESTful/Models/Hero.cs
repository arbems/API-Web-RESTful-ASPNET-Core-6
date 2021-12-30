using System;
using System.ComponentModel.DataAnnotations;

namespace APIWebRESTful.Models
{
    public class Hero
    {
        [Key]
        public int Id { get; set; }
        public string? Name { get; set; }
        public bool IsComplete { get; set; }
        public string? Secret { get; set; }
    }
}
