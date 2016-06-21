using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace LightStore.Model
{
    /// <summary>
    /// Default object to accept a single string formatted as a json object
    /// </summary>
    [DataContract]
    public sealed class DefaultJsonString
    {
        /// <summary>
        /// Object json key
        /// </summary>
        [DataMember(Name = "key")]
        public string Key { get; set; }

        /// <summary>
        /// Object json value
        /// </summary>
        [DataMember(Name = "value")]
        public string Value { get; set; }
    }
}