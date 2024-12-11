using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UsersRestApi.Repositories;
using UsersRestApi.Entities;
using System.Collections.Generic;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace UsersRestApi.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        [HttpGet]
        public IActionResult Get(int id)
        {
            UsersRepository repo = new UsersRepository();
            List<User> items = repo.GetAll().FindAll(u => u.Role != "Owner");

            return Ok(items);
        }

        [HttpPost]
        public IActionResult Post([FromBody] User userDto)
        {
            UsersRepository repo = new UsersRepository();
            int userId = repo.Add(userDto);

            return Ok(new { userId });
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

        [HttpDelete("{userId}")]
        public IActionResult Delete(int userId)
        {
            EmployeeToClinicRepository empToClinicRepo = new EmployeeToClinicRepository();

            EmployeeToClinic emptToClinicRecord = empToClinicRepo.GetAll().Find(emp => emp.UserId == userId);

            if (emptToClinicRecord != null)
            {
                empToClinicRepo.Delete(emptToClinicRecord);
                return Ok();
            }

            return NotFound(emptToClinicRecord);
        }
    }
}
