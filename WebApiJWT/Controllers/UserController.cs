using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApiJWT.Attributes;
using WebApiJWT.Constants;
using WebApiJWT.Models;
using WebApiJWT.Repositories;
using WebApiJWT.Exceptions;

namespace WebApiJWT.Controllers
{
    [Route("user")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly DataContext _context;

        public UserController(DataContext context)
        {
            _context = context;
        }

        [Authorize]
        [HttpGet("{id}")]
        public ActionResult Get(int id)
        {
            var response = _context.User.Find(id);

            if (response == null) throw new NotFoundException("User not found.");

            return Ok(response);
        }

        [Authorize(PrivilegeConst.ReadUser)]
        [HttpGet]
        public ActionResult GetAll()
        {
            var response = _context.User.Include(x => x.Role).ToList();
            return Ok(response);
        }

        [Authorize(PrivilegeConst.CreateUser)]
        [HttpPost]
        public ActionResult Create([FromBody] User user)
        {
            _context.User.Add(user);
            _context.SaveChanges();
            return Ok(user);
        }
    }
}
