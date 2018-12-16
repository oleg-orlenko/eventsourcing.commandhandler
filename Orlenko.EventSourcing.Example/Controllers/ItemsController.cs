using Microsoft.AspNetCore.Mvc;
using Orlenko.EventSourcing.Example.Contracts.Abstractions;
using Orlenko.EventSourcing.Example.Contracts.Commands;
using Orlenko.EventSourcing.Example.Contracts.Models;
using System.Threading.Tasks;

namespace Orlenko.EventSourcing.Example.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ItemsController : ControllerBase
    {
        private readonly ICommandHandler commandHandler;

        public ItemsController(ICommandHandler commandHandler)
        {
            this.commandHandler = commandHandler;
        }

        [HttpPost]
        public async Task<IActionResult> CreateItemAsync([FromBody]ItemModel item)
        {
            var command = new CreateItemCommand(item, "");
            await this.commandHandler.HandleAsync(command);
            return this.Created("");
        }
    }
}