using System;
using System.Collections.Generic;
using System.Reflection;

namespace Utilities
{
    public static class ClassUtility
    {
        /// <summary>
        /// Lấy primaryKey của class
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static PropertyInfo GetPrimaryKeyPropertyOfClass(Type type)
        {
            PropertyInfo[] propertyInfos = type.GetProperties();
            foreach (PropertyInfo property in propertyInfos)
            {
                var nameProperty = property.Name;
                IEnumerable<Attribute> attributesOfProperty = property.GetCustomAttributes();
                foreach (Attribute attribute in attributesOfProperty)
                {
                    if (attribute.GetType().Name == "PrimaryKey")
                    {
                        return property;
                    }
                }
            }

            return null;
        }

        /// <summary>
        /// Lấy thông tin property với name property
        /// </summary>
        /// <param name="type"></param>
        /// <param name="instanceGetPropertyValue"></param>
        /// <param name="propertyName"></param>
        /// <returns></returns>
        public static object GetPropertyValueByName(Type type, object instanceGetPropertyValue, string propertyName)
        {
            PropertyInfo[] propertyInfos = type.GetProperties();
            foreach (PropertyInfo property in propertyInfos)
            {
                if (property.Name == propertyName)
                {
                    return property.GetValue(instanceGetPropertyValue);
                }
            }
            return null;
        }
        
        /// <summary>
        /// lấy list tên trường insert vào db
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static List<string> GetNamePropertiesExceptPrimaryKeyInsertDb(Type type)
        {
            List<string> propertiesNameAddDb = new List<string>();
            PropertyInfo[] propertyInfos = type.GetProperties();
            foreach (PropertyInfo property in propertyInfos)
            {
                var nameProperty = property.Name;
                bool isValidProperty = false;
                IEnumerable<Attribute> attributesOfProperty = property.GetCustomAttributes();
                foreach (Attribute attribute in attributesOfProperty)
                {
                    if (attribute.GetType().Name != "PrimaryKey" && attribute.GetType().Name == "AddDatabase")
                    {
                        isValidProperty = true;
                        break;
                    }
                }
                if(isValidProperty)
                    propertiesNameAddDb.Add(property.Name);
            }

            return propertiesNameAddDb;
        }

        /// <summary>
        /// lấy list tên trường insert vào db
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static List<string> GetNamePropertiesInsertDb(Type type)
        {
            List<string> propertiesNameAddDb = new List<string>();
            PropertyInfo[] propertyInfos = type.GetProperties();
            foreach (PropertyInfo property in propertyInfos)
            {
                var nameProperty = property.Name;
                bool isValidProperty = false;
                IEnumerable<Attribute> attributesOfProperty = property.GetCustomAttributes();
                foreach (Attribute attribute in attributesOfProperty)
                {
                    if (attribute.GetType().Name == "PrimaryKey" || attribute.GetType().Name == "AddDatabase")
                    {
                        isValidProperty = true;
                        break;
                    }
                }
                if (isValidProperty)
                    propertiesNameAddDb.Add(property.Name);
            }

            return propertiesNameAddDb;
        }

        /// <summary>
        /// lấy param insert db trừ trường primarykey
        /// </summary>
        /// <param name="type"></param>
        /// <param name="recordInsert"></param>
        /// <returns></returns>
        public static Dictionary<string,object> GetPropertiesExceptPrimaryKeyInsertDb(Type type, object recordInsert)
        {
            Dictionary<string, object> propertiesAddDb = new Dictionary<string, object>();
            PropertyInfo[] propertyInfos = type.GetProperties();
            int typeProperty = -1;
            foreach (PropertyInfo property in propertyInfos)
            {
                var nameProperty = property.Name;
                IEnumerable<Attribute> attributesOfProperty = property.GetCustomAttributes();
                foreach (Attribute attribute in attributesOfProperty)
                {
                    if (attribute.GetType().Name != "PrimaryKey" && attribute.GetType().Name == "AddDatabase")
                    {
                        typeProperty = 1;
                        break;
                    }
                }
                if (typeProperty == 1)
                    propertiesAddDb.Add(property.Name, property.GetValue(recordInsert));
            }

            return propertiesAddDb;
        }

        /// <summary>
        /// lấy param insert db
        /// </summary>
        /// <param name="type"></param>
        /// <param name="recordInsert"></param>
        /// <returns></returns>
        public static Dictionary<string, object> GetPropertiesInsertDb(Type type, object recordInsert)
        {
            Dictionary<string, object> propertiesAddDb = new Dictionary<string, object>();
            PropertyInfo[] propertyInfos = type.GetProperties();
            foreach (PropertyInfo property in propertyInfos)
            {
                var nameProperty = property.Name;
                IEnumerable<Attribute> attributesOfProperty = property.GetCustomAttributes();
                int typeProperty = -1;
                foreach (Attribute attribute in attributesOfProperty)
                {
                    if (attribute.GetType().Name != "PrimaryKey" && attribute.GetType().Name == "AddDatabase")
                    {
                        typeProperty = 1;
                        break;
                    }
                    else if (attribute.GetType().Name == "PrimaryKey")
                    {
                        typeProperty = 2;
                        break;
                    }
                }

                if (typeProperty == 1)
                    propertiesAddDb.Add(property.Name, property.GetValue(recordInsert));
                else if (typeProperty == 2)
                {
                    object? valuePrimaryKey = property.GetValue(recordInsert);
                    if (valuePrimaryKey != null)
                    {
                        propertiesAddDb.Add(property.Name, valuePrimaryKey);
                    }
                    else
                        propertiesAddDb.Add(property.Name, Guid.NewGuid());
                    
                }
            }

            return propertiesAddDb;
        }
    }
}
