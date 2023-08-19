
using LibraryCMCM.Models;

namespace LibraryCMCM.DataAccess;

public class CategoryData : ICategoryData
{
    private readonly ISqlDataAccess _sql;

    public CategoryData(ISqlDataAccess sql)
    {
        _sql = sql;
    }

    // dbo.spGetAllCategories
    public async Task<List<CategoryModel>> GetAllCategroies()
    {
        var result = await _sql.LoadData<CategoryModel, dynamic>("dbo.spGetAllCategories", null!, "Default");
        return result;
    }
}
