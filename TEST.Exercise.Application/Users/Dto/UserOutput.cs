using System;
using System.Collections.Generic;
using System.Text;

namespace TEST.Exercise.Application.Users.Dto
{
    /// <summary>
    /// 用户登陆后返回的数据
    /// </summary>
    public class LoginOutput
    {
        /// <summary>
        /// 资料是否完善
        /// </summary>
        public bool Perfect { get; set; }
        /// <summary>
        /// 当前用户ID
        /// </summary>
        public string UserName { get; set; }
        /// <summary>
        /// 返回Token
        /// </summary>
        public string Token { get; set; }

        /// <summary>
        /// 用户手机号
        /// </summary>
        public string PhoneNumber { get; set; }

        /// <summary>
        /// 用户所属部门
        /// </summary>
        public string Dempartment { get; set; }
    }

    public class UserInput 
    {
        public string UserName { get; set; }
        public string UserPhone { get; set; }
        public string UserDempartment { get; set; }
    }
}
