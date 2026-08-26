using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using project_cuoiky.Data;
using project_cuoiky.Models;


namespace project_cuoiky.Components
{
    public class CategoryViewComponent : ViewComponent
    {
        public readonly project_cuoikyContext _context;

        public CategoryViewComponent(project_cuoikyContext context)
        {
           this._context = context;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            List<Category> list = await _context.Categories.ToListAsync();
            return View(list);
        }
    }
}