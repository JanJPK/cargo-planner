using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using CargoPlanner.API.Db;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Result = CargoPlanner.API.Dtos.Result;
using Model = CargoPlanner.Models;
using ListItem = CargoPlanner.API.Dtos.ListItem;

namespace CargoPlanner.API.Controllers
{
    [ApiController]
    [Route("api/v1/results")]
    [Produces("application/json")]
    public class ResultController : ControllerBase
    {
        private readonly IGenericRepository<Model.Result, Guid> resultRepository;
        private readonly IMapper mapper;

        public ResultController(IGenericRepository<Model.Result, Guid> resultRepository,
                                IMapper mapper)
        {
            this.resultRepository = resultRepository;
            this.mapper = mapper;
        }

        /// <summary>
        ///     Returns result with specific id
        /// </summary>
        /// <response code="200">Returns result with specific id</response>
        /// <response code="404">Result with specific id does not exist</response>
        [HttpGet("{id:guid}")]
        [ProducesResponseType(typeof(Result.Result), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetById(Guid id)
        {
            var result = await resultRepository.GetById(id);

            if (result == null)
            {
                return NotFound();
            }

            return Ok(mapper.Map<Result.Result>(result));
        }

        /// <summary>
        ///     Returns short summary of all results.
        /// </summary>
        [HttpGet("{userId:guid}/summary")]
        [ProducesResponseType(typeof(List<ListItem.Result>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetResultSummary(Guid userId)
        {
            var instances = await resultRepository.Get()
                                                  .Where(e => e.UserId == userId)
                                                  .OrderByDescending(e => e.CreationDate)
                                                  .ToListAsync();

            return Ok(mapper.Map<List<ListItem.Result>>(instances));
        }
    }
}
