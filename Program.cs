using Amazon.Rekognition;
using Amazon.S3;
using UserRekongition.Model;
using UserRekongition.Services;
using UserRekongition.Services.IServices;

namespace UserRekongition
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddDefaultAWSOptions(builder.Configuration.GetAWSOptions());
            builder.Services.AddAWSService<IAmazonS3>();
            builder.Services.AddAWSService<IAmazonRekognition>();
            builder.Services.AddSingleton<Logger>(new Logger("E:\\DoAn\\Log.txt"));
            builder.Services.AddSingleton<ICollectionService, CollectionService>();
            builder.Services.AddSingleton<IS3Service, S3Service>();
            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
