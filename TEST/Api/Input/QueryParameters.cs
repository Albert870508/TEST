using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Reflection;
using System.Text;

namespace TEST.Api.Input
{
    public class QueryParameters
    {
        private int pagesize;
        /// <summary>
        /// 每页多少条
        /// </summary>
        public int PageSize
        {
            get
            {
                if (pagesize <= 0)
                    pagesize = 5;
                return pagesize;
            }
            set
            {
                pagesize = value;
            }
        }

        /// <summary>
        /// 第几页从0开始
        /// </summary>
        public int PageIndex { get; set; }

        /// <summary>
        /// 搜素文本
        /// </summary>
        public string Search { get; set; }

        /// <summary>
        /// 升序还是降序
        /// 'descend' | 'ascend';
        /// </summary>
        public string SortType { get; set; }

        /// <summary>
        /// 根据哪个属性进行排序
        /// </summary>
        public string SortKey { get; set; }

        /// <summary>
        /// 筛选
        /// </summary>
        public List<Filter> Filter { get; set; } = new List<Filter>();

    }

    public class Filter
    {
        public string Key { get; set; }

        public List<string> Values { get; set; }
    }

    public static class QueryableExtensions
    {
        /// <summary>
        /// 过滤数据(返回经过 排序,筛选,以及分页处理后的数据)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source">全部数据源</param>
        /// <param name="queryParameters">查询参数</param>
        /// <returns></returns>
        public static IQueryable<T> GetPaginationData<T>(this IQueryable<T> source, QueryParameters queryParameters)
        {

            IQueryable<T> ts = source;

            #region 按照某字段排序
            if (!string.IsNullOrEmpty(queryParameters.SortKey) && ColumnsContainKey<T>(queryParameters.SortKey, out string sortKeyDataType)) //如果排序关键字不为空并且在实体类中存在,则进行排序
            {
                string completeSortExpression = (!string.IsNullOrEmpty(queryParameters.SortType) && queryParameters.SortType.ToLower() == "descend") ? queryParameters.SortKey + " descending" : queryParameters.SortKey;
                ts = ts.OrderBy(completeSortExpression);
            }
            #endregion

            #region 按照某个字段筛选
            if (queryParameters.Filter.Count > 0)
            {
                StringBuilder strWhere = new StringBuilder();
                foreach (Filter filter in queryParameters.Filter)
                {
                    if (!string.IsNullOrEmpty(filter.Key) && ColumnsContainKey<T>(filter.Key, out string filterKeyDataType))//如果筛选关键字不为空并且在实体类中存在,则进行筛选
                    {
                        string valuesStr = string.Join(",", filter.Values.ToArray());
                        if (filterKeyDataType != "Int64")
                        {
                            valuesStr = string.Format("\"{0}\"", valuesStr.Replace(",", "\",\""));
                        }
                        strWhere.AppendFormat("(" + filter.Key + " in ({0})) and ", valuesStr);
                    }
                }
                strWhere.Append("1>0");

                if (!string.IsNullOrWhiteSpace(strWhere.ToString()))
                {
                    ts = ts.Where(strWhere.ToString());
                }
            }

            #endregion

            #region 按照PageIndex和PageSize获取指定页数据
            ts = ts.Skip(queryParameters.PageIndex * queryParameters.PageSize)
                       .Take(queryParameters.PageSize);
            #endregion


            return ts;
        }

        /// <summary>
        /// 判断关键字是否在实体类中存在
        /// </summary>
        /// <typeparam name="T">要检验的实体类</typeparam>
        /// <param name="key">关键字(排序关键字或者筛选关键字)</param>
        /// <param name="keyType">返回该关键字在实体类中的数据类型</param>
        /// <returns></returns>
        private static bool ColumnsContainKey<T>(string key, out string keyDataType)
        {
            var Types = typeof(T);//获得类型
            //List<string> columns = new List<string>();
            Dictionary<string, string> columns = new Dictionary<string, string>();
            foreach (PropertyInfo sp in Types.GetProperties())
            {
                columns.Add(sp.Name.ToLower(), sp.PropertyType.Name);
            }
            if (columns.Keys.Contains(key.ToLower()))
            {
                keyDataType = columns[key.ToLower()].ToString();
                return true;
            }
            else
            {
                keyDataType = string.Empty;
                return false;
            }
        }

        #region 多字段排序,暂时用不到,以后扩展使用
        /// <summary>
        /// 动态排序,支持多字段排序,多个字段用逗号分隔(比如按姓名和年龄排序[Name,Age])
        /// </summary>
        /// <typeparam name="T">排序返回的实体类</typeparam>
        /// <param name="source">数据源</param>
        /// <param name="sortKey">排序关键字(Id/Name/Sex等)</param>
        /// <param name="sortType">排序类型(descend降序/ascend升序)</param>
        /// <returns></returns>
        private static IQueryable<T> ApplySort<T>(this IQueryable<T> source, string sortKey, string sortType)
        {
            if (source == null)
            {
                throw new ArgumentNullException("source");
            }

            if (sortKey == null)
            {
                return source;
            }

            //把以逗号分隔的排序字符串放数组中
            var listSort = sortKey.Split(',');

            string completeSortExpression = "";
            foreach (var sortOption in listSort)
            {
                //如果排序字段以-开头就降序，否则升序
                if (sortType == "descend")
                {
                    completeSortExpression = sortOption + " descending,";
                }
                else
                {
                    completeSortExpression = completeSortExpression + sortOption + ",";
                }
            }

            if (!string.IsNullOrWhiteSpace(completeSortExpression))
            {
                source = source.OrderBy(completeSortExpression
                    .Remove(completeSortExpression.Count() - 1));
            }

            return source;
        }
        #endregion
    }
}
