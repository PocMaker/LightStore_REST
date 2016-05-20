using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LightStore.Service
{
    ///
    public interface IServiceCrudT<T> where T : class
    {
        ///
        IList<T> Read();
        ///
        T ReadOne(string id);
        ///
        T CreateOne(T data);
        ///
        T UpdateOne(T data, string id);
        ///
        void DeleteOne(string id);
    }
}
