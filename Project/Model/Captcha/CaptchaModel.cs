using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace LightStore.Model
{
    /// <summary>
    /// Class to collect captcha
    /// </summary>
    [DataContract]
    public sealed class CaptchaModel
    {
        /// <summary>
        /// User response for the captcha
        /// </summary>
        [DataMember(Name = "response")]
        public string Response { get; set; }
        /// <summary>
        /// Public key for google authentication
        /// </summary>
        [DataMember(Name = "key")]
        public string Key { get; set; }
    }
}