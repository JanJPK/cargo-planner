using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using CargoPlanner.Algos;
using CargoPlanner.API.Db;
using CargoPlanner.API.Utility;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Input = CargoPlanner.API.Dtos.Input;
using Model = CargoPlanner.Models;
using ListItem = CargoPlanner.API.Dtos.ListItem;

namespace CargoPlanner.API.Controllers
{
    [ApiController]
    [Route("api/v1/instances")]
    [Produces("application/json")]
    public class InstanceController : ControllerBase
    {
        private readonly IAlgo algo;
        private readonly IGenericRepository<Model.Instance, Guid> instanceRepository;
        private readonly IMapper mapper;
        private readonly IGenericRepository<Model.Result, Guid> resultRepository;

        public InstanceController(IGenericRepository<Model.Instance, Guid> instanceRepository,
                                  IGenericRepository<Model.Result, Guid> resultRepository,
                                  IAlgo algo,
                                  IMapper mapper)
        {
            this.instanceRepository = instanceRepository;
            this.resultRepository = resultRepository;
            this.algo = algo;
            this.mapper = mapper;
        }

        /// <summary>
        ///     Returns instance with specific id.
        /// </summary>
        /// <response code="200">Returns instance with specific id</response>
        /// <response code="404">Instance with specific id does not exist</response>
        [HttpGet("{id:guid}")]
        [ProducesResponseType(typeof(Input.Instance), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetById(Guid id)
        {
            var instance = await instanceRepository.GetById(id);

            if (instance == null)
            {
                return NotFound();
            }

            var instanceDto = mapper.Map<Input.Instance>(instance);
            instanceDto.Items = instance.Items.Pack();

            return Ok(instanceDto);
        }

        /// <summary>
        ///     Returns short summary of all instances.
        /// </summary>
        [HttpGet("{userId:guid}/summary")]
        [ProducesResponseType(typeof(List<ListItem.Instance>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetInstanceSummary(Guid userId)
        {
            var instances = await instanceRepository.Get()
                                                    .Where(e => e.UserId == userId)
                                                    .OrderByDescending(e => e.CreationDate)
                                                    .ToListAsync();

            return Ok(mapper.Map<List<ListItem.Instance>>(instances));
        }

        /// <summary>
        ///     Creates a new instance.
        /// </summary>
        /// <response code="200">Returns instance's id</response>
        [HttpPost]
        [ProducesResponseType(typeof(Guid), StatusCodes.Status200OK)]
        public async Task<IActionResult> Post([FromBody] Input.Instance instanceDto)
        {
            var instance = mapper.Map<Model.Instance>(instanceDto);
            instance.Items = instanceDto.Items.Unpack();

            await instanceRepository.Insert(instance);

            return Ok(instance.Id);
        }

        /// <summary>
        ///     Updates an instance.
        /// </summary>
        /// <response code="200">Instance has been replaced</response>
        /// <response code="404">Instance with specific id does not exist</response>
        [HttpPut("{id:guid}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Put(Guid id,
                                             [FromBody] Input.Instance instanceDto)
        {
            var instance = await instanceRepository.GetById(id);

            if (instance == null)
            {
                return NotFound();
            }

            var newInstance = mapper.Map<Model.Instance>(instanceDto);
            newInstance.Items = instanceDto.Items.Unpack();

            instance.Truck = newInstance.Truck;
            instance.Items = newInstance.Items;

            await instanceRepository.Update(instance);

            return NoContent();
        }

        /// <summary>
        ///     Deletes the instance.
        /// </summary>
        /// <response code="200">Instance has been deleted</response>
        /// <response code="404">Instance with specific id does not exist</response>
        [HttpDelete("{id:guid}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteById(Guid id)
        {
            if (!await instanceRepository.Exists(id))
            {
                return NotFound();
            }

            await instanceRepository.Delete(id);

            return NoContent();
        }

        /// <summary>
        ///     Solves the instance.
        /// </summary>
        /// <response code="200">Returns result id for specified instance</response>
        /// /// <response code="404">Instance has no items</response>
        /// <response code="404">Incorrect instance id</response>
        [HttpGet("{id:guid}/solve")]
        [ProducesResponseType(typeof(Guid), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Solve(Guid id)
        {
            var instance = await instanceRepository.GetById(id);

            if (instance == null)
            {
                return NotFound();
            }

            if (instance.Items.Count == 0)
            {
                return BadRequest();
            }

            var algoResult = algo.Solve(instance, ItemsOrder.VolumeDesc);

            var result = new Model.Result(instance, algoResult);
            await resultRepository.Insert(result);

            return Ok(result.Id);
        }

    }
}