using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Mock.Application.Databases;

namespace Mock.Application.Services;

public class HostedDbMigrationService : IHostedService
{
    private readonly IServiceScopeFactory _serviceScopeFactory;

    public HostedDbMigrationService(IServiceScopeFactory serviceScopeFactory)
    {
        _serviceScopeFactory = serviceScopeFactory;
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        using var scope = _serviceScopeFactory.CreateScope();
        var context = scope.ServiceProvider.GetService<MockContext>();
        if (context != null)
        {
            await context.Database.EnsureCreatedAsync(cancellationToken);
        }
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}