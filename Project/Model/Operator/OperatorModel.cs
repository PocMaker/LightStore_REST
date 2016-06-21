using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace LightStore.Model
{
    /// <summary>
    /// Class describing an operator int the LightStore environnement
    /// </summary>
    [DataContract]
    public class OperatorModel : AIdEquatableT<OperatorModel>
    {
        private string _login;
        /// <summary>
        /// Unique login
        /// </summary>
        [DataMember]
        public string Login
        {
            get { return _login; }
            set
            {
                if (String.IsNullOrWhiteSpace(value)) throw new ArgumentNullException("login", "Login cannot be empty");
                _login = value;
            }
        }

        private string _firstName;
        /// <summary>
        /// First name (required)
        /// </summary>
        [DataMember(IsRequired = true)]
        [Required(ErrorMessage = "FirstName is required")]
        public string FirstName
        {
            get { return _firstName; }
            set
            {
                if (String.IsNullOrWhiteSpace(value)) throw new ArgumentNullException("FirstName", "FirstName cannot be empty");
                _firstName = value;
            }
        }


        private string _lastName;
        /// <summary>
        /// Last name (required)
        /// </summary>
        [DataMember(IsRequired = true)]
        [Required(ErrorMessage = "LastName is required")]
        public string LastName
        {
            get { return _lastName; }
            set
            {
                if (String.IsNullOrWhiteSpace(value)) throw new ArgumentNullException("LastName", "LastName cannot be empty");
                _lastName = value;
            }
        }

        /// <summary>
        /// Is operator password defined
        /// </summary>
        [DataMember]
        public bool IsPasswordDefined { get; set; }

        /// <summary>
        /// Main contact email (optional)
        /// </summary>
        [DataMember]
        [RegularExpression(@"[^\@]+\@[a-zA-Z0-9]+(\.[a-zA-Z0-9]+)+", ErrorMessage = "E-mail is invalid")]
        public string Email { get; set; }

        internal DateTime internalModifiedDate { get; set; }
        /// <summary>
        /// Last modified date in DB (format "yyyy-mm-ddThh:MM:ss")
        /// </summary>
        [DataMember]
        public string ModifiedDate
        {
            get { return internalModifiedDate.ToString("O", CultureInfo.InvariantCulture); }
            set { }
        }
    }
}