using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Data;

public abstract class DAL
{
    public Database MRT001 { get; set; }
    public DAL()
    {
        SystemConfigurationSource systemSource = new SystemConfigurationSource();
        DatabaseProviderFactory factory = new DatabaseProviderFactory(systemSource);
        MRT001 = factory.CreateDefault();
    }
}