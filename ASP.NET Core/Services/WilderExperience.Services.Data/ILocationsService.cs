namespace WilderExperience.Services.Data
{
    using System.Threading.Tasks;

    public interface ILocationsService
    {
        int GetIdByName(string name);

        string GetNameById(int? id);
    }
}
