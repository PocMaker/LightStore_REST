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
        /// <exclude/>
        [DataMember(IsRequired =true)]
        public string Login { get; set; }

        /// <exclude/>
        [DataMember]
        public string Password { get; set; }

        /// <exclude/>
        [DataMember]
        public string NewPassword { get; set; }
    }
}