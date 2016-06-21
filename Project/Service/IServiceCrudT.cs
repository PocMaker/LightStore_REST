using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LightStore.Service
{
    ///<exclude/>
    public interface IServiceCrudT<T> where T : class, new()
    {
        ///<exclude/>
        IList<T> Read();
        ///<exclude/>
        T ReadOne(string id);
        ///<exclude/>
        T CreateOne(T data);
        ///<exclude/>
        T UpdateOne(T data, string id);
        ///<exclude/>
        void DeleteOne(string id);
    }
}
