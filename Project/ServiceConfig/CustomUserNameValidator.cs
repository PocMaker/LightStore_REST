using LightStore.Dal;
using LightStore.Model;
using System;
using System.Collections.Generic;

namespace LightStore.ServiceConfig
{
    internal class CustomUserNameValidator
    {
        private static IDictionary<Credential, OperatorWithDateModel> _logins = new System.Collections.Concurrent.ConcurrentDictionary<Credential, OperatorWithDateModel>();

        class OperatorWithDateModel
        {
            public OperatorModel Operator { get; set; }
            public DateTime Date { get; set; }
        }

        class Credential : IEquatable<Credential>
        {
            public string UserName { get; set; }
            public string Password { get; set; }

            public override bool Equals(object obj)
            {
                return Equals(obj as Credential);
            }
            public bool Equals(Credential other)
            {
                if (other == null) return false;
                return GetHashCode() == other.GetHashCode();
            }
            public override int GetHashCode()
            {
                int hash = UserName.GetHashCode();
                hash ^= Password.GetHashCode();
                return hash;
            }
        }

        private IOperatorDal _operatorDal;

        public CustomUserNameValidator() : this(new OperatorDal()) { }
        public CustomUserNameValidator(IOperatorDal operatorDal)
        {
            _operatorDal = operatorDal;
        }

        public static bool Validate(string userName, string password)
        {
            CustomUserNameValidator c = new CustomUserNameValidator();
            OperatorModel ope =c.ValidateCredential(userName,password);
            return (ope != null);
        }


        public OperatorModel ValidateCredential(string userName, string password)
        {
            return ValidateCredential(userName, password, false);
        }
        // This method validates users. It allows in two users, user1 and user2
        // This code is for illustration purposes only and
        // must not be used in a production environment because it is not secure.
        public OperatorModel ValidateCredential(string userName, string password, bool force)
        {
            Credential credential = new Credential { UserName = userName, Password = password };
            if (!force && AlreadyLoggedIn(credential)) return _logins[credential].Operator;

            OperatorModel ope = _operatorDal.LogIn(credential.UserName, credential.Password);
            if (ope == null) return null;

            _logins[credential] = new OperatorWithDateModel { Operator = ope, Date = DateTime.Now };

            return ope;
        }

        private bool AlreadyLoggedIn(Credential credential)
        {
            if (!_logins.ContainsKey(credential)) return false;

            if ((DateTime.Now - _logins[credential].Date).TotalMinutes <= 20)
            {
                _logins[credential].Date = DateTime.Now;
                return true;
            }

            _logins.Remove(credential);
            return false;
        }

    }
}