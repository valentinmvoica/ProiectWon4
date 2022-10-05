using Data.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ProiectWon4.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SeedController : ControllerBase
    {
        private readonly CatalogueDbContext context;

        public SeedController(CatalogueDbContext context)
        {
            this.context = context;
        }
        /// <summary>
        /// SEEDS the db
        /// </summary>
        [HttpPost]
        public void Seed() {

            this.context.Database.EnsureCreated();

            var Student1 = new Student { FirstName = "Marin", LastName = "Chitac", Age = 23 };
            var Student2 = new Student { FirstName = "Chitac", LastName = "Marin", Age = 32 };
            var Student3 = new Student { FirstName = "Andrei", LastName = "Popa", Age = 23 };


            var address1 = new Address { City = "Timisoara", Street = "Libertatii", Number = 32 };
            var address2 = new Address { City = "Iasi", Street = "Unirii", Number = 44 };
            var address3 = new Address { City = "Bucuresti", Street = "Revolutiei", Number = 12 };

            Student1.Address = address1;
            Student2.Address = address2;
            Student3.Address = address3;

            this.context.Add(Student1);
            this.context.Add(Student2);
            this.context.Add(Student3);

            context.SaveChanges();
        }
    }
}


