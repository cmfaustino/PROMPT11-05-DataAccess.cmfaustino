namespace Mod05_ChelasDAL.Metadata
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;

    using Mod05_ChelasDAL.DataAttributes;

    public class MetaDataStore
    {
        private readonly Dictionary<Type, TableInfo> typeToTableInfo = new Dictionary<Type, TableInfo>();

        public TableInfo GetTableInfoFor<TEntity>()
        {
            return GetTableInfoFor(typeof(TEntity));
        }

        public TableInfo GetTableInfoFor(Type entityType)
        {
            if (!typeToTableInfo.ContainsKey(entityType))
            {
                return null;
            }

            return typeToTableInfo[entityType];
        }

        public void BuildMetaDataFor(Assembly assembly)
        {
            BuildMapOfEntityTypesWithTheirTableInfo(assembly);

            foreach (KeyValuePair<Type, TableInfo> pair in typeToTableInfo)
            {
                // we need this info for each entity before we can deal with references to other entities
                LoopThroughPropertiesWith<PrimaryKeyAttribute>(pair.Key, pair.Value, SetPrimaryKeyInfo);
            }

            foreach (KeyValuePair<Type, TableInfo> pair in typeToTableInfo)
            {
                LoopThroughPropertiesWith<ReferenceAttribute>(pair.Key, pair.Value, AddReferenceInfo);
                LoopThroughPropertiesWith<ColumnAttribute>(pair.Key, pair.Value, AddColumnInfo);
            }
        }


        public void BuildTableInfoFor<TEntity>()
        {
            this.BuildTableInfoForType(typeof(TEntity));
        }

        public void BuildTableInfoForType(Type type)
        {
            var typeAttributes = Attribute.GetCustomAttributes(type, typeof(TableAttribute));

            TableInfo tableInfo;

            if (typeAttributes.Length > 0)
            {
                var tableAttribute = (TableAttribute)typeAttributes[0];
                //var tableInfo = new TableInfo(this, tableAttribute.TableName, type);
                tableInfo = new TableInfo(this, tableAttribute.TableName, type);
                this.typeToTableInfo.Add(type, tableInfo);
            }

            // HACK: acrescenta info das colunas (apenas uma PrimaryKeyAttribute) - feito
            // TODO: acrescentar info das colunas (Column)

            PrimaryKeyAttribute typePkAttribute;

            foreach (var prop in type.GetProperties())
            {
                var propPkAttrs = prop.GetCustomAttributes(typeof(PrimaryKeyAttribute), false);
                if (propPkAttrs.Length != 0)
                {
                    if(typePkAttribute == null)
                    {
                        typePkAttribute = propPkAttrs[0];
                    }
                    else
                    {
                    throw new InvalidOperationException(
                        string.Format("Type {0} have more than one PrimaryKeyAttribute", type.Name));
                    }
                }
                //break;
                //propertyInfo = prop;
            }

            var typePkAttributes = Attribute.GetCustomAttributes(type, typeof(PrimaryKeyAttribute));

            if (typePkAttributes.Length > 0)
            {
                var typePkAttribute = (PrimaryKeyAttribute)typePkAttributes[0];

                if (tableInfo == null)
                {
                    throw new InvalidOperationException(
                        string.Format("Type {0} have a PrimaryKeyAttribute but no TableAttribute", type.Name));
                }

                PropertyInfo propertyInfo;

                foreach (var prop in type.GetProperties())
                {
                    var propAttrs = prop.GetCustomAttributes(typeof(PrimaryKeyAttribute), false);
                    if (propAttrs.Length == 0) continue;
                    break;
                    propertyInfo = prop;
                }

                tableInfo.PrimaryKey = new ColumnInfo(this, typePkAttribute.ColumnName, type.GetProperties()[0].GetType(), propertyInfo);
            }

            // TODO: acrescentar info das colunas (Column)

            var typePkAttributes = Attribute.GetCustomAttributes(type, typeof(PrimaryKeyAttribute));
            Attribute.

            if (typePkAttributes.Length > 0)
            {
                var typePkAttribute = (PrimaryKeyAttribute)typePkAttributes[0];

                if (tableInfo == null)
                {
                    throw new InvalidOperationException(
                        string.Format("Type {0} have a PrimaryKeyAttribute but no TableAttribute", type.Name));
                }

                PropertyInfo propertyInfo;

                foreach (var prop in type.GetProperties())
                {
                    var propAttrs = prop.GetCustomAttributes(typeof(PrimaryKeyAttribute), false);
                    if (propAttrs.Length == 0) continue;
                    break;
                    propertyInfo = prop;
                }

                tableInfo.PrimaryKey = new ColumnInfo(this, typePkAttribute.ColumnName, type.GetProperties()[0].GetType(), propertyInfo);
            }
        }

        private void BuildMapOfEntityTypesWithTheirTableInfo(Assembly assembly)
        {
            foreach (var type in assembly.GetTypes())
            {
                this.BuildTableInfoForType(type);
            }
        }

        

        private void LoopThroughPropertiesWith<TAttribute>(
            Type entityType, TableInfo tableInfo, Action<TableInfo, PropertyInfo, TAttribute> andExecuteFollowingCode)
            where TAttribute : Attribute
        {
            foreach (var propertyInfo in entityType.GetProperties())
            {
                var attribute = GetAttribute<TAttribute>(propertyInfo);

                if (attribute != null)
                {
                    andExecuteFollowingCode(tableInfo, propertyInfo, attribute);
                }
            }
        }

        private void SetPrimaryKeyInfo(
            TableInfo tableInfo, PropertyInfo propertyInfo, PrimaryKeyAttribute primaryKeyAttribute)
        {
            tableInfo.PrimaryKey = new ColumnInfo(
                this, primaryKeyAttribute.ColumnName, propertyInfo.PropertyType, propertyInfo);
        }

        private void AddColumnInfo(TableInfo tableInfo, PropertyInfo propertyInfo, ColumnAttribute columnAttribute)
        {
            tableInfo.AddColumn(
                new ColumnInfo(this, columnAttribute.ColumnName, propertyInfo.PropertyType, propertyInfo));
        }

        private void AddReferenceInfo(
            TableInfo tableInfo, PropertyInfo propertyInfo, ReferenceAttribute referenceAttribute)
        {
            tableInfo.AddReference(
                new ReferenceInfo(this, referenceAttribute.ColumnName, propertyInfo.PropertyType, propertyInfo));
        }

        private TAttribute GetAttribute<TAttribute>(PropertyInfo propertyInfo) where TAttribute : Attribute
        {
            var attributes = Attribute.GetCustomAttributes(propertyInfo, typeof(TAttribute));
            if (attributes.Length == 0) return null;
            return (TAttribute)attributes[0];
        }
    }
}