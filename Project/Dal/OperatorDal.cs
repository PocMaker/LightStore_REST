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

        /// <summary>
        /// Update password for operator with specified id
        /// </summary>
        /// <param name="id">Operator Id</param>
        /// <param name="password">New password for operator</param>
        /// <returns>Updated operator</returns>
        OperatorModel Update(int id, string password);

        /// <summary>
        /// Check if login and associated password match in DB
        /// </summary>
        /// <param name="login">Operator login</param>
        /// <param name="password">Current operator password</param>
        /// <exception cref="SqlException">Thrown when login or password is wrong</exception>
        /// <returns>Matched operator data</returns>
        OperatorModel LogIn(string login, string password);
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

        /// <summary>
        /// Check if login and associated password match in DB
        /// </summary>
        /// <param name="login">Operator login</param>
        /// <param name="password">Current operator password</param>
        /// <exception cref="SqlException">Thrown when login or password is wrong</exception>
        /// <returns>Matched operator data</returns>
        public OperatorModel LogIn(string login, string password)
        {
            if (String.IsNullOrWhiteSpace(login)) throw new ArgumentNullException("login", "Login cannot be null");

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                using (SqlCommand command = new SqlCommand())
                {
                    command.Connection = connection;
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "Administration.uspLoginOperator";
                    command.Parameters.Add("@p_login", SqlDbType.VarChar).Value = login;
                    command.Parameters.Add("@p_password", SqlDbType.VarChar).Value = password;
                    
                    var operators = this.ConvertDataTableToOperators(command);
                    return operators.First();
                }
            }
        }

        /// <summary>
        /// Update password for operator with specified id
        /// </summary>
        /// <param name="id">Operator Id</param>
        /// <param name="password">New password for operator</param>
        /// <returns>Updated operator</returns>
        public OperatorModel Update(int id, string password)
        {
            if (id == 0) throw new ArgumentNullException("id", "Id cannot be null");

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                using (SqlCommand command = new SqlCommand())
                {
                    command.Connection = connection;
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "Administration.uspUpdateOperator";
                    command.Parameters.Add("@p_id", SqlDbType.Int).Value = id;
                    command.Parameters.Add("@p_password", SqlDbType.VarChar).Value = password;

                    var operators = this.ConvertDataTableToOperators(command);
                    return operators.First();
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
                                                      select new OperatorModel
                                                      {
                                                          Id = row.Field<int>("Id"),
                                                          Login = row.Field<string>("Login"),
                                                          FirstName = row.Field<string>("FirstName"),
                                                          LastName = row.Field<string>("LastName"),
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