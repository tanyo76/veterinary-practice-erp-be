﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Text;
using UsersRestApi.Entities;
using UsersRestApi.Repositories;


public class MyModel
{
    public string email { get; set; }
    public string password { get; set; }
}

namespace UsersRestApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        [HttpPut]
        public IActionResult Put([FromBody] MyModel model)
        {

            UsersRepository repo = new UsersRepository();

            User loggedUser = repo.GetAll().Find(u => u.Email == model.email &&
                                                              u.Password == model.password);

            if (loggedUser == null)
            {
                return BadRequest(new { message = "Invalid credentials" });
            }

            var claims = new[]
            {
                new Claim("LoggedUserId", loggedUser.Id.ToString())
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("843y1rfhuewuiwqjsu9201jsaklsdnsafuieruig213b89"));
            var signingCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                "vetmasters",
                "vetmasters.react.app",
                claims,
                expires: DateTime.UtcNow.AddDays(10),
                signingCredentials: signingCredentials
            );

            JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
            string jwt = tokenHandler.WriteToken(token);

            return Ok(new { success = true, token = jwt, userId = loggedUser.Id });
        }

        [HttpPost]
        public IActionResult Post([FromBody] User userDto)
        {

            UsersRepository repo = new UsersRepository();

            User user = repo.GetAll().Find(u => u.Email == userDto.Email);

            if (user != null)
            {
                return Conflict(new { message = "User with this email already exists" });
            }

            int userId = repo.Add(userDto);

            var claims = new[]
            {
                new Claim("LoggedUserId", userId.ToString())
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("843y1rfhuewuiwqjsu9201jsaklsdnsafuieruig213b89"));
            var signingCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                "vetmasters",
                "vetmasters.react.app",
                claims,
                expires: DateTime.UtcNow.AddDays(10),
                signingCredentials: signingCredentials
            );

            JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
            string jwt = tokenHandler.WriteToken(token);

            return Ok(new { userId, accessToken = jwt });
        }
    }
}
