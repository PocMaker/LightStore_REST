using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Principal;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;
using System.ServiceModel.Dispatcher;
using System.ServiceModel.Web;
using System.Text;
using System.Threading;
using System.Web;
using System.Web.Security;

namespace LightStore.ServiceConfig
{
    /// <summary>
    /// Mark a REST method as "Basic authentication required" type
    /// </summary>
    internal class BasicAuthenticationInvoker : Attribute, IOperationBehavior, IOperationInvoker
    {
        #region Private Fields

        private IOperationInvoker _invoker;

        #endregion Private Fields

        #region IOperationBehavior Members

        public void ApplyDispatchBehavior(OperationDescription operationDescription, DispatchOperation dispatchOperation)
        {
            _invoker = dispatchOperation.Invoker;
            dispatchOperation.Invoker = this;
        }

        public void ApplyClientBehavior(OperationDescription operationDescription, ClientOperation clientOperation)
        {
        }

        public void AddBindingParameters(OperationDescription operationDescription, BindingParameterCollection bindingParameters)
        {
        }

        public void Validate(OperationDescription operationDescription)
        {
        }

        #endregion IOperationBehavior Members

        #region IOperationInvoker Members

        public object Invoke(object instance, object[] inputs, out object[] outputs)
        {
            //System.Diagnostics.Debugger.Break();
            if (Authenticate()) return _invoker.Invoke(instance, inputs, out outputs);
            else
            {
                outputs = null;
                return null;
            }
        }

        public object[] AllocateInputs()
        {
            return _invoker.AllocateInputs();
        }

        public IAsyncResult InvokeBegin(object instance, object[] inputs, AsyncCallback callback, object state)
        {
            throw new NotSupportedException();
        }

        public object InvokeEnd(object instance, out object[] outputs, IAsyncResult result)
        {
            throw new NotSupportedException();
        }

        public bool IsSynchronous
        {
            get { return true; }
        }

        #endregion IOperationInvoker Members

        private bool Authenticate()
        {
            string[] credentials = GetCredentials(WebOperationContext.Current.IncomingRequest.Headers);

            if (credentials != null && credentials.Length == 2)
            {
                var username = credentials[0];
                var password = credentials[1];
                if (CustomUserNameValidator.Validate(username, password)) return true;
            }

            WebOperationContext.Current.OutgoingResponse.Headers["WWW-Authenticate"] = string.Format("Basic realm=\"{0}\"", string.Empty);
            WebOperationContext.Current.OutgoingResponse.StatusCode = HttpStatusCode.Unauthorized;
            return false;
        }

        private string[] GetCredentials(WebHeaderCollection headers)
        {
            string credentials = WebOperationContext.Current.IncomingRequest.Headers["Authorization"];
            if (credentials != null) credentials = credentials.Trim();

            if (!string.IsNullOrWhiteSpace(credentials))
            {
                string[] credentialParts = credentials.Split(new[] { ' ' });
                if (credentialParts.Length == 2 && credentialParts[0].Equals("basic", StringComparison.OrdinalIgnoreCase))
                {
                    credentials = Encoding.ASCII.GetString(Convert.FromBase64String(credentialParts[1]));
                    credentialParts = credentials.Split(new[] { ':' });
                    if (credentialParts.Length == 2) return credentialParts;
                }
            }

            return null;
        }
    }
}
