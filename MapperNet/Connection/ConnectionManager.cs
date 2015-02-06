using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Common;
using System.Linq;

namespace MapperNet.Connection
{
    public class ConnectionManager
    {
        #region Protected Variables

        protected string connectionString = string.Empty;

        protected DbConnection connection = null;

        protected DbCommand command = null;

        protected DbProviderFactory providerFactory = null;

        #endregion

        #region Constructor

        protected ConnectionManager(string connectionString)
        {
            this.connectionString = ConfigurationManager.ConnectionStrings[connectionString].ConnectionString;
            this.providerFactory = DbProviderFactories.GetFactory(ConfigurationManager.ConnectionStrings[connectionString].ProviderName);
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets the connection to the database
        /// </summary>
        public DbConnection Connection 
        {
            get 
            {
                if (this.connection == null)
                {
                    this.connection = this.providerFactory.CreateConnection();
                }

                if (string.IsNullOrEmpty(this.connection.ConnectionString))
                {
                    this.connection.ConnectionString = this.connectionString;
                }

                return this.connection;
            }
        }

        /// <summary>
        /// Gets the command to execute queries
        /// </summary>
        public DbCommand Command
        {
            get
            {
                if (this.command == null)
                {
                    this.command = this.providerFactory.CreateCommand();
                }

                return this.command;
            }
        }

        #endregion

        #region Factory Method
        
        /// <summary>
        /// Creates a connection manager instance
        /// </summary>
        /// <param name="connectionString">The connection string name</param>
        /// <returns></returns>
        public static ConnectionManager Create(string connectionString)
        {
            return new ConnectionManager(connectionString);
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Gets a parameter based on the name and the value passed
        /// </summary>
        /// <param name="parameterName">The parameter name</param>
        /// <param name="parameterValue">The parameter value</param>
        /// <returns>The parameter created</returns>
        public DbParameter GetParameter(string parameterName, object parameterValue)
        {
            var parameter = this.providerFactory.CreateParameter();
            parameter.ParameterName = parameterName;
            parameter.Value = parameterValue;

            return parameter;
        }

        #endregion
    }
}
