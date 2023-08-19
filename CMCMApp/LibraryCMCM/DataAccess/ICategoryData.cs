using LibraryCMCM.Models;

namespace LibraryCMCM.DataAccess
{
    public interface ICategoryData
    {
        Task<List<CategoryModel>> GetAllCategroies();
    }
}