using System;

namespace Orlenko.EventSourcing.Example.ViewModels
{
    /// <summary>
    /// This model represents shared vision across domain, of how the item shoul look like.
    /// </summary>
    /// TODO: Make it a shared class
    public class ItemViewModel
    {
        public Guid ItemId { get; set; }

        public string Name { get; set; }

        public DateTime Created { get; set; }

        public string CreatedBy { get; set; }

        public DateTime Updated { get; set; }

        public string UpdatedBy { get; set; }
    }
}
