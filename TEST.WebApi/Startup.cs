using System.Text.Encodings.Web;
using System.Text.Unicode;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using TEST.EntityFrameworkCore;
using TEST.Exercise.Application;

namespace TEST.WebApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            #region 数据库配置
            var ConnectionString = Configuration["ConnectionStrings:Default"];
            services.AddDbContext<TESTDbContext>(options =>
            {
                DbContextOptionsConfigurer.Configure(options, ConnectionString);
            });
            #endregion

            // 注入Test
            services.AddTest<TESTDbContext>();
            services.AddTestExercise();

            // 注册Swagger生成器,定义一个或多个Swagger文档
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo { Title = "Test API", Version = "v1" });
            });

            services.AddWebEncoders(opt =>
            {
                opt.TextEncoderSettings = new TextEncoderSettings(UnicodeRanges.All);
            });

            // 注入MVC
            services.AddMvc(option=>option.EnableEndpointRouting=false).SetCompatibilityVersion(CompatibilityVersion.Version_3_0);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();           

            app.UseMvc();

            //启用中间件服务生成Swagger作为JSON终结点
            app.UseSwagger();
            //启用中间件服务对swagger-ui，指定Swagger JSON终结点
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "TEST API V1");
            });


            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
