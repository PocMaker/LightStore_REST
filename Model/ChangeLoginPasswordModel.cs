using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace LightStore.Model
{
    /// <summary>
    /// Change password for a specific login
    /// </summary>
    [DataContract]
    public sealed class ChangeLoginPasswordModel : LoginPasswordModel
    {
        private string _newPassword;

        /// <exclude/>
        [DataMember(IsRequired = true)]
        public string NewPassword
        {
            get { return _newPassword; }
            set { _newPassword = value; }
        }
    }
}