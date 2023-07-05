using CRUDwithDapper.Models;
using Dapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Npgsql;
using System.Data.SqlClient;
using System.Runtime.InteropServices;

namespace CRUDwithDapper.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SuperHeroController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly NpgsqlConnection _connection;

        public SuperHeroController(IConfiguration configuration)
        {
            _configuration = configuration;
            _connection = new NpgsqlConnection(_configuration.GetConnectionString("DefaultConnection"));
        }

        [HttpGet]
        public async Task<ActionResult<List<SuperHero>>> GetAllSuperHeroes()
        {
            var heroes = await _connection.QueryAsync<SuperHero>("select * from superheroes");
            return Ok(heroes);
        }
        [HttpGet("{heroId}")]
        public async Task<IActionResult> GetSingleHero(int heroId)
        {
            var hero = await _connection.QueryAsync<SuperHero>("Select * from superheroes where Id=@id", new { Id = heroId });
            return Ok(hero);
        }
        [HttpPost]
        public async Task<IActionResult> CreateHero(SuperHero hero)
        {
            await _connection.ExecuteAsync("insert into superheroes (name, firstname, lastname, city) values (@Name, @FirstName, @LastName, @City)", hero);
            return Ok(); 
        }
        [HttpPut]
        public async Task<IActionResult> UpdateHero(SuperHero hero)
        {
            await _connection.ExecuteAsync("update superheroes set name=@Name,firstname=@FirstName, lastname=@LastName, city=@City where id=@Id",hero);
            return Ok();
        }
        [HttpDelete("{heroId}")]
        public async Task<IActionResult> UpdateHero(int heroId)
        {
            await _connection.ExecuteAsync("delete from superheroes where id=@Id", new {Id=heroId});
            return Ok();
        }


    }
}
