using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using TEST.Exercise.Domain.Entities;

namespace TEST.EntityFrameworkCore
{
    public class TESTDbContext : DbContext
    {
        /// <summary>
        /// 题目类型表
        /// </summary>
        public DbSet<QuestionType> QuestionTypes { get; set; }

        /// <summary>
        /// 题目表
        /// </summary>
        public DbSet<Question> Questions { get; set; }

        /// <summary>
        /// 用户表
        /// </summary>
        public DbSet<User> Users { get; set; }

        /// <summary>
        /// 成绩表
        /// </summary>
        public DbSet<Score> Scores { get; set; }
        
        /// <summary>
        /// 后台管理员表
        /// </summary>
        public DbSet<Administrator> Administrators { get; set; }
        /// <summary>
        /// 部门表
        /// </summary>
        public DbSet<Department> Departments { get; set; }
        /// <summary>
        /// 考试表
        /// </summary>
        public DbSet<Examination> Examinations { get; set; }

        /// <summary>
        /// 答题记录表
        /// </summary>
        public DbSet<AnswerRecord> AnswerRecords { get; set; }

        public TESTDbContext(DbContextOptions<TESTDbContext> options)
            : base(options)
        {
            // EnsureCreated()的作用是，如果有数据库存在，那么什么也不会发生。
            // 但是如果没有，那么就会创建一个数据库。
            Database.EnsureCreated();
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);            
        }
    }
}
