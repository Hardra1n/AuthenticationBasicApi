using Application.Dtos;
using Application.Services;
using Common.Dtos;
using Domain;
using Domain.Exceptions;
using Domain.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace Application.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private ILogger<UsersController> _logger;
        private UserService _service;

        public UsersController(ILogger<UsersController> logger, UserService service)
        {
            _logger = logger;
            _service = service;
        }

        [HttpGet("{id}")]
        public IActionResult GetById(Guid id)
        {
            var user = _service.GetById(id).ToIdUserDto();
            return Ok(user);
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            return Ok(_service.GetAll());
        }

        [HttpPut]
        public IActionResult Update([FromBody] UserIdDto user)
        {
            _service.Update(user.ToUser());
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
        public IActionResult AddUser([FromBody] UserIdDto user)
        {
            _service.Add(user.ToUser());
            return Created();
        }

        [HttpGet]
        [Route("/api/ping")]
        public IActionResult TestRequest()
        {
            return Ok(true);
        }
    }
}
