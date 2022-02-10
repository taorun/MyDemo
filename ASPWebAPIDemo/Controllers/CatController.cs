using ASPWebAPIDemo.Models;
using ASPWebAPIDemo.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ASPWebAPIDemo.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class CatController : ControllerBase
    {
        [HttpGet]
        public ActionResult<List<Cat>> GetAll() => CatService.GetAll();

        [HttpGet("{id}")]
        public ActionResult<Cat> Get(int id) => CatService.GetAll().FirstOrDefault(x => x.Id == id);

        [HttpPost]
        public IActionResult Create(Cat cat)
        {
            if(cat == null)
            {
                return BadRequest();
            }

            CatService.Add(cat);
            return CreatedAtAction(nameof(Create), new {id = cat.Id}, cat);
        }

        /*[HttpPost(Name = "Create2")]
        public IActionResult Create2(Cat cat)
        {
            CatService.Add(cat);
            return CreatedAtAction(nameof(Create), new { id = cat.Id }, cat);
        }*/
    }
}
