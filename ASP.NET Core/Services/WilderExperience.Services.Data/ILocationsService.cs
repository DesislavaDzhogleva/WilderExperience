namespace WilderExperience.Services.Data
{
    using System.Threading.Tasks;

    using WilderExperience.Web.ViewModels.Locations;

    public interface ILocationsService
    {
        int GetIdByName(string name);

        string GetNameById(int? id);

    }
}
