using Microsoft.Extensions.DependencyInjection;
using TEST.Exercise.Application.Admin;
using TEST.Exercise.Application.AnswerRecords;
using TEST.Exercise.Application.Departments;
using TEST.Exercise.Application.Examinations;
using TEST.Exercise.Application.Exercises;
using TEST.Exercise.Application.ExerciseType;
using TEST.Exercise.Application.Scores;
using TEST.Exercise.Application.Users;

namespace TEST.Exercise.Application
{
    public static class TESTExerciseApplicationServiceCollectionExtensions
    {
        public static IServiceCollection AddTestExercise(this IServiceCollection services)
        {
            services.AddScoped<IAdminService, AdminService>();
            services.AddScoped<IDepartmentService, DepartmentService>();
            services.AddScoped<IExaminationService, ExaminationService>();
            services.AddScoped<IExerciseService, ExerciseService>();
            services.AddScoped<IExerciseTypeService, ExerciseTypeService>();
            services.AddScoped<IScoreService, ScoreService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IAnswerRecordService, AnswerRecordService>();
            return services;
        }
    }
}
