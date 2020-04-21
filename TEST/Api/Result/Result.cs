using System;
using System.Collections.Generic;
using System.Text;

namespace TEST.Api.Result
{
    /// <summary>
    /// API返回参数
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class Result<T>
    {
        /// <summary>
        /// 数据
        /// </summary>
        public T Data { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public Boolean IsSuccess { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public String Msg { get; set; }


        /// <summary>失败
        /// </summary>
        /// <param name="msg">定义失败信息</param>
        /// <returns></returns>
        public static Result<T> Fail(String msg)
        {
            return new Result<T>
            {
                IsSuccess = false,
                Msg = msg
            };
        }
        /// <summary>
        /// 失败 
        /// </summary>
        /// <param name="data">要返回的数据泛型</param>
        /// <param name="msg">定义失败信息</param>
        /// <returns></returns>
        public static Result<T> Fail(T data, String msg)
        {
            return new Result<T>
            {
                IsSuccess = false,
                Msg = msg,
                Data = data
            };
        }

        /// <summary>
        /// 成功 
        /// </summary>
        /// <param name="data">要返回的数据泛型</param>
        /// <param name="msg">可定义成功信息</param>
        /// <returns></returns>
        public static Result<T> Success(T data, String msg)
        {
            return new Result<T>
            {
                IsSuccess = true,
                Msg = msg,
                Data = data
            };
        }

        /// <summary>
        /// 成功 
        /// </summary>
        /// <param name="data">要返回的数据泛型</param>
        /// <returns></returns>
        public static Result<T> Success(T data)
        {
            return Success(data, string.Empty);
        }
    }
}
