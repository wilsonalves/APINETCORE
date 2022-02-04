using System;
using System.Net;
using System.Threading.Tasks;
using Api.Domain.Dtos.User;
using Api.Domain.Entities;
using Api.Domain.Interfaces.Services.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace api.Application.Controllers
{

    [Route("api/Users")]
    [ApiController]


    public class UsersController : ControllerBase
    {
        private IUserService _service;
        public UsersController(IUserService service)
        {
            _service = service;
        }


        [Authorize("Bearer")]
        [HttpGet]
        public async Task<ActionResult> GetAll()
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                return Ok(await _service.GetAll());
            }
            catch (ArgumentException ex)
            {

                return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
            }

        }
        //localhost:5000/api/Users/2894389439584
        [Authorize("Bearer")]
        [HttpGet]
        [Route("{id}", Name = "GETID")]
        public async Task<ActionResult> Get(Guid id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                return Ok(await _service.Get(id));
            }
            catch (ArgumentException ex)
            {

                return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
            }

        }

        [Authorize("Bearer")]
        [HttpPost]
        [Route("")]
        public async Task<ActionResult> Post([FromBody] UserDto user)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                var result = await _service.Post(user);
                if (result != null)
                {
                    return Created(new Uri(Url.Link("GETID", new { id = result.Id })), result);
                }
                else
                {
                    return BadRequest();
                }

            }
            catch (Exception ex)
            {

                return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [Authorize("Bearer")]
        [HttpPut]

        public async Task<ActionResult> PUT([FromBody] UserDto user)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                var result = await _service.Put(user);
                if (result != null)
                {
                    return Ok(result);
                    //return Created(new Uri(Url.Link("GETID", new { id = result.Id })), result);
                }
                else
                {
                    return BadRequest();
                }

            }
            catch (ArgumentException ex)
            {

                return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [Authorize("Bearer")]
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(Guid id)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                var result = await _service.Delete(id);
                if (result)
                {
                    return Ok(result);
                    //return Created(new Uri(Url.Link("GETID", new { id = result.Id })), result);
                }
                else
                {
                    return BadRequest();
                }

            }
            catch (ArgumentException ex)
            {

                return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
            }
        }
    }

}
