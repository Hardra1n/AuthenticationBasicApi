﻿using Application.Services;
using Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace Application.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : Controller
    {
        private ILogger<UserController> _logger;
        private UserService _service;

        public UserController(ILogger<UserController> logger, UserService service)
        {
            _logger = logger;
            _service = service;
        }

        [HttpGet("{id}")]
        public IActionResult GetById(Guid id)
        {
            var user = _service.GetById(id);
            return Ok(user);
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            return Ok(_service.GetAll());
        }

        [HttpPut]
        public IActionResult Update([FromBody] User user)
        {
            _service.Update(user);
            return Ok();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(Guid id)
        {
            _service.Delete(id);
            return Ok();
        }

        [HttpPost]
        [Authorize]
        public IActionResult AddUser([FromBody] User user)
        {
            _service.Add(user);
            return Created();
        }
    }
}
