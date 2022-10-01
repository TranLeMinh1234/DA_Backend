using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ClassModel.Query.SQLBuilder
{
    public class ConditionSqlBuilder
    {
        public int Skip { get; set; } = -1;
        public int Take { get; set; } = -1;
        public string TableName { get; set; }
        public string Columns { get; set; }
        public List<KeyValuePair<string, string>> Sort { get; set; } = null;
        public List<ConditionQuery> ListCondition { get; set; } = null;
        public DateTime? FromDate { get; set; } = null;
        public DateTime? ToDate { get; set; } = null;
        public Dictionary<string, object> ParamCondition { get; set; }
        public string StringWhereCondition { get; set; } = null;



        public string BuildPagingSql()
        {
            StringBuilder sql = new StringBuilder();
            sql.Append("SELECT ");
            if (string.IsNullOrEmpty(Columns))
            {
                sql.Append(" * ");
            }

            sql.Append($"FROM {TableName} WHERE {BuildWhereCondition()}");

            if (Sort != null && Sort.Count > 0)
            {
                sql.Append("LIMIT ");
                foreach (KeyValuePair<string, string> keyValuePair in Sort)
                {
                    sql.Append($" {keyValuePair.Key} {keyValuePair.Value},");
                }
                sql.Remove(sql.Length - 1, 1);
            }

            return sql.ToString();
        }

        /// <summary>
        /// Build dieu kien Where
        /// </summary>
        /// <returns></returns>
        public string BuildWhereCondition()
        {
            StringBuilder stringCondition = new StringBuilder();
            stringCondition.Append("1 = 1");
            if (ListCondition != null && ListCondition.Count > 0)
            {
                for (int i = 0; i < ListCondition.Count; i++)
                {
                    var conditionQuery = ListCondition.ElementAt(i);
                    if (conditionQuery.GroupNumber == null)
                    {
                        stringCondition.Append($" {conditionQuery.Operator} {conditionQuery.ColumnConition} {conditionQuery.TypeCondition} @{conditionQuery.ColumnConition}");
                    }
                    // Truong hop group dieu kien xu li sau
                }
            }

            this.StringWhereCondition = stringCondition.ToString();

            return stringCondition.ToString();
        }

        public Dictionary<string, object> BuildParamCondition()
        {
            Dictionary<string, object> paramCondition = new Dictionary<string, object>();
            if (ListCondition != null && ListCondition.Count > 0)
            {
                foreach (ConditionQuery conditionQuery in ListCondition)
                {
                    paramCondition.Add(conditionQuery.ColumnConition, conditionQuery.Value);
                }
            }

            this.ParamCondition = paramCondition;

            return paramCondition;
        }

        public class ConditionQuery
        {
            public int? GroupNumber { get; set; } = null;
            public string OperatorGroup { get; set; } = null;
            public string Operator { get; set; }
            public string TypeCondition { get; set; }
            public object Value { get; set; }
            public string ColumnConition { get; set; }
            public List<Object> ListConditionIn { get; set; } = null;
        }
    }
}
   
