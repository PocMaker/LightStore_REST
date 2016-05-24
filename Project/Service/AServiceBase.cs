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
        /// Wrap exception in an SOAP well formed exception
        /// </summary>
        /// <param name="e">Exception to wrap</param>
        protected internal void ThrowFaultException(Exception e)
        {
            this.ThrowFaultException(e.GetType().Name, e.Message);
        }

        /// <summary>
        /// Wrap exception in an SOAP well formed exception
        /// </summary>
        /// <param name="errorCode">Internal error code</param>
        /// <param name="errorMessage">Internal error description</param>
        protected internal void ThrowFaultException(string errorCode, string errorMessage)
        {
            StackTrace st = new StackTrace();
            StackFrame sf = st.GetFrame(1);
            string method = sf.GetMethod().Name;

            JsonFaultModel jsonFault = new JsonFaultModel
            {
                Class = this.GetType().Name,
                Method = method,
                ErrorCode = errorCode,
                ErrorMessage = errorMessage,
            };
            this.ThrowFaultException(jsonFault);
        }
        /// <summary>
        /// Wrap exception in an SOAP well formed exception
        /// </summary>
        /// <param name="jsonFault">Object describing logic exception</param>
        protected internal void ThrowFaultException(JsonFaultModel jsonFault)
        {
            throw new FaultException<JsonFaultModel>(jsonFault);
        }

    }
}