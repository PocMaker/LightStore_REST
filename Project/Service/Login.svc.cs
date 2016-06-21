using LightStore.Dal;
using LightStore.Model;
using LightStore.ServiceConfig;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
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
        [WebInvoke(UriTemplate = "login/info", Method = "PUT", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        IdIsPasswordDefinedModel IsPasswordDefined(CredentialModel credential);

        /// <summary>
        /// Log into LightStore system
        /// </summary>b
        /// <param name="credential">Login and password informations</param>
        /// <returns></returns>
        [OperationContract]
        [WebInvoke(UriTemplate = "login/log", Method = "PUT", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        OperatorModel LogIn(CredentialModel credential);

        /// <summary>
        /// Check if user's captcha response is correct
        /// </summary>
        /// <param name="captcha">User response and key</param>
        [OperationContract]
        [WebInvoke(UriTemplate = "login/captcha", Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        ReCaptchaModel Captcha(CaptchaModel captcha);
    }

    /// <summary>
    /// Available actions on login time
    /// </summary>
    public class Login : AServiceBase, ILogin
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
            if (ope == null) this.ThrowFaultException("INVALID_CREDENTIAL", "Login and password do not match");

            IdIsPasswordDefinedModel result = new IdIsPasswordDefinedModel { Id = ope.Id, IsPasswordDefined = ope.IsPasswordDefined };
            return result;
        }

        /// <summary> comm
        /// Log into LightStore system
        /// </summary>
        /// <param name="credential">Login and password informations</param>
        /// <exception cref="SqlException">Thrown when login or password is wrong</exception>
        /// <returns></returns>
        public OperatorModel LogIn(CredentialModel credential)
        {
            try
            {
                OperatorModel ope = _validator.ValidateCredential(credential.Login, credential.Password, true);
                if (ope == null) this.ThrowFaultException("INVALID_CREDENTIAL", "Login and password do not match");
                return ope;
            }
            catch (SqlException e)
            {
                if (e.Class == 16 && e.State == 1) this.ThrowFaultException("INVALID_CREDENTIAL", "Login and password do not match");
                throw;
            }
        }

        /// <summary>
        /// Check if user's captcha response is correct
        /// </summary>
        /// <param name="captcha">User response and key</param>
        public ReCaptchaModel Captcha(CaptchaModel captcha)
        {
            string privateKey = @"6LefWSETAAAAAMe5KrsGby-Gl3mulJgndh5Razec";
            if (captcha.Key == @"6LeIxAcTAAAAAJcZVRqyHh71UMIEGNQ_MXjiZKhI") return new ReCaptchaModel { Success = true };

            NameValueCollection section = (NameValueCollection)ConfigurationManager.GetSection("CaptchaKeys");
            if (section.AllKeys.Contains(captcha.Key)) privateKey = section[captcha.Key];

            var request = new RestRequest("/recaptcha/api/siteverify", Method.POST);
            request.AddQueryParameter("secret", privateKey);
            request.AddQueryParameter("response", captcha.Response);

            var client = new RestClient("https://www.google.com");
            //client.Proxy = new WebProxy(@"proxy.tesson.intra", 8080);
            var response = client.Execute<ReCaptchaModel>(request);

            if (response.ErrorException != null) this.ThrowFaultException("ERR", response.ErrorException.StackTrace);
            
            return response.Data;
        }
    }
}
