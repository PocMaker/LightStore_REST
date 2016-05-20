using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace LightStore.Model
{
    /// <summary>
    /// Class to know if a password is defined for a specific ID
    /// </summary>
    [DataContract]
    public sealed class IdIsPasswordDefinedModel
    {
        /// <summary>
        /// Operator Id
        /// </summary>
        [DataMember]
        public int Id { get; set; }

        /// <summary>
        /// Is password defined for Id
        /// </summary>
        [DataMember]
        public bool IsPasswordDefined { get; set; }
    }
}