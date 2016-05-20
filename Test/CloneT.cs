using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace LightStore.Test
{
    public class Tools
    {
        public static T Clone<T>(T obj)
        {
            DataContractSerializer dcSer = new DataContractSerializer(obj.GetType());
            MemoryStream memoryStream = new MemoryStream();

            dcSer.WriteObject(memoryStream, obj);
            memoryStream.Position = 0;

            T newObject = (T)dcSer.ReadObject(memoryStream);
            return newObject;
        }
    }
}
