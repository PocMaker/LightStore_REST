using System;
using System.Collections.Generic;
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
    public class OperatorModel : BaseOperatorModel
    {
        [DataMember(Name = "Id")]
        private readonly int _id;
        /// <summary>
        /// Unique id in DB (always strictly positive)
        /// </summary>
        public int Id
        {
            get { return _id; }
        }

        private string _firstName;
        /// <summary>
        /// First name (required)
        /// </summary>
        [DataMember(IsRequired = true)]
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
        /// Main contact email (optional)
        /// </summary>
        [DataMember]
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

        /// <summary>
        /// Create a new operator
        /// </summary>
        /// <param name="login">Required unique login</param>
        /// <param name="firstName">Required firstname</param>
        /// <param name="lastName">Required lastname</param>
        public OperatorModel(string login, string firstName, string lastName) : base(login)
        {
            FirstName = firstName;
            LastName = lastName;
        }
        /// <summary>
        /// Describe an existing operator
        /// </summary>
        /// <param name="id">Unique ID in DB</param>
        /// <param name="login">Required unique login</param>
        /// <param name="firstName">Required firstname</param>
        /// <param name="lastName">Required lastname</param>
        public OperatorModel(int id, string login, string firstName, string lastName) : this(login, firstName, lastName)
        {
            if (id < 0) throw new ArgumentOutOfRangeException("Id", "Id cannot be negative");
            _id = id;
        }
    }
}