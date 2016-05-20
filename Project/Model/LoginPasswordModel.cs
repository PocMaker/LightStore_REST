using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace LightStore.Model
{
    /// <summary>
    /// Credential class
    /// </summary>
    [DataContract]
    public class LoginPasswordModel
    {
        private string _login;

        /// <exclude/>
        [DataMember(IsRequired = true)]
        public string Login
        {
            get { return _login; }
            set { _login = value; }
        }


        /// <exclude/>
        [DataMember]
        public string Password { get; set; }
    }
}