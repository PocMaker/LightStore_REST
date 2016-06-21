using LightStore.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Web;

namespace LightStore.Dal
{
    /// <summary>
    /// CRUD actions to manage products
    /// </summary>
    public interface IPackagingDal
    {
        /// <summary>
        /// Select all products
        /// </summary>
        /// <param name="productId">Product id for which returns all packaging</param>
        /// <returns>A list of PackagingModel describing each packaging for the specified product</returns>
        IList<PackagingModel> Select(int productId);
        /// <summary>
        /// Select a specific product
        /// </summary>
        /// <param name="productId">Product id for which returns all packaging</param>
        /// <param name="id">Id of wanted product</param>
        /// <returns>An PackagingModel describing the required packaging</returns>
        PackagingModel Select(int productId, int id);

        /// <summary>
        /// Create a new product from given <paramref name="item"/>
        /// </summary>
        /// <param name="item">A new packaging</param>
        /// <returns>An PackagingModel describing the created packaging</returns>
        PackagingModel Insert(PackagingModel item);

        /// <summary>
        /// Delete a specific product
        /// </summary>
        /// <param name="productId">Product id for which packaging have to be deleted</param>
        /// <param name="id">Product id to delete</param>
        /// <returns>True in case of success</returns>
        bool Delete(int productId, int? id);
    }

    /// <summary>
    /// CRUD actions to manage products
    /// </summary>
    public sealed class PackagingDal : ABaseDal, IPackagingDal
    {
        /// <summary>
        /// DB is describing in the "main" node of ConnectionString in the config file
        /// </summary>
        public PackagingDal() : base() { }
        /// <summary>
        /// DB is describing in the <paramref name="connectionString"/> argument (unit tests only)
        /// </summary>
        /// <param name="connectionString">A way to connect to a specific DB</param>
        public PackagingDal(string connectionString) : base(connectionString) { }

        #region IProductDal

        /// <summary>
        /// Delete a specific product
        /// </summary>
        /// <param name="productId">Product id for which packaging have to be deleted</param>
        /// <param name="id">Packaging id to delete</param>
        /// <exception cref="SqlException">Severity 16
        ///     <para>State 255 : Error while deleting packaging</para>
        /// </exception>
        /// <returns>True in case of success</returns>
        public bool Delete(int productId, int? id)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                using (SqlCommand command = new SqlCommand())
                {
                    command.Connection = connection;
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "Product.uspDeletePackaging";
                    command.Parameters.Add("@p_productId", SqlDbType.Int).Value = productId;
                    if (id != null) command.Parameters.Add("@p_id", SqlDbType.Int).Value = id;

                    connection.Open();

                    int result = command.ExecuteNonQuery();
                    return (result == 1);
                }
            }
        }

        /// <summary>
        /// Create a new product from given <paramref name="item"/>
        /// </summary>
        /// <param name="item">A new packaging</param>
        /// <exception cref="ArgumentNullException">Thrown when input <paramref name="item"/> is null</exception>
        /// <exception cref="SqlException">Severity 16
        ///     <para>State 10 : GTIN already exists (in products)</para>
        ///     <para>State 11 : GTIN already exists (in packagings)</para>
        ///     <para>State 12 : Quantity cannot be negative</para>
        ///     <para>State 255 : Error while creating packaging</para>
        /// </exception>
        /// <returns>An PackagingModel describing the created packaging</returns>
        public PackagingModel Insert(PackagingModel item)
        {
            if (item == null) throw new ArgumentNullException("item", "Packaging to create cannot be null");

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                using (SqlCommand command = new SqlCommand())
                {
                    command.Connection = connection;
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "Product.uspCreatePackaging";
                    command.Parameters.Add("@p_unity", SqlDbType.VarChar).Value = item.Unity;
                    command.Parameters.Add("@p_quantity", SqlDbType.Int).Value = item.Quantity;
                    command.Parameters.Add("@p_fromId", SqlDbType.Int).Value = item.FromId;
                    command.Parameters.Add("@p_gtin", SqlDbType.VarChar).Value = item.GTIN;
                    command.Parameters.Add("@p_productId", SqlDbType.Int).Value = item.ProductId;

                    var products = this.ConvertDataTableToProducts(command);
                    return products.First();
                }
            }
        }

        /// <summary>
        /// Select all products
        /// </summary>
        /// <param name="productId">Product id for which returns all packaging</param>
        /// <returns>A list of PackagingModel describing each packaging</returns>
        public IList<PackagingModel> Select(int productId)
        {
            return SelectProduct(productId, null);
        }

        /// <summary>
        /// Select a specific product
        /// </summary>
        /// <param name="productId">Product id for which returns all packaging</param>
        /// <param name="id">Id of wanted packaging</param>
        /// <returns>An PackagingModel describing the required packaging</returns>
        public PackagingModel Select(int productId, int id)
        {
            var products = SelectProduct(productId, id);
            return products.First();
        }

        /// <exclude/>
        public PackagingModel Update(PackagingModel item)
        {
            throw new NotImplementedException();
        }

        #endregion

        private IList<PackagingModel> SelectProduct(int productId, int? id)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                using (SqlCommand command = new SqlCommand())
                {
                    command.Connection = connection;
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "Product.uspGetPackaging";
                    command.Parameters.Add("@p_productId", SqlDbType.Int).Value = productId;
                    if (id != null) command.Parameters.Add("@p_id", SqlDbType.Int).Value = id;

                    return this.ConvertDataTableToProducts(command);
                }
            }
        }

        private IList<PackagingModel> ConvertDataTableToProducts(SqlCommand command)
        {
            using (SqlDataAdapter da = new SqlDataAdapter(command))
            {
                using (DataTable dt = new DataTable() { Locale = CultureInfo.CurrentCulture, })
                {
                    da.Fill(dt);

                    IList<PackagingModel> products = (from DataRow row in dt.Rows
                                                      select new PackagingModel
                                                      {
                                                          Id = row.Field<int>("Id"),
                                                          Unity = row.Field<string>("Unity"),
                                                          Quantity = row.Field<int>("Quantity"),
                                                          FromId = row.Field<int?>("FromId"),
                                                          GTIN = row.Field<string>("GTIN"),
                                                          ProductId = row.Field<int>("ProductId"),
                                                          internalModifiedDate = row.Field<DateTime>("ModifiedDate"),
                                                      }).ToList();

                    return products;
                }
            }
        }
    }
}