using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HAC.Domain
{
    interface IRepository<T>: IEnumerable<T>
    where T : Entity
    {
        void Add(T item);
        bool Contains(T item);
        int Count { get; }
    }
}
