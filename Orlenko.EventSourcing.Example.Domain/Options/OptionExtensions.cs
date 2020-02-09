using System.Collections.Generic;
using System.Linq;

namespace Orlenko.EventSourcing.Example.Domain.Options
{
    public static class OptionExtensions
    {
        public static IEnumerable<T> Merge<T>(this IEnumerable<IOption<T>> optionsList) where T : class
        {
            var availableItems = optionsList
                .Where(x => x.IsSome)
                .Select(x => x.Match((T)null, y => y))
                .ToArray();
            return availableItems;
        }
    }
}
