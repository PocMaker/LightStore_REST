using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LightStore.Dal
{
    ///
    public interface ICrudDalT<T> where T : class
    {
        ///
        IList<T> Select();
        ///
        T Select(int id);
        ///
        T Insert(T item);
        ///
        T Update(T item);
        ///
        bool Delete(int id);
    }
}
