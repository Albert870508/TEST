using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using TEST.Domain.Entities;

namespace TEST.Exercise.Application.Departments.Dto
{
    public class DepartmentDto
    {
        [Display(Name="编号")]
        [JsonConverter(typeof(IdToStringConverter))]
        public string Id { get; set; }
        [Display(Name = "部门名称")]
        public string Name { get; set; }
    }
}
