using System.Configuration;

namespace WebApplication1.Helping_Classes
{
    public enum EnumRole
    {
        Admin = 1,
        Manager = 2,
        Employee = 3,
    }

    public static class ProjectVariable
    {
        public readonly static string[] StorageContainers = {"candidateresume","userprofile"};

        public readonly static string AzureCloudStorageString = ConfigurationManager.AppSettings["AzureCloudStorageString"];
    }
}