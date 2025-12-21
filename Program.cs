var builder = WebApplication.CreateBuilder(args);

// 啟用 CORS
builder.Services.AddCors(options =>
{
    // 策略名稱：可以自定義
    options.AddPolicy("賴又德上課不要滑手機Policy",
        policy =>
        {
            // 允許的來源 (Origin)
            policy.AllowAnyOrigin() // 允許所有來源 (用於開發或開放 API)
                                    // 或精確指定：.WithOrigins("http://localhost:3000", "https://yourfrontend.com")
                                    // 允許的 HTTP 方法
                  .AllowAnyMethod() // 允許 GET, POST, PUT, DELETE 等
                                    // 允許的 HTTP 標頭
                  .AllowAnyHeader(); // 允許所有標頭 (Content-Type, Authorization 等)
        });

    // 您可以在這裡定義更多策略，例如 "StrictPolicy"
});


builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// 啟用 CORS
app.UseCors();

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
