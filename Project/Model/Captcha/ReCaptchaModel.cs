using RestSharp.Deserializers;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace LightStore.Model
{
    /// <summary>
    /// Response from Google to the captcha step
    /// </summary>
    [DataContract]
    public class ReCaptchaModel
    {
        /// <summary>
        /// Is user's captcha entry correct ?
        /// </summary>
        [DataMember(Name = "success")]
        [DeserializeAs(Name = "success")]
        public bool Success { get; set; }

        /// <summary>
        /// Response time from google
        /// </summary>
        [DataMember(Name = "challenge_ts")]
        [DeserializeAs(Name = "challenge_ts")]
        public DateTime ChallengeTS { get; set; }

        /// <summary>
        /// Hostname from where the captcha was typed
        /// </summary>
        [DataMember(Name = "hostname")]
        [DeserializeAs(Name = "hostname")]
        public string HostName { get; set; }

        /// <summary>
        /// Optional error codes if request is not well formated
        /// </summary>
        [DataMember(Name = "error-codes")]
        [DeserializeAs(Name = "error-codes")]
        public List<string> ErrorCodes { get; set; }

        ///<exclude/>
        public ReCaptchaModel()
        {
            ChallengeTS = DateTime.Now;
        }
    }
}