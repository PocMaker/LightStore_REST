using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.ServiceModel.Dispatcher;
using System.ServiceModel.Web;
using System.Web;

namespace LightStore.Controller
{
    class CustomErrorHandler : IErrorHandler
    {
        public bool HandleError(Exception error)
        {
            // Do some logging, system notifications, shutting down the application or whatever...
            // Returning true indicates you performed your behavior
            return true;
        }

        public void ProvideFault(Exception e, System.ServiceModel.Channels.MessageVersion version, ref System.ServiceModel.Channels.Message fault)
        {
            Console.WriteLine("Inside ProvideFault: Converting Exception status code to Forbidden for InvalidOperationException..");

            WebOperationContext.Current.OutgoingResponse.StatusCode = HttpStatusCode.Forbidden;
            WebOperationContext.Current.OutgoingResponse.StatusDescription = e.Message;
        }
    }
}