using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Orlenko.EventSourcing.Example.Contracts.Abstractions;
using Orlenko.EventSourcing.Example.Contracts.Commands;
using Orlenko.EventSourcing.Example.Contracts.Models;
using Orlenko.EventSourcing.Example.ViewModels;
using System;
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

        [HttpPost]
        public async Task<IActionResult> CreateItemAsync([FromBody]ItemCreateViewModel item)
        {
            if (item == null)
            {
                return BadRequest();
            }

            var itemModel = new ItemModel(Guid.NewGuid(), item.Name);
            var command = new CreateItemCommand(itemModel, User.Identity.Name);
            var evt = await commandHandler.HandleAsync(command);
            var getViewModel = new ItemViewModel
            {
                ItemId = itemModel.Id,
                Name = itemModel.Name,
                Created = evt.EventDate,
                Updated = evt.EventDate,
                CreatedBy = evt.UserName,
                UpdatedBy = evt.UserName
            };
            // Can't return Created with Location, because this service does not now where this item can be accessed :( 
            return Ok(getViewModel);
        }

        [HttpPut("{itemId}")]
        public async Task<IActionResult> UpdateItemAsync([FromBody] ItemUpdateViewModel item, [FromRoute] Guid itemId)
        {
            if (item == null)
            {
                return BadRequest();
            }

            var itemModel = new ItemModel(itemId, item.Name);
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