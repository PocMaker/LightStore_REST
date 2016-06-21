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
    public interface IProductDal : ICrudDalT<ProductModel>
    {
        /// <summary>
        /// Select a product thanks to its GTIN include in cab under "01" ai
        /// </summary>
        /// <param name="cab">Cab containing "01" ai to describe product GTIN</param>
        /// <returns>Product Data</returns>
        ProductModel Select(string cab);
    }

    /// <summary>
    /// CRUD actions to manage products
    /// </summary>
    public sealed class ProductDal : ABaseDal, IProductDal
    {
        /// <summary>
        /// DB is describing in the "main" node of ConnectionString in the config file
        /// </summary>
        public ProductDal() : base() { }
        /// <summary>
        /// DB is describing in the <paramref name="connectionString"/> argument (unit tests only)
        /// </summary>
        /// <param name="connectionString">A way to connect to a specific DB</param>
        public ProductDal(string connectionString) : base(connectionString) { }

        #region IProductDal

        /// <summary>
        /// Delete a specific product
        /// </summary>
        /// <param name="id">Product id to delete</param>
        /// <exception cref="SqlException">Severity 16
        ///     <para>State 255 : Product x does no longer exist</para>
        /// </exception>
        /// <returns>True in case of success</returns>
        public bool Delete(int id)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                using (SqlCommand command = new SqlCommand())
                {
                    command.Connection = connection;
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "Product.uspDeleteProduct";
                    command.Parameters.Add("@p_id", SqlDbType.Int).Value = id;

                    connection.Open();

                    int result = command.ExecuteNonQuery();
                    return (result == 1);
                }
            }
        }

        /// <summary>
        /// Create a new product from given <paramref name="item"/>
        /// </summary>
        /// <param name="item">A new product</param>
        /// <exception cref="ArgumentNullException">Thrown when input <paramref name="item"/> is null</exception>
        /// <exception cref="SqlException">Severity 16
        ///     <para>State 10 : Code cannot be EMPTY</para>
        ///     <para>State 11 : Label cannot be EMPTY</para>
        ///     <para>State 12 : Code already exists</para>
        ///     <para>State 13 : GTIN already exists (in products)</para>
        ///     <para>State 14 : GTIN already exists (in packagings)</para>
        ///     <para>State 255 : Error while creating product</para>
        /// </exception>
        /// <returns>An ProductModel describing the created product</returns>
        public ProductModel Insert(ProductModel item)
        {
            if (item == null) throw new ArgumentNullException("item", "Product to create cannot be null");

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                using (SqlCommand command = new SqlCommand())
                {
                    command.Connection = connection;
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "Product.uspCreateProduct";
                    command.Parameters.Add("@p_code", SqlDbType.VarChar).Value = item.Code;
                    command.Parameters.Add("@p_label", SqlDbType.VarChar).Value = item.Label;
                    command.Parameters.Add("@p_unity", SqlDbType.VarChar).Value = item.Unity;
                    command.Parameters.Add("@p_weight", SqlDbType.Decimal).Value = item.Weight;
                    command.Parameters.Add("@p_tare", SqlDbType.Decimal).Value = item.Tare;
                    command.Parameters.Add("@p_gtin", SqlDbType.VarChar).Value = item.GTIN;

                    var products = this.ConvertDataTableToProducts(command);
                    return products.First();
                }
            }
        }

        /// <summary>
        /// Select all products
        /// </summary>
        /// <returns>A list of ProductModel describing each product</returns>
        public IList<ProductModel> Select()
        {
            return SelectProduct(null, null);
        }

        /// <summary>
        /// Select a specific product
        /// </summary>
        /// <param name="id">Id of wanted product</param>
        /// <returns>An ProductModel describing the required product</returns>
        public ProductModel Select(int id)
        {
            var products = SelectProduct(id);
            return products.First();
        }

        /// <exclude/>
        public ProductModel Update(ProductModel item)
        {
            throw new NotImplementedException();
        }

        #endregion

        private IList<ProductModel> SelectProduct(int? id)
        {
            return SelectProduct(id, null);
        }
        private IList<ProductModel> SelectProduct(string gtin)
        {
            return SelectProduct(null, gtin);
        }
        private IList<ProductModel> SelectProduct(int? id, string gtin)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                using (SqlCommand command = new SqlCommand())
                {
                    command.Connection = connection;
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "Product.uspGetProduct";
                    if (id != null) command.Parameters.Add("@p_id", SqlDbType.Int).Value = id;
                    if (!String.IsNullOrWhiteSpace(gtin)) command.Parameters.Add("@p_gtin", SqlDbType.VarChar).Value = gtin;

                    return this.ConvertDataTableToProducts(command);
                }
            }
        }

        private IList<ProductModel> ConvertDataTableToProducts(SqlCommand command)
        {
            using (SqlDataAdapter da = new SqlDataAdapter(command))
            {
                using (DataTable dt = new DataTable() { Locale = CultureInfo.CurrentCulture, })
                {
                    da.Fill(dt);

                    IList<ProductModel> products = (from DataRow row in dt.Rows
                                                    select new ProductModel
                                                    {
                                                        Id = row.Field<int>("Id"),
                                                        Code = row.Field<string>("Code"),
                                                        Label = row.Field<string>("Label"),
                                                        Unity = row.Field<string>("Unity"),
                                                        GTIN = row.Field<string>("GTIN"),
                                                        internalModifiedDate = row.Field<DateTime>("ModifiedDate"),
                                                        Weight = row.Field<decimal>("Weight"),
                                                        Tare = row.Field<decimal>("Tare"),
                                                    }).ToList();

                    return products;
                }
            }
        }

        /// <summary>
        /// Select a product thanks to its GTIN include in cab under "01" ai
        /// </summary>
        /// <param name="cab">Cab containing "01" ai to describe product GTIN</param>
        /// <returns>Product Data</returns>
        public ProductModel Select(string cab)
        {
            string gtin = null;

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                using (SqlCommand command = new SqlCommand())
                {
                    command.Connection = connection;
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "Barcode.uspDecodeBarcode";
                    command.Parameters.Add("@p_cab", SqlDbType.VarChar).Value = cab;

                    using (SqlDataAdapter adapter = new SqlDataAdapter(command))
                    {
                        using (DataTable dt = new DataTable { Locale = CultureInfo.CurrentCulture })
                        {
                            adapter.Fill(dt);
                            gtin = (from DataRow row in dt.Rows
                                    where row.Field<string>("Code") == "01" || row.Field<string>("Code") == "02"
                                    orderby row.Field<string>("Code") ascending
                                    select row.Field<string>("Value")).FirstOrDefault();
                            if (String.IsNullOrWhiteSpace(gtin))
                            {
                                if (dt.Rows.Count == 1) gtin = dt.Rows[0].Field<string>("Value");
                                else return null;
                            }
                        }
                    }
                }
            }
            
            var products = SelectProduct(gtin);
            return products.FirstOrDefault();
        }
    }
}