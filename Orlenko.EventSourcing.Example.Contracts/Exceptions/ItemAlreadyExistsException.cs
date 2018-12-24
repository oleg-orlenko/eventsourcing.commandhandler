using System;

namespace Orlenko.EventSourcing.Example.Contracts.Exceptions
{
    public class ItemAlreadyExistsException : Exception
    {
        public ItemAlreadyExistsException(string message) : base(message)
        {
        }
    }
}
