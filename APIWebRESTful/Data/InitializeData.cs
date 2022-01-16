using System;
using System.Linq;
using APIWebRESTful.Models;

namespace APIWebRESTful.Data
{
    public class InitializeData
    {
        public static void Initialize(MyContext context)
        {
            if (!context.Heroes.Any())
            {
                context.Heroes.AddRange(
                    new Hero { Id = 1, Name = "SPIDERMAN", IsPopulate = true, Secret = "This is secret" },
                    new Hero { Id = 2, Name = "SUPERMAN", IsPopulate = true, Secret = "This is secret" },
                    new Hero { Id = 3, Name = "CATWOMAN", IsPopulate = false, Secret = "This is secret" }
                );

                context.SaveChanges();
            }

            
        }
    }
}
