using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UsersRestApi.Repositories;
using UsersRestApi.Entities;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace UsersRestApi.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            UsersRepository repo = new UsersRepository();
            User item = repo.GetAll().Find(u => u.Id == id);

            if (item == null)
                return NotFound();

            return Ok(item);
        }

        [HttpPut]
        public IActionResult Put([FromBody] User model)
        {

            UsersRepository repo = new UsersRepository();
            int id = 1;
            foreach (User item in repo.GetAll())
            {
                if (id <= item.Id)
                    id = item.Id + 1;
            }

            model.Id = id;
            repo.Add(model);

            return Created(model.Id.ToString(), model);
        }

        [HttpDelete("{test}")]
        public IActionResult Delete(int test)
        {
            UsersRepository repo = new UsersRepository();

            User user = null;
            foreach (User item in repo.GetAll())
            {
                if (test == item.Id)
                {
                    user = item;
                    break;
                }
            }

            if (user != null)
            {
                repo.Delete(user);
                return Ok(user);
            }

            return NotFound(user);
        }
    }
}
