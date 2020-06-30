using System;
using System.Threading.Tasks;
using CargoPlanner.Algos;
using CargoPlanner.API.Db;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Input = CargoPlanner.API.Dtos.Input;
using Model = CargoPlanner.Models;
using ListItem = CargoPlanner.API.Dtos.ListItem;

namespace CargoPlanner.API.Controllers
{
    [ApiController]
    [Route("api/v1/health")]
    [Produces("application/json")]
    public class HealthController : ControllerBase
    {
        private readonly IGenericRepository<Model.Instance, Guid> instanceRepository;
        private readonly IGenericRepository<Model.Result, Guid> resultRepository;
        private readonly IGenericRepository<Model.User, Guid> userRepository;
        private readonly IAlgo algo;

        public HealthController(IGenericRepository<Model.Instance, Guid> instanceRepository,
                                IGenericRepository<Model.Result, Guid> resultRepository,
                                IGenericRepository<Model.User, Guid> userRepository,
                                IAlgo algo)
        {
            this.instanceRepository = instanceRepository;
            this.resultRepository = resultRepository;
            this.userRepository = userRepository;
            this.algo = algo;
        }

        [HttpGet("tracked-entities")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> Get()
        {
            var response = new
            {
                TrackedEntities = new string[]
                {
                    $"{await instanceRepository.Count()} instances",
                    $"{await resultRepository.Count()} results",
                    $"{await userRepository.Count()} users"
                },
                AlgoType = algo.GetType().Name
            };

            return Ok(response);
        }
    }
}