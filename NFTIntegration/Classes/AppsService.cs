using NFTIntegration.Data;
using NFTIntegration.Models;
using System.Threading.Tasks;

namespace NFTIntegration.Classes
{
    public interface IAppsService
    {
        Task CreateProject(CreateProjectModel model);
    }

    public class AppsService : IAppsService
    {
        public AppsService()
        { }

        public async Task CreateProject(CreateProjectModel model)
        {
            await Task.Run(() => new DataAdapter().CreateProject(model)).ConfigureAwait(false);
        }
    } 
}
