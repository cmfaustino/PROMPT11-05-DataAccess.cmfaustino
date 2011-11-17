namespace Mod05_ChelasDAL.Metadata
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// This class holds all metadata to map an entity to a SQL table.
    /// </summary>
    public class TableInfo : MetaData
    {
        private readonly Dictionary<string, ColumnInfo> _columns = new Dictionary<string, ColumnInfo>();

        private readonly Dictionary<string, ReferenceInfo> _references = new Dictionary<string, ReferenceInfo>();

        /// <summary>
        /// Initializes a new instance of the <see cref="TableInfo"/> class.
        /// </summary>
        /// <param name="store">The <see cref="MetaDataStore"/>.</param>
        /// <param name="name">The table name.</param>
        /// <param name="entityType">Type of the entity.</param>
        public TableInfo(MetaDataStore store, string name, Type entityType)
            : base(store)
        {
            Name = name;
            EntityType = entityType;
        }

        /// <summary>
        /// Gets the table name.
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// Gets the type of the entity.
        /// </summary>
        /// <value>
        /// The type of the entity.
        /// </value>
        public Type EntityType { get; private set; }

        /// <summary>
        /// Gets or sets the primary key <see cref="ColumnInfo"/>.
        /// </summary>
        /// <value>
        /// The primary key.
        /// </value>
        public ColumnInfo PrimaryKey { get; set; }


        /// <summary>
        /// Gets all entity references.
        /// </summary>
        public IEnumerable<ReferenceInfo> References
        {
            get { return _references.Values; }
        }

        /// <summary>
        /// Gets all entity columns <see cref="ColumnInfo"/>.
        /// </summary>
        public IEnumerable<ColumnInfo> Columns
        {
            get { return _columns.Values; }
        }


        /// <summary>
        /// Adds a column to the entity.
        /// </summary>
        /// <param name="column">The column to add.</param>
        public void AddColumn(ColumnInfo column)
        {
            if (_columns.ContainsKey(column.Name))
            {
                throw new InvalidOperationException(
                    string.Format("An item with key {0} has already been added", column.Name));
            }

            _columns.Add(column.Name, column);
        }

        /// <summary>
        /// Adds a reference to the entity.
        /// </summary>
        /// <param name="reference">The reference to add.</param>
        public void AddReference(ReferenceInfo reference)
        {
            if (_references.ContainsKey(reference.Name))
            {
                throw new InvalidOperationException(
                    string.Format("An item with key {0} has already been added", reference.Name));
            }

            _references.Add(reference.Name, reference);
        }

        /// <summary>
        /// Gets a <see cref="ColumnInfo"/> given its <paramref name="columnName"/>.
        /// </summary>
        /// <param name="columnName">Name of the column.</param>
        /// <returns>The corresponding <see cref="ColumnInfo"/></returns>
        public ColumnInfo GetColumn(string columnName)
        {
            if (!_columns.ContainsKey(columnName))
            {
                throw new InvalidOperationException(
                    string.Format("The table '{0}' does not have a '{1}' column", Name, columnName));
            }

            return _columns[columnName];
        }


        #region SQL commands construction
        public StringBuilder GetSelectStatementForAllFields()
        {
            var builder = new StringBuilder("SELECT " + Escape(PrimaryKey.Name) + ", ");

            AddColumnNames(builder);
            builder.Append(" FROM " + Escape(Name));

            return builder;
        }


        public string GetInsertStatement()
        {
            var builder = new StringBuilder("INSERT INTO " + Escape(Name) + " (");

            AddColumnNames(builder);
            builder.Append(") VALUES (");
            AddColumnParameterNames(builder);
            builder.Append("); SELECT SCOPE_IDENTITY();");

            return builder.ToString();
        }

        public string GetUpdateStatement()
        {
            var builder = new StringBuilder("UPDATE " + Escape(Name) + " SET ");

            AddColumnsNameWithParameterName(builder);
            AddWhereByIdClause(builder);
            builder.Append(";");

            return builder.ToString();
        }

        public string GetDeleteStatement()
        {
            var builder = new StringBuilder("DELETE FROM " + Escape(Name) + " ");

            AddWhereByIdClause(builder);
            builder.Append(";");

            return builder.ToString();
        }


        private void AddColumnNames(StringBuilder builder)
        {
            builder.Append( 
                String.Join(", ", References
                                        .Concat(Columns)
                                        .Select(r => Escape(r.Name))) 
            );
        }

        private void AddColumnParameterNames(StringBuilder builder)
        {             
            builder.Append( 
                String.Join(", ", References
                                        .Concat(Columns)
                                        .Select(r => "@" + r.Name)) 
            );
        }

       private void AddColumnsNameWithParameterName(StringBuilder builder)
       {             
            builder.Append( 
                String.Join(", ", References
                                        .Concat(Columns)
                                        .Select(r => Escape(r.Name) + " = @" + r.Name)) 
            );
        }
        
        public StringBuilder AddWhereByIdClause(StringBuilder query)
        {
            query.Append(" WHERE " + Escape(PrimaryKey.Name) + " = " + GetPrimaryKeyParameterName());
            return query;
        }

        public string GetPrimaryKeyParameterName()
        {
            return "@" + PrimaryKey.Name;
        }


        public IEnumerable<AdoParameterInfo> GetParametersForInsert(object entity)
        {
            return GetParametersForAllReferenceAndRegularColumns(entity);
        }

        public IEnumerable<AdoParameterInfo> GetParametersForUpdate(object entity)
        {
            var parameters = GetParametersForAllReferenceAndRegularColumns(entity);
            parameters.Add(
                new AdoParameterInfo(PrimaryKey.Name, PrimaryKey.DbType, PrimaryKey.PropertyInfo.GetValue(entity, null)));
            return parameters;
        }

        private List<AdoParameterInfo> GetParametersForAllReferenceAndRegularColumns(object entity)
        {
            var parameters = new List<AdoParameterInfo>();

            foreach (var referenceInfo in References)
            {
                var referencedEntity = referenceInfo.PropertyInfo.GetValue(entity, null);
                var referencePrimaryKeyProperty =
                    MetaDataStore.GetTableInfoFor(referenceInfo.ReferenceType).PrimaryKey.PropertyInfo;

                if (referencedEntity == null)
                {
                    parameters.Add(new AdoParameterInfo(referenceInfo.Name, referenceInfo.DbType, null));
                }
                else
                {
                    parameters.Add(
                        new AdoParameterInfo(
                            referenceInfo.Name,
                            referenceInfo.DbType,
                            referencePrimaryKeyProperty.GetValue(referencedEntity, null)));
                }
            }

            parameters.AddRange(
                Columns.Select(columnInfo => new AdoParameterInfo(columnInfo.Name, columnInfo.DbType, columnInfo.PropertyInfo.GetValue(entity, null)))
            );

            return parameters;
        }

        private static string Escape(string name)
        {
            return "[" + name + "]";
        }

        #endregion SQL commands construction
    }


}