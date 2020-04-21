using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using TEST.Exercise.Domain.Entities;

namespace TEST.EntityFrameworkCore
{
    public static class DatabaseInitializer
    {
        public static List<Administrator> administrators;
        public static void Initialize(IServiceProvider applicationServices)
        {
            using (var serviceScope = applicationServices.CreateScope())
            {
                using (var db = new TESTDbContext(serviceScope.ServiceProvider.GetRequiredService<DbContextOptions<TESTDbContext>>()))
                {
                    //创建默认管理员
                    if (!db.Administrators.Any(m=>true))
                    {
                        db.Administrators.Add(new Exercise.Domain.Entities.Administrator()
                        {
                            Name = "Administrator",
                            Password = Helper.StringTools.MD5Encrypt32("123456")
                        });
                    }
                    //初始化题目类型表
                    if (!db.QuestionTypes.Any(m=>true))
                    {
                        InitializeQuestionTypes(db);
                    }
                    if (!db.Departments.Any(m => true))
                    {
                        InitializeDepartments(db);
                    }
                    administrators = db.Administrators.ToList();

                    db.SaveChanges();
                }
            }
        }

        /// <summary>
        /// 初始化题目类型表
        /// </summary>
        /// <param name="db"></param>
        private static void InitializeQuestionTypes(TESTDbContext db)
        {
            List<QuestionType> questionTypes = new List<QuestionType>() {
                new QuestionType(){ Name="单选", Score=2.0, Number=10 },
                new QuestionType(){ Name="多选", Score=2.0, Number=10 },
                new QuestionType(){ Name="判断", Score=2.0, Number=10 }
            };
            db.QuestionTypes.AddRange(questionTypes);
        }

        private static void InitializeDepartments(TESTDbContext db)
        {
            string departmentFile = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Department.txt");
            List<Department> departments = new List<Department>();
            foreach (string item in File.ReadAllLines(departmentFile, Encoding.GetEncoding("gb2312")))
            {
                if (!String.IsNullOrEmpty(item.Trim()))
                {
                    Department department = new Department();
                    department.Name = item;
                    departments.Add(department);
                }                
            }
            db.Departments.AddRange(departments);
        }
    }
}
