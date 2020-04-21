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
        public string UserId { get; set; }
        /// <summary>
        /// 返回Token
        /// </summary>
        public string Token { get; set; }
    }

    public class UserInput 
    {
        public string UserName { get; set; }
        public string UserPhone { get; set; }
        public string UserDempartment { get; set; }
    }
}
