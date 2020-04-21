using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TEST.Management.Models
{
    public class LoginViewModel
    {
        // 用户名
        [Display(Name = "用户名")]
        [Required]
        public string UserName { get; set; }

        // 密码
        [Display(Name = "密码")]
        [Required]
        [StringLength(20, MinimumLength = 6,ErrorMessage ="密码长度应大于6个字符小于20个字符")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
