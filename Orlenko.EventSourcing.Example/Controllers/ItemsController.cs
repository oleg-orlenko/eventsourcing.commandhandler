using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Orlenko.EventSourcing.Example.Contracts.Abstractions;
using Orlenko.EventSourcing.Example.Contracts.Commands;
using Orlenko.EventSourcing.Example.Domain;
using Orlenko.EventSourcing.Example.ViewModels;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Orlenko.EventSourcing.Example.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Policy = "AllUsers")]
    public class ItemsController : ControllerBase
    {
        private readonly ICommandHandler commandHandler;

        public ItemsController(ICommandHandler commandHandler)
        {
            this.commandHandler = commandHandler;
        }

        [HttpGet("process/{id}", Name = "ProcessState")]
        public async Task<IActionResult> GetProcessStateAsync(Guid id, CancellationToken cancellationToken)
        {
            // TODO
            return this.Ok();
        }

        [HttpPost]
        public async Task<IActionResult> CreateItemAsync([FromBody]ItemCreateViewModel item, CancellationToken cancellationToken)
        {
            if (item == null)
            {
                return BadRequest();
            }

            var itemModel = new Item(Guid.NewGuid(), item.Name);
            var command = new CreateItemCommand(itemModel, User.Identity.Name);
            await commandHandler.HandleAsync(command, cancellationToken);

            return AcceptedAtAction("ProcessState", new { id = "TODO" });
        }

        [HttpPut("{itemId}")]
        public async Task<IActionResult> UpdateItemAsync([FromBody] ItemUpdateViewModel item, [FromRoute] Guid itemId)
        {
            if (item == null)
            {
                return BadRequest();
            }

            var itemModel = new Item(itemId, item.Name);
            var command = new UpdateItemCommand(itemModel, User.Identity.Name);
            await commandHandler.HandleAsync(command);
            return NoContent();
        }

        [HttpDelete("{itemId}")]
        public async Task<IActionResult> DeleteItemAsync(Guid itemId)
        {
            var command = new DeleteItemCommand(itemId, User.Identity.Name);
            await commandHandler.HandleAsync(command);
            return NoContent();
        }
    }
}