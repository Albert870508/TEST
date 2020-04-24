using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using TEST.Domain.Entities;

namespace TEST.Exercise.Domain.Entities
{
    /// <summary>
    /// 用户表（根据UserName PhoneNumber DepartmentId 判断用户资料是否完善）
    /// </summary>
    public class User : Entity
    {
        /// <summary>
        /// 用户名称
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// 手机号
        /// </summary>
        public string PhoneNumber { get; set; }

        /// <summary>
        /// 微信ID
        /// </summary>
        public string WeChatOpenId { get; set; }


        /// <summary>
        /// 部门ID
        /// </summary>        
        public string Department { get; set; }
    }
}
