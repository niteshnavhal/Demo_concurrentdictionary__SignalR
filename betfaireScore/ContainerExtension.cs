using betfaireScore.Services;
using BusinessServices.Implementation;
using BusinessServices.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Repository.Implementation;
using Repository.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace betfaireScore
{
    public static class ContainerExtension
    {
        public static void ConfigureDI(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddSingleton<getScore>();
            services.AddSingleton<IConfiguration>(configuration);

            services.AddTransient<IScoreServices, ScoreServices>();
            services.AddTransient<IbetfairScoreServices, betfairScoreServices>();
            services.AddSingleton<IScoreListServices, ScoreListServices>();

            services.AddTransient<IDbContext, DbContext>();
            services.AddTransient<ICommentaryContext, CommentaryContext>();
            services.AddTransient<ICommentaryContext_New, CommentaryContext_New>();
            services.AddTransient<ICommentaryRepository, CommentaryRepository>();
        }
    }
}
