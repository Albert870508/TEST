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
            #region ���ݿ�����
            var ConnectionString = Configuration["ConnectionStrings:Default"];
            services.AddDbContext<TESTDbContext>(options =>
            {
                DbContextOptionsConfigurer.Configure(options, ConnectionString);
            });
            #endregion

            // ע��Test
            services.AddTest<TESTDbContext>();
            services.AddTestExercise();

            // ע��Swagger������,����һ������Swagger�ĵ�
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo { Title = "Test API", Version = "v1" });
            });

            services.AddWebEncoders(opt =>
            {
                opt.TextEncoderSettings = new TextEncoderSettings(UnicodeRanges.All);
            });

            // ע��MVC
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

            //�����м����������Swagger��ΪJSON�ս��
            app.UseSwagger();
            //�����м�������swagger-ui��ָ��Swagger JSON�ս��
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
