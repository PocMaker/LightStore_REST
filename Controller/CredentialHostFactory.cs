using BasicAuthenticationUsingWCF;
using Microsoft.ServiceModel.Web;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.ServiceModel.Web;

namespace LightStore.Controller
{
    /// <summary>
    /// Factory that provides instances of ServiceHost in managed hosting environments where the host instance is created dynamically in response to incoming messages.
    /// </summary>
    public sealed class CredentialHostFactory : ServiceHostFactory
    {
        /// <summary>
        /// Creates a ServiceHost for a specified type of service with a specific base address.
        /// </summary>
        /// <param name="serviceType">Specifies the type of service to host. </param>
        /// <param name="baseAddresses">The Array of type Uri that contains the base addresses for the service hosted.</param>
        /// <returns>A ServiceHost for the type of service specified with a specific base address.</returns>
        protected override ServiceHost CreateServiceHost(Type serviceType, Uri[] baseAddresses)
        {
            WebServiceHost2 serviceHost = new WebServiceHost2(serviceType, true, baseAddresses);
            serviceHost.Interceptors.Add(RequestInterceptorFactory.Create("DataWebService", new CredentialProvider(serviceType)));
            serviceHost.Description.Behaviors.Add(new ErrorHandlerServiceBehavior());
            return serviceHost;
        }
    }
}