using LightStore.Dal;
using LightStore.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace LightStore.Service
{
    /// <summary>
    /// Base class interface to manage operators
    /// </summary>
    [ServiceContract]
    public interface IOperator : IServiceCrudT<OperatorModel>
    {
        /// <summary>
        /// Retrieve all operators
        /// </summary>
        /// <returns>Operators data</returns>
        [OperationContract]
        [WebInvoke(UriTemplate = "Operators/", Method = "GET", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        new IList<OperatorModel> Read();

        /// <summary>
        /// Retrieve a specific operator
        /// </summary>
        /// <param name="id">Opertor ID</param>
        /// <returns>Operator data</returns>
        [OperationContract]
        [WebInvoke(UriTemplate = "Operators/{id}/", Method = "GET", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        new OperatorModel ReadOne(string id);

        /// <summary>
        /// Create a new operator
        /// </summary>
        /// <param name="data">Operator to create</param>
        /// <returns>Operator data after creation</returns>
        [OperationContract]
        [WebInvoke(UriTemplate = "Operators/", Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        new OperatorModel CreateOne(OperatorModel data);

        /// <summary>
        /// Update an existing operator
        /// </summary>
        /// <param name="data">Operator data to update</param>
        /// <param name="id">Operator ID</param>
        /// <returns>Operator data after update</returns>
        [OperationContract]
        [WebInvoke(UriTemplate = "Operators/{id}/", Method = "PUT", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        new OperatorModel UpdateOne(OperatorModel data, string id);

        /// <summary>
        /// Delete operator with the specified id
        /// </summary>
        /// <param name="id">Operator ID to delete</param>
        [OperationContract]
        [WebInvoke(UriTemplate = "Operators/{id}/", Method = "DELETE", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        new void DeleteOne(string id);
    }

    /// <summary>
    /// Class to manage operators
    /// </summary>
    public sealed class Operator : AServiceBase, IOperator
    {
        private readonly IOperatorDal _operatorDal;

        /// <summary>
        /// Interact with DB
        /// </summary>
        public Operator() : this(new OperatorDal()) { }
        /// <summary>
        /// Interact with a mock (unit tests only)
        /// </summary>
        /// <exception cref="ArgumentNullException">Thrown if mocked Dal is null</exception>
        /// <param name="operatorDal">Db mock for unit test (required)</param>
        public Operator(IOperatorDal operatorDal)
        {
            if (operatorDal == null) throw new ArgumentNullException("operatorDal", "Mocked Dal cannot be null");
            _operatorDal = operatorDal;
        }

        #region IOperator

        /// <summary>
        /// Retrieve all operators
        /// </summary>
        /// <returns>Operators data</returns>
        public IList<OperatorModel> Read()
        {
            return _operatorDal.Select();
        }

        /// <summary>
        /// Retrieve a specific operator
        /// </summary>
        /// <param name="id">Opertor ID</param>
        /// <exception cref="FormatException">Thrown when <paramref name="id"/> is not a strictly positive numeric data</exception>
        /// <returns>Operator data</returns>
        public OperatorModel ReadOne(string id)
        {
            int opeID;

            if (!int.TryParse(id, out opeID)) throw new FormatException("id is not a numeric value");
            if (opeID <= 0) throw new FormatException("id have to be a positive value");

            return _operatorDal.Select(opeID);
        }

        /// <summary>
        /// Create a new operator
        /// </summary>
        /// <param name="data">Operator to create</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="data"/> is null</exception>
        /// <returns>Operator data after creation</returns>
        public OperatorModel CreateOne(OperatorModel data)
        {
            if (data == null) throw new ArgumentNullException("data", "Operator to create cannot be null");

            return _operatorDal.Insert(data);
        }

        /// <summary>
        /// Update an existing operator
        /// </summary>
        /// <param name="data">Operator data to update</param>
        /// <param name="id">Operator ID</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="data"/> is null</exception>
        /// <exception cref="ArgumentException">Thrown if <paramref name="id"/> is not equal to <paramref name="data"/> ID</exception>
        /// <returns>Operator data after update</returns>
        public OperatorModel UpdateOne(OperatorModel data, string id)
        {
            if (data == null) throw new ArgumentNullException("data", "Operator to create cannot be null");
            if (data.Id.ToString() != id) throw new ArgumentException("Operator id and parameter id doesn't match", "id");

            return _operatorDal.Update(data);
        }

        /// <summary>
        /// Delete operator with the specified id
        /// </summary>
        /// <param name="id">Operator ID to delete</param>
        /// <exception cref="FormatException">Thrown when <paramref name="id"/> is not a strictly positive numeric data</exception>
        public void DeleteOne(string id)
        {
            int opeID;

            if (!int.TryParse(id, out opeID)) throw new FormatException("id is not a numeric value");
            if (opeID <= 0) throw new FormatException("id have to be a positive value");

            _operatorDal.Delete(opeID);
        }

        #endregion
    }
}
