using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;

namespace LightStore.Controller
{
    ///<exclude/>
    public sealed class CredentialProvider : MembershipProvider
    {
        private readonly Type _serviceType;

        ///<exclude/>
        public CredentialProvider(Type serviceType)
        {
            _serviceType = serviceType;
        }

        ///<exclude/>
        public override string ApplicationName
        {
            get
            {
                throw new NotImplementedException();
            }

            set
            {
                throw new NotImplementedException();
            }
        }
        ///<exclude/>
        public override bool EnablePasswordReset
        {
            get
            {
                throw new NotImplementedException();
            }
        }
        ///<exclude/>
        public override bool EnablePasswordRetrieval
        {
            get
            {
                throw new NotImplementedException();
            }
        }
        ///<exclude/>
        public override int MaxInvalidPasswordAttempts
        {
            get
            {
                throw new NotImplementedException();
            }
        }
        ///<exclude/>
        public override int MinRequiredNonAlphanumericCharacters
        {
            get
            {
                throw new NotImplementedException();
            }
        }
        ///<exclude/>
        public override int MinRequiredPasswordLength
        {
            get
            {
                throw new NotImplementedException();
            }
        }
        ///<exclude/>
        public override int PasswordAttemptWindow
        {
            get
            {
                throw new NotImplementedException();
            }
        }
        ///<exclude/>
        public override MembershipPasswordFormat PasswordFormat
        {
            get
            {
                throw new NotImplementedException();
            }
        }
        ///<exclude/>
        public override string PasswordStrengthRegularExpression
        {
            get
            {
                throw new NotImplementedException();
            }
        }
        ///<exclude/>
        public override bool RequiresQuestionAndAnswer
        {
            get
            {
                throw new NotImplementedException();
            }
        }
        ///<exclude/>
        public override bool RequiresUniqueEmail
        {
            get
            {
                throw new NotImplementedException();
            }
        }
        ///<exclude/>
        public override bool ChangePassword(string username, string oldPassword, string newPassword)
        {
            throw new NotImplementedException();
        }
        ///<exclude/>
        public override bool ChangePasswordQuestionAndAnswer(string username, string password, string newPasswordQuestion, string newPasswordAnswer)
        {
            throw new NotImplementedException();
        }
        ///<exclude/>
        public override MembershipUser CreateUser(string username, string password, string email, string passwordQuestion, string passwordAnswer, bool isApproved, object providerUserKey, out MembershipCreateStatus status)
        {
            throw new NotImplementedException();
        }
        ///<exclude/>
        public override bool DeleteUser(string username, bool deleteAllRelatedData)
        {
            throw new NotImplementedException();
        }
        ///<exclude/>
        public override MembershipUserCollection FindUsersByEmail(string emailToMatch, int pageIndex, int pageSize, out int totalRecords)
        {
            throw new NotImplementedException();
        }
        ///<exclude/>
        public override MembershipUserCollection FindUsersByName(string usernameToMatch, int pageIndex, int pageSize, out int totalRecords)
        {
            throw new NotImplementedException();
        }
        ///<exclude/>
        public override MembershipUserCollection GetAllUsers(int pageIndex, int pageSize, out int totalRecords)
        {
            throw new NotImplementedException();
        }
        ///<exclude/>
        public override int GetNumberOfUsersOnline()
        {
            throw new NotImplementedException();
        }
        ///<exclude/>
        public override string GetPassword(string username, string answer)
        {
            throw new NotImplementedException();
        }
        ///<exclude/>
        public override MembershipUser GetUser(string username, bool userIsOnline)
        {
            throw new NotImplementedException();
        }
        ///<exclude/>
        public override MembershipUser GetUser(object providerUserKey, bool userIsOnline)
        {
            throw new NotImplementedException();
        }
        ///<exclude/>
        public override string GetUserNameByEmail(string email)
        {
            throw new NotImplementedException();
        }
        ///<exclude/>
        public override string ResetPassword(string username, string answer)
        {
            throw new NotImplementedException();
        }
        ///<exclude/>
        public override bool UnlockUser(string userName)
        {
            throw new NotImplementedException();
        }
        ///<exclude/>
        public override void UpdateUser(MembershipUser user)
        {
            throw new NotImplementedException();
        }
        ///<exclude/>
        public override bool ValidateUser(string username, string password)
        {
            return true;
        }
    }
}