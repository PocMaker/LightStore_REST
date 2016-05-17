using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using LightStore.Model;
using System.Data.SqlClient;
using System.Data;
using System.Globalization;

namespace LightStore.Dal
{
    /// <summary>
    /// List of actions available to manage operators
    /// </summary>
    public interface IOperatorDal : ICrudDalT<OperatorModel>
    {
        /// <summary>
        /// Select a specific operator
        /// </summary>
        /// <param name="login">Login of wanted operator</param>
        /// <returns>An OperatorModel describing the required operator</returns>
        OperatorModel Select(string login);
    }

    /// <summary>
    /// Interface with DB to manage Operators
    /// </summary>
    public sealed class OperatorDal : ABaseDal, IOperatorDal
    {
        /// <summary>
        /// DB is describing in the "main" node of ConnectionString in the config file
        /// </summary>
        public OperatorDal() : base() { }
        /// <summary>
        /// DB is describing in the <paramref name="connectionString"/> argument (unit tests only)
        /// </summary>
        /// <param name="connectionString">A way to connect to a specific DB</param>
        public OperatorDal(string connectionString) : base(connectionString) { }

        #region IOperatorDal

        /// <summary>
        /// Select all operators
        /// </summary>
        /// <returns>A list of OperatorModel describing each operator</returns>
        public IList<OperatorModel> Select()
        {
            return SelectOperator(null, null);
        }

        /// <summary>
        /// Select a specific operator
        /// </summary>
        /// <param name="id">Id of wanted operator</param>
        /// <returns>An OperatorModel describing the required operator</returns>
        public OperatorModel Select(int id)
        {
            var operators = SelectOperator(id);
            return operators.First();
        }

        /// <summary>
        /// Select a specific operator
        /// </summary>
        /// <param name="login">Login of wanted operator</param>
        /// <returns>An OperatorModel describing the required operator</returns>
        public OperatorModel Select(string login)
        {
            var operators = SelectOperator(login);
            return operators.FirstOrDefault();
        }

        /// <summary>
        /// Create a new operator from given <paramref name="item"/>
        /// </summary>
        /// <param name="item">A new operator</param>
        /// <exception cref="ArgumentNullException">Thrown when input <paramref name="item"/> is null</exception>
        /// <returns>An OperatorModel describing the created operator</returns>
        public OperatorModel Insert(OperatorModel item)
        {
            if (item == null) throw new ArgumentNullException("item", "Operator to create cannot be null");

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                using (SqlCommand command = new SqlCommand())
                {
                    command.Connection = connection;
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "Administration.uspCreateOperator";
                    command.Parameters.Add("@p_firstName", SqlDbType.VarChar).Value = item.FirstName;
                    command.Parameters.Add("@p_lastName", SqlDbType.VarChar).Value = item.LastName;
                    command.Parameters.Add("@p_login", SqlDbType.VarChar).Value = item.Login;
                    command.Parameters.Add("@p_email", SqlDbType.VarChar).Value = item.Email;

                    var operators = this.ConvertDataTableToOperators(command);
                    return operators.First();
                }
            }
        }

        /// <summary>
        /// Update an exisiting operator from given <paramref name="item"/>
        /// </summary>
        /// <param name="item">An existing operator with filled properties included password</param>
        /// <exception cref="ArgumentNullException">Thrown when input <paramref name="item"/> is null</exception>
        /// <returns>An OperatorModel describing the updated operator</returns>
        public OperatorModel Update(OperatorModel item)
        {
            if (item == null) throw new ArgumentNullException("item", "Operator to update cannot be null");

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                using (SqlCommand command = new SqlCommand())
                {
                    command.Connection = connection;
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "Administration.uspUpdateOperator";
                    command.Parameters.Add("@p_id", SqlDbType.Int).Value = item.Id;
                    command.Parameters.Add("@p_firstName", SqlDbType.VarChar).Value = item.FirstName;
                    command.Parameters.Add("@p_lastName", SqlDbType.VarChar).Value = item.LastName;
                    command.Parameters.Add("@p_email", SqlDbType.VarChar).Value = item.Email;
                    if (item is OperatorWithPasswordModel) command.Parameters.Add("@p_password", SqlDbType.VarChar).Value = ((OperatorWithPasswordModel)item).Password;

                    var operators = this.ConvertDataTableToOperators(command);
                    return operators.First();
                }
            }
        }

        /// <summary>
        /// Delete a specific operator
        /// </summary>
        /// <param name="id">Operator id to delete</param>
        /// <returns>True in case of success</returns>
        public bool Delete(int id)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                using (SqlCommand command = new SqlCommand())
                {
                    command.Connection = connection;
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "Administration.uspDeleteOperator";
                    command.Parameters.Add("@p_id", SqlDbType.Int).Value = id;

                    connection.Open();

                    try
                    {
                        int result = command.ExecuteNonQuery();
                        return (result == 1);
                    }
                    catch (SqlException e)
                    {
                        if (e.State == 255 && e.Number == 16) return false;
                        throw;
                    }
                }
            }
        }

        #endregion

        private IList<OperatorModel> SelectOperator()
        {
            return SelectOperator(null, null);
        }
        private IList<OperatorModel> SelectOperator(int? id)
        {
            return SelectOperator(id, null);
        }
        private IList<OperatorModel> SelectOperator(string login)
        {
            return SelectOperator(null, login);
        }
        private IList<OperatorModel> SelectOperator(int? id, string login)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                using (SqlCommand command = new SqlCommand())
                {
                    command.Connection = connection;
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "Administration.uspGetOperator";
                    if (id != null) command.Parameters.Add("@p_id", SqlDbType.Int).Value = id;
                    if (login != null) command.Parameters.Add("@p_login", SqlDbType.VarChar).Value = login;

                    return this.ConvertDataTableToOperators(command);
                }
            }
        }

        private IList<OperatorModel> ConvertDataTableToOperators(SqlCommand command)
        {
            using (SqlDataAdapter da = new SqlDataAdapter(command))
            {
                using (DataTable dt = new DataTable() { Locale = CultureInfo.CurrentCulture, })
                {
                    da.Fill(dt);

                    IList<OperatorModel> operators = (from DataRow row in dt.Rows
                                                      select new OperatorModel(row.Field<int>("Id"), row.Field<string>("Login"), row.Field<string>("FirstName"), row.Field<string>("LastName"))
                                                      {
                                                          Email = row.Field<string>("Email"),
                                                          internalModifiedDate = row.Field<DateTime>("ModifiedDate"),
                                                          IsPasswordDefined = row.Field<bool>("IsPasswordDefined"),
                                                      }).ToList();

                    return operators;
                }
            }
        }
    }
}