using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace LightStore.Model
{
    /// <summary>
    /// Model to describe exception for client process
    /// </summary>
    [DataContract]
    public sealed class JsonFaultModel
    {
        /// <summary>
        /// Class where error occured
        /// </summary>
        [DataMember]
        public string Class { get; set; }

        /// <summary>
        /// Method where error occured
        /// </summary>
        [DataMember]
        public string Method { get; set; }

        /// <summary>
        /// Internal error code (Exception type by default)
        /// </summary>
        [DataMember]
        public string ErrorCode { get; set; }

        /// <summary>
        /// Internal error message (Exception message by default)
        /// </summary>
        [DataMember]
        public string ErrorMessage { get; set; }

        /// <summary>
        /// HttpError status code
        /// </summary>
        [DataMember]
        public int StatusCode { get; set; }
    }
}