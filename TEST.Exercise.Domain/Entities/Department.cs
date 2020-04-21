using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using TEST.Domain.Entities;

namespace TEST.Exercise.Domain.Entities
{
    /// <summary>
    /// 部门表
    /// </summary>
    public class Department : Entity
    {
        /// <summary>
        /// 部门信息
        /// </summary>
        [Display(Name="部门名称")]
        public string Name { get; set; }
    }
}
