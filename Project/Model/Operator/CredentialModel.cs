using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace LightStore.Model
{
    /// <summary>
    /// Credential to log into LightStore sytem
    /// </summary>
    [DataContract]
    public sealed class CredentialModel
    {
        /// <summary>
        /// Login/Pseudo for authentication (required)
        /// </summary>
        [DataMember]
        public string Login { get; set; }

        /// <summary>
        /// Password for auuthentication
        /// </summary>
        [DataMember]
        public string Password { get; set; }

        /// <summary>
        /// New password if need to change it
        /// </summary>
        [DataMember]
        public string NewPassword { get; set; }
    }
}