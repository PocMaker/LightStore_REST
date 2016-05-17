using LightStore.Dal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

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
        /// <param name="login">Current login to watch</param>
        /// <returns>True if password should be defined on login</returns>
        [OperationContract]
        bool ShouldDefinePassword(string login);
    }

    /// <summary>
    /// Available actions on login time
    /// </summary>
    public class Login : ILogin
    {
        private readonly IOperatorDal _operatorDal;

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
        }

        /// <summary>
        /// Define if current login have to set its password
        /// </summary>
        /// <param name="login">Current login to watch</param>
        /// <returns>True if password should be defined on login</returns>
        public bool ShouldDefinePassword(string login)
        {
            
            return true;
        }
    }
}
