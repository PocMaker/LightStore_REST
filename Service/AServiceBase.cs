using LightStore.Model;
using System;
using System.Diagnostics;
using System.ServiceModel;

namespace LightStore.Service
{
    /// <summary>
    /// Base class for LightStore services
    /// </summary>
    public abstract class AServiceBase
    {
        /// <summary>
        /// Wrap exception
        /// </summary>
        /// <param name="e">Exception to wrap</param>
        protected internal void ThrowFaultException(Exception e)
        {
            StackTrace st = new StackTrace();
            StackFrame sf = st.GetFrame(1);
            string method = sf.GetMethod().Name;

            JsonFaultModel jsonFault = new JsonFaultModel
            {
                Class = this.GetType().Name,
                Method = method,
                ErrorCode = e.GetType().Name,
                ErrorMessage = e.Message,
            };

            throw new FaultException<JsonFaultModel>(jsonFault);
        }
    }
}