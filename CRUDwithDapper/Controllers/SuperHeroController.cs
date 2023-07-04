using CRUDwithDapper.Models;
using Dapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;
using System.Runtime.InteropServices;

namespace CRUDwithDapper.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SuperHeroController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public SuperHeroController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpGet]
        public async Task<ActionResult<List<SuperHero>>> GetAllSuperHeroes()
        {
            using var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection"));
            var heroes = await connection.QueryAsync<SuperHero>("select * from superheroes");
            return Ok(heroes);
        }
        [HttpPost]
        public async Task<IActionResult> CreateHero(SuperHero hero)
        {
            using var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection"));
            await connection.ExecuteAsync("insert into superheroes (name, firstname, lastname, city) values (@Name, @FirstName, @LastName, @City)", hero);
            return Ok();
        }
    }
}
