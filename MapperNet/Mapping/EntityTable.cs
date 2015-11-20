using MapperNet.Connection;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;

namespace MapperNet.Mapping
{
    public class EntityTable<TModel> where TModel : class
    {
        #region Properties

        /// <summary>
        /// The table's name
        /// </summary>
        public string TableName { get; protected set; }

        /// <summary>
        /// The mapping amoung table's columns and object properties
        /// </summary>
        public Dictionary<string, string> FieldMapping { get; protected set; }

        /// <summary>
        /// The name of the primary key in the table
        /// </summary>
        public string PrimaryKeyName { get; protected set; }

        /// <summary>
        /// The connection manager
        /// </summary>
        public ConnectionManager Connection { get; protected set; }

        #endregion

        #region Constructor

        public EntityTable() : this("DefaultConnection") { }

        public EntityTable(string connectionString)
        {
            this.Connection = ConnectionManager.Create(connectionString);
            this.FieldMapping = new Dictionary<string, string>();

            this.MapEntity();
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Gets the list of all models
        /// </summary>
        /// <returns>A list of models</returns>
        public virtual IQueryable<TModel> Query()
        {
            return this.Query(string.Format("SELECT * FROM {0}", this.TableName));
        }

        /// <summary>
        /// Gets a list of models based on the specified query
        /// </summary>
        /// <param name="query">The query to execute</param>
        /// <returns>A list of models</returns>
        public virtual IQueryable<TModel> Query(string query)
        {
            return Query(query, null);
        }

        /// <summary>
        /// Gets a list of models based on the specified query
        /// </summary>
        /// <param name="query">The query to execute</param>
        /// <param name="parameters">A map of parameters for the query to execute</param>
        /// <returns>A list of models</returns>
        public virtual IQueryable<TModel> Query(string query, IDictionary<string, object> parameters)
        {
            IQueryable<TModel> modelSet = null;

            using (var conn = this.Connection.Connection)
            {
                var command = this.Connection.Command;
                command.CommandText = query;
                command.Connection = conn;

                if (parameters != null && parameters.Count > 0)
                {
                    var queryParams = this.BuildParameters(parameters, true);

                    command.Parameters.Clear();
                    foreach (var param in queryParams)
                    {
                        command.Parameters.Add(param);
                    }
                }

                try
                {
                    conn.Open();
                    using (var reader = command.ExecuteReader())
                    {
                        modelSet = this.BuildModelSetFromReader(reader);
                    }
                }
                catch
                {
                    throw;
                }
            }

            return modelSet;
        }

        /// <summary>
        /// Insert a model in the database
        /// </summary>
        /// <param name="model">The model to insert</param>
        public virtual void Insert(TModel model)
        {
            using (var conn = this.Connection.Connection)
            {
                var sql = this.PrepareSqlInsert();
                var parameters = this.BuildParameters(model);

                var command = this.Connection.Command;
                command.CommandText = sql;
                command.Connection = conn;

                command.Parameters.Clear();
                foreach (var param in parameters)
                {
                    command.Parameters.Add(param);
                }

                try
                {
                    conn.Open();
                    command.ExecuteNonQuery();
                }
                catch
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// Update a model of the database
        /// </summary>
        /// <param name="model">The model to update</param>
        public virtual void Update(TModel model)
        {
            using (var conn = this.Connection.Connection)
            {
                var sql = this.PrepareSqlUpdate();
                var parameters = this.BuildParameters(model, true);

                var command = this.Connection.Command;
                command.CommandText = sql;
                command.Connection = conn;

                command.Parameters.Clear();
                foreach (var param in parameters)
                {
                    command.Parameters.Add(param);
                }

                try
                {
                    conn.Open();
                    command.ExecuteNonQuery();
                }
                catch
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// Delete a model from the database
        /// </summary>
        /// <param name="model">The model to delete</param>
        public virtual void Delete(TModel model)
        {
            using (var conn = this.Connection.Connection)
            {
                var sql = this.PrepareSqlDelete();

                var primaryKeyValue = model.GetType().GetProperty(FieldMapping.Where(f => f.Key == this.PrimaryKeyName).FirstOrDefault().Value).GetValue(model);
                var parameters = this.BuildParameters(new Dictionary<string, object>() { { this.PrimaryKeyName, primaryKeyValue } }, true);

                var command = this.Connection.Command;
                command.CommandText = sql;
                command.Connection = conn;

                command.Parameters.Clear();
                foreach (var param in parameters)
                {
                    command.Parameters.Add(param);
                }

                try
                {
                    conn.Open();
                    command.ExecuteNonQuery();
                }
                catch
                {
                    throw;
                }
            }
        }

        #endregion

        #region Protected Methods

        /// <summary>
        /// Maps the entity to the database's table
        /// </summary>
        protected virtual void MapEntity()
        {
            var modelType = typeof(TModel);
            this.TableName = modelType.Name;

            var tableAttribute = modelType.GetCustomAttributes(true).FirstOrDefault(t => t.GetType() == typeof(TableAttribute));
            if (tableAttribute != null)
            {
                var tableName = (tableAttribute as TableAttribute).Name;
                if (!string.IsNullOrEmpty(tableName))
                    this.TableName = tableName;
            }

            this.MapFields();
        }

        /// <summary>
        /// Sets the mappings for the model fields
        /// </summary>
        protected virtual void MapFields()
        {
            var modelType = typeof(TModel);

            foreach (var property in modelType.GetProperties())
            {
                var columnAttribute = property.GetCustomAttributes(true).FirstOrDefault(c => c.GetType() == typeof(ColumnAttribute));

                if (columnAttribute != null)
                {
                    var column = (columnAttribute as ColumnAttribute);
                    var columnName = column.Name ?? property.Name;

                    this.FieldMapping.Add(columnName, property.Name);
                    if (column.IsPrimaryKey)
                        this.PrimaryKeyName = columnName;
                }
            }
        }

        /// <summary>
        /// Build a model list from a datareader
        /// </summary>
        /// <param name="reader">The datareader</param>
        /// <returns>A list of models</returns>
        protected IQueryable<TModel> BuildModelSetFromReader(DbDataReader reader)
        {
            var modelSet = new List<TModel>();

            while (reader.Read())
            {
                var modelType = typeof(TModel);
                var modelInstance = (TModel)Activator.CreateInstance(modelType);

                foreach (var field in this.FieldMapping.Keys)
                {
                    modelType.GetProperty(FieldMapping[field]).SetValue(modelInstance, reader[field]);
                }

                modelSet.Add(modelInstance);
            }

            return (modelSet.AsQueryable() as IQueryable<TModel>);
        }

        /// <summary>
        /// Prepare and returns the INSERT sql string
        /// </summary>
        /// <returns>The INSERT sql string</returns>
        protected string PrepareSqlInsert()
        {
            var fields = new List<string>();
            var values = new List<string>();

            foreach (var fieldMap in this.FieldMapping)
            {
                if (fieldMap.Key != this.PrimaryKeyName)
                {
                    fields.Add(fieldMap.Key);
                    values.Add(string.Format("@{0}", fieldMap.Value));
                }
            }

            return string.Format("INSERT INTO {0}({1}) VALUES({2})", this.TableName, string.Join(",", fields), string.Join(",", values));
        }

        /// <summary>
        /// Prepare and returns the UPDATE sql string
        /// </summary>
        /// <returns>The UPDATE sql string</returns>
        protected string PrepareSqlUpdate()
        {
            var values = new List<string>();
            string condition = string.Empty;

            foreach (var fieldMap in this.FieldMapping)
            {
                string value = string.Format("{0}=@{1}", fieldMap.Key, fieldMap.Value);
                if (fieldMap.Key != this.PrimaryKeyName)
                {
                    values.Add(value);
                }
                else
                {
                    condition = value;
                }
            }

            return string.Format("UPDATE {0} SET {1} WHERE {2}", this.TableName, string.Join(",", values), condition);
        }

        /// <summary>
        /// Prepare and returns the DELETE sql string
        /// </summary>
        /// <returns>The DELETE sql string</returns>
        protected string PrepareSqlDelete()
        {
            var primaryKeyMap = this.FieldMapping.Where(f => f.Key == this.PrimaryKeyName).First();
            string condition = string.Format("{0}=@{1}", primaryKeyMap.Key, primaryKeyMap.Value);

            return string.Format("DELETE FROM {0} WHERE {1}", this.TableName, condition);
        }

        /// <summary>
        /// Create a list of parameters for the query to execute
        /// </summary>
        /// <param name="model">The model from which build the list of parameters</param>
        /// <returns>The list of parameters created</returns>
        protected IEnumerable<DbParameter> BuildParameters(TModel model)
        {
            return BuildParameters(model, false);
        }

        /// <summary>
        /// Create a list of parameters for the query to execute
        /// </summary>
        /// <param name="model">The model from which build the list of parameters</param>
        /// <param name="includePrimaryKey">Indicates wheter include the primary key field in the parameters list</param>
        /// <returns>The list of parameters created</returns>
        protected IEnumerable<DbParameter> BuildParameters(TModel model, bool includePrimaryKey)
        {
            var propertyMap = model.GetType().GetProperties().ToDictionary(p => p.Name, p => p.GetValue(model));
            return BuildParameters(propertyMap, includePrimaryKey);
        }

        /// <summary>
        /// Create a list of parameters for the query to execute
        /// </summary>
        /// <param name="fields">A map of fields to add to the parameters</param>
        /// <returns>The list of parameters created</returns>
        protected IEnumerable<DbParameter> BuildParameters(IDictionary<string, object> fields)
        {
            return BuildParameters(fields, false);
        }

        /// <summary>
        /// Create a list of parameters for the query to execute
        /// </summary>
        /// <param name="fields">A map of fields to add to the parameters</param>
        /// <param name="includePrimaryKey">Indicates wheter include the primary key field in the parameters list</param>
        /// <returns>The list of parameters created</returns>
        protected IEnumerable<DbParameter> BuildParameters(IDictionary<string, object> fields, bool includePrimaryKey)
        {
            var parameters = new List<DbParameter>();

            foreach (var field in fields)
            {
                if ((field.Key != this.FieldMapping.Where(f => f.Key == this.PrimaryKeyName).First().Value) || includePrimaryKey)
                {
                    parameters.Add(this.Connection.GetParameter(string.Format("@{0}", field.Key), field.Value));
                }
            }

            return parameters;
        }

        #endregion
    }
}
