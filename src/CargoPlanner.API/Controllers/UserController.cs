using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using CargoPlanner.API.Db;
using CargoPlanner.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Input = CargoPlanner.API.Dtos.Input;
using Model = CargoPlanner.Models;

namespace CargoPlanner.API.Controllers
{
    [ApiController]
    [Route("api/v1/users")]
    [Produces("application/json")]
    public class UserController : ControllerBase
    {
        private readonly IGenericRepository<User, Guid> userRepository;
        private readonly IMapper mapper;

        public UserController(IGenericRepository<Models.User, Guid> userRepository,
                              IMapper mapper)
        {
            this.userRepository = userRepository;
            this.mapper = mapper;
        }

        /// <summary>
        ///     Creates a new user.
        /// </summary>
        /// <response code="200">Returns user's id</response>
        [HttpPost("register")]
        [ProducesResponseType(typeof(Guid), StatusCodes.Status200OK)]
        public async Task<IActionResult> Register([FromBody] Input.User userDto)
        {
            var userId = await userRepository.Insert(CreateUser(userDto));

            return Ok(userId);
        }

        /// <summary>
        ///     Login.
        /// </summary>
        [HttpPost("login")]
        [ProducesResponseType(typeof(Guid), StatusCodes.Status200OK)]
        public IActionResult Login([FromBody] Input.User userDto)
        {
            var user = userRepository.Get().FirstOrDefault(u => u.Username == userDto.Username);

            if (user == null)
            {
                return NotFound();
            }

            var password = GenerateHash(userDto.Password, user.PasswordSalt);

            if (password == user.PasswordHash)
            {
                return Ok(user.Id);
            }
            else
            {
                return Unauthorized();
            }
        }

        private Model.User CreateUser(Input.User userDto)
        {
            var salt = CreateSalt();
            var password = GenerateHash(userDto.Password, salt);
            var user = new Model.User
            {
                Username = userDto.Username,
                PasswordHash = password,
                PasswordEncryption = "SHA256",
                PasswordSalt = salt
            };
            return user;
        }

        private string CreateSalt(int size = 64)
        {
            var rng = new RNGCryptoServiceProvider();
            var buffer = new byte[size];
            rng.GetBytes(buffer);
            return Convert.ToBase64String(buffer);
        }

        private string GenerateHash(string password, string salt)
        {
            byte[] saltedPassword = Encoding.UTF8.GetBytes(password + salt);
            var sha256 = new SHA256Managed();
            byte[] hashedPassword = sha256.ComputeHash(saltedPassword);
            return Convert.ToBase64String(hashedPassword);
        }
    }
}