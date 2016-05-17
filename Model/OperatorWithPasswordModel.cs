using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace LightStore.Model
{
    /// <summary>
    /// An existing operator with a clear password <seealso cref="OperatorModel"/>
    /// </summary>
    public sealed class OperatorWithPasswordModel : OperatorModel
    {
        /// <summary>
        /// A new clear password
        /// </summary>
        [DataMember]
        public string Password { internal get; set; }

        /// <summary>
        /// Describe an existing operator
        /// </summary>
        /// <param name="id">Unique ID in DB</param>
        /// <param name="login">Required unique login</param>
        /// <param name="firstName">Required firstname</param>
        /// <param name="lastName">Required lastname</param>
        public OperatorWithPasswordModel(int id, string login, string firstName, string lastName) : base(id, login, firstName, lastName) { }
    }
}