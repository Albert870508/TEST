using System;
using System.Collections.Generic;
using System.Text;
using TEST.Domain.Entities;

namespace TEST.Exercise.Domain.Entities
{
    /// <summary>
    /// 管理员表
    /// </summary>
   public class Administrator:Entity
   {
        /// <summary>
        /// 用户名
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 密码
        /// </summary>
        public string Password { get; set; }
        /// <summary>
        /// 邮箱，用来找回密码
        /// </summary>
        public string EmailAddress { get; set; }
    }
}
