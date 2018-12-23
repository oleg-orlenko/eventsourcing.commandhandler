using System;

namespace Orlenko.EventSourcing.Example.Contracts.Commands
{
    public abstract class BaseItemCommand
    {
        public readonly string UserName;

        public BaseItemCommand(string userName)
        {
            if (String.IsNullOrEmpty(userName))
            {
                throw new ArgumentNullException(nameof(userName));
            }

            UserName = userName;
        }
    }
}
