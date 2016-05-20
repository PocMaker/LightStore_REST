using LightStore.Model;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Dispatcher;
using System.ServiceModel.Web;
using System.Web;
using System.Web.Services.Protocols;

namespace LightStore.ServiceConfig
{
    internal class JsonErrorHandler : IErrorHandler
    {
        #region Public Method(s)

        #region IErrorHandler Members
        ///
        /// Is the error always handled in this class?
        ///
        public bool HandleError(Exception error)
        {
            return true;
        }

        ///
        /// Provide the Json fault message
        ///
        public void ProvideFault(Exception error, MessageVersion version, ref Message fault)
        {
            HttpStatusCode statusCode = HttpStatusCode.BadRequest;
            JsonFaultModel jsonFault = null;

            if (error is FaultException<JsonFaultModel>) jsonFault = ((FaultException<JsonFaultModel>)error).Detail;
            else
            {
                jsonFault = new JsonFaultModel
                {
                    ErrorCode = error.GetType().Name,
                    ErrorMessage = error.Message,
                };
            }
            jsonFault.StatusCode = (int)statusCode;

            fault = Message.CreateMessage(version, null, jsonFault, new DataContractJsonSerializer(typeof(JsonFaultModel)));

            fault.Properties.Add(WebBodyFormatMessageProperty.Name, new WebBodyFormatMessageProperty(WebContentFormat.Json));
            var rmp = new HttpResponseMessageProperty
            {
                StatusCode = statusCode,
                StatusDescription = "Bad Request",
            };
            rmp.Headers[HttpResponseHeader.ContentType] = "application/json";
            fault.Properties.Add(HttpResponseMessageProperty.Name, rmp);
        }
        #endregion

        #endregion

    }
}