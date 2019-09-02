using System.Threading;
using System.Threading.Tasks;

namespace AwesomeSauceApi.HostedServices
{
    public interface IHostedService
    {
        Task StartAsync(CancellationToken cancellationToken);
        Task StopAsync(CancellationToken cancellationToken);
    }
}
