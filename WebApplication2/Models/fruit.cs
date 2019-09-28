using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication2.Models
{
    public class Fruit
    {
        [Key]
        public int id { get; set; }
        public string Nome { get; set; }

    }
}
