using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace LightStore.Dal
{
    ///
    public abstract class ABaseDal
    {
        ///
        protected internal readonly string _connectionString;

        ///
        public ABaseDal()
        {
            ConnectionStringSettings conSettings = ConfigurationManager.ConnectionStrings["main"];
            _connectionString = conSettings.ConnectionString;
        }
        ///
        public ABaseDal(string connectionString)
        {
            _connectionString = connectionString;
        }
    }
}