using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace LightStore.Model
{
    /// <summary>
    /// Minimum information for an operator
    /// </summary>
    [DataContract]
    public class BaseOperatorModel : IEquatable<BaseOperatorModel>
    {
        [DataMember(Name = "Login")]
        private readonly string _login;
        /// <summary>
        /// Unique login
        /// </summary>
        public string Login
        {
            get { return _login; }
        }

        /// <summary>
        /// Is operator password defined
        /// </summary>
        [DataMember]
        public bool IsPasswordDefined { get; set; }

        /// <summary>
        /// Create a new operator with his login
        /// </summary>
        /// <param name="login"></param>
        public BaseOperatorModel(string login)
        {
            if (String.IsNullOrWhiteSpace(login)) throw new ArgumentNullException("login", "Login cannot be empty");
            _login = login;
        }

        #region Equatable

        ///
        public override bool Equals(object obj)
        {
            return Equals((BaseOperatorModel)obj);
        }
        ///
        public bool Equals(BaseOperatorModel other)
        {
            return GetHashCode() == other.GetHashCode();
        }
        ///
        public override int GetHashCode()
        {
            return Login.GetHashCode();
        }

        #endregion
    }
}