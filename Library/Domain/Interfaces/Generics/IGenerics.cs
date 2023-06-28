using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interfaces.Generics
{
    public interface IGenerics<X> where X : class
    {
        Task Add(X Objeto);
        Task Update(X Objeto);
        Task Delete(X Objeto);
        Task<X> GetEntityById(int Id);
        Task<List<X>> List();

    }
}
