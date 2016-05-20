using LightStore.Dal;
using LightStore.Model;
using LightStore.ServiceConfig;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.Serialization;
using System.Security.Authentication;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

#if DEBUG
using WebOperationContext = System.ServiceModel.Web.MockedWebOperationContext;
#endif

namespace LightStore.Service
{
    /// <summary>
    /// Available actions on login time
    /// </summary>
    [ServiceContract]
    public interface ILogin
    {
        /// <summary>
        /// Define if current login have to set its password
        /// </summary>
        /// <param name="credential">Current login to watch</param>
        /// <returns>Id and password definition info for login</returns>
        [OperationContract]
        [WebInvoke(UriTemplate = "Login/info/", Method = "PUT", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        IdIsPasswordDefinedModel IsPasswordDefined(CredentialModel credential);

        /// <summary>
        /// Log into LightStore system
        /// </summary>
        /// <param name="credential">Login and password informations</param>
        /// <returns></returns>
        [OperationContract]
        [WebInvoke(UriTemplate = "Login/log/", Method = "PUT", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        OperatorModel LogIn(CredentialModel credential);
    }

    /// <summary>
    /// Available actions on login time
    /// </summary>
    public class Login : ILogin
    {
        private readonly IOperatorDal _operatorDal;
        private readonly CustomUserNameValidator _validator;

        /// <summary>
        /// Interact with DB
        /// </summary>
        public Login() : this(new OperatorDal()) { }
        /// <summary>
        /// Interact with a mock (unit tests only)
        /// </summary>
        /// <exception cref="ArgumentNullException">Thrown if mocked Dal is null</exception>
        /// <param name="operatorDal">Db mock for unit test (required)</param>
        public Login(IOperatorDal operatorDal)
        {
            if (operatorDal == null) throw new ArgumentNullException("operatorDal", "Mocked Dal cannot be null");
            _operatorDal = operatorDal;
            _validator = new CustomUserNameValidator(_operatorDal);
        }

        /// <summary>
        /// Define if current login have to set its password
        /// </summary>
        /// <param name="credential">Current login to watch</param>
        /// <exception cref="InvalidCredentialException">Thrown when login does not exist</exception>
        /// <returns>True if password should be defined on login</returns>
        public IdIsPasswordDefinedModel IsPasswordDefined(CredentialModel credential)
        {
            OperatorModel ope = _operatorDal.Select(credential.Login);
            if (ope == null) throw new InvalidCredentialException();

            IdIsPasswordDefinedModel result = new IdIsPasswordDefinedModel { Id = ope.Id, IsPasswordDefined = ope.IsPasswordDefined };
            return result;
        }

        /// <summary>
        /// Log into LightStore system
        /// </summary>
        /// <param name="credential">Login and password informations</param>
        /// <exception cref="SqlException">Thrown when login or password is wrong</exception>
        /// <returns></returns>
        public OperatorModel LogIn(CredentialModel credential)
        {
            OperatorModel ope = _validator.ValidateCredential(credential.Login, credential.Password);
            return ope;
        }
    }
}
