using System;
using System.Linq;
using APIWebRESTful.Models;

namespace APIWebRESTful.Data
{
    public class SeedData
    {
        public static void Initialize(MyContext context)
        {
            if (!context.Heroes.Any())
            {
                context.Heroes.AddRange(
                    new Hero { Id = 1, Name = "SPIDERMAN", IsComplete = true, Secret = "This is secret" },
                    new Hero { Id = 2, Name = "SUPERMAN", IsComplete = true, Secret = "This is secret" },
                    new Hero { Id = 3, Name = "CATWOMAN", IsComplete = true, Secret = "This is secret" }
                );

                context.SaveChanges();
            }

            
        }
    }
}
