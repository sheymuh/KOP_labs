using EmployeeApi.Data;
using EmployeeApi.Entities;

namespace EmployeeApi.Services;

public class EmployeePostSyncService(IHttpClientFactory httpClientFactory, IServiceScopeFactory scopeFactory) : BackgroundService
{
    private readonly IHttpClientFactory _httpClientFactory = httpClientFactory;
    private readonly IServiceScopeFactory _scopeFactory = scopeFactory;

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                using var scope = _scopeFactory.CreateScope();
                var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

                var client = _httpClientFactory.CreateClient("Service1Api");
                var remotePosts = await client.GetFromJsonAsync<List<EmployeePostReadDto>>("/api/employeepost?includeDeleted=true", cancellationToken: stoppingToken);

                if (remotePosts is not null)
                {
                    var remoteIds = remotePosts.Select(t => t.Id).ToHashSet();

                    foreach (var dto in remotePosts)
                    {
                        var post = await db.EmployeePostCache.FindAsync(dto.Id);
                        if (post is null)
                        {
                            post = new EmployeePost
                            {
                                Id = dto.Id,
                                Name = dto.Name,
                                IsDeprecated = dto.IsDeprecated
                            };
                            db.EmployeePostCache.Add(post);
                        }
                        else
                        {
                            post.Name = dto.Name;
                            post.IsDeprecated = dto.IsDeprecated;
                        }
                    }

                    // Удаляем записи, которых больше нет на стороне WebApi
                    var localPosts = db.EmployeePostCache.Where(t => !remoteIds.Contains(t.Id));
                    db.EmployeePostCache.RemoveRange(localPosts);

                    await db.SaveChangesAsync(stoppingToken);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка синхронизации: {ex.Message}");
            }

            await Task.Delay(TimeSpan.FromSeconds(5), stoppingToken);
        }
    }
}
