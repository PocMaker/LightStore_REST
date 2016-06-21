using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace LightStore.Dal
{
    /// <summary>
    /// Abstract class for a DB connected class
    /// </summary>
    public abstract class ABaseDal
    {
        /// <summary>
        /// Connection string to access DB
        /// </summary>
        protected internal readonly string _connectionString;

        /// <summary>
        /// Ctor for an access to the DB set in the config file
        /// </summary>
        public ABaseDal()
        {
            ConnectionStringSettings conSettings = ConfigurationManager.ConnectionStrings["main"];
            _connectionString = conSettings.ConnectionString;
        }


        /// <summary>
        /// Ctor for an access to the DB <paramref name="connectionString"/>
        /// </summary>
        /// <param name="connectionString">Specific address to join the DB</param>
        public ABaseDal(string connectionString)
        {
            _connectionString = connectionString;
        }
    }
}