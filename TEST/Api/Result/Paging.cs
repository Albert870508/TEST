using System;
using System.Collections.Generic;
using System.Text;

namespace TEST.Api.Result
{
    /// <summary>
    /// 分页的时候要返回的内容
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class Paging<T>
    {
        /// <summary>
        /// 数据总数
        /// </summary>
        public int Total { get; set; }

        /// <summary>
        /// 当前页面数据
        /// </summary>
        public T Data { get; set; }
    }
}
