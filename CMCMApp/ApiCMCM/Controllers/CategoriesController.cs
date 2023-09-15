using LibraryCMCM.DataAccess;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ApiCMCM.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CategoriesController : ControllerBase
{
    private readonly ICategoryData _categoryData;

    public CategoriesController(ICategoryData categoryData)
    {
        _categoryData = categoryData;
    }

    #region GET
    [HttpGet]
    public async Task<IActionResult> GetAllCategories()
    {
        var result = await _categoryData.GetAllCategroies();
        return Ok(result);
    }
    #endregion
}
