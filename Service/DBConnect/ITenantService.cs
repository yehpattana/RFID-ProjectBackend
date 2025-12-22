namespace RFIDApi.Service.Tenant
{
    public interface ITenantService
    {
        string GetConnectionString(string company);
        string GetCompany();
    }
}
