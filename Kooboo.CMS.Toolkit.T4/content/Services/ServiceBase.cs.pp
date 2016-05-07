using Kooboo.CMS.Content.Models;

namespace $RootNamespace$.Services
{
    public class ServiceBase<TEntity> : Kooboo.CMS.Toolkit.Services.ServiceBase<TEntity>
        where TEntity : TextContent, new()
    {
        private const string RepositoryName = ModuleAreaRegistration.ModuleName;

        public ServiceBase()
            : base(new Repository(RepositoryName))
        { }
    }
}