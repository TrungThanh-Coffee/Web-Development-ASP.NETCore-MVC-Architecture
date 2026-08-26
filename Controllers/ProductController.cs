using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using project_cuoiky.Data;
using project_cuoiky.Models;

namespace project_cuoiky.Controllers
{
    public class ProductController : Controller
    {
        private readonly project_cuoikyContext _context;

        public ProductController(project_cuoikyContext context)
        {
            _context = context;
        }


        // =========================================================
        // HIỂN THỊ SẢN PHẨM
        //
        // Có categoryId  -> lọc theo Category
        // Không categoryId -> hiển thị tất cả sản phẩm
        // =========================================================
        public IActionResult Index(int? categoryId)
        {
            var query = _context.Products
                .Include(p => p.Category)
                .AsQueryable();


            // Nếu có categoryId thì lọc sản phẩm theo category
            if (categoryId.HasValue)
            {
                query = query.Where(
                    p => p.CategoryId == categoryId.Value
                );

                // Lấy tên Category để hiển thị tiêu đề
                var category = _context.Categories
                    .FirstOrDefault(
                        c => c.Id == categoryId.Value
                    );

                ViewBag.PageTitle =
                    category != null
                        ? category.Name
                        : "Products";
            }
            else
            {
                // Không truyền categoryId
                // => Hiển thị tất cả sản phẩm
                ViewBag.PageTitle = "All Products";
            }


            List<Product> productList =
                query.ToList();


            return View(productList);
        }


        // =========================================================
        // PRODUCT DETAIL
        // =========================================================
        public async Task<IActionResult> Detail(int id)
        {
            Product? product =
                await _context.Products
                    .Include(p => p.Category)
                    .FirstOrDefaultAsync(
                        p => p.Id == id
                    );


            if (product == null)
            {
                return NotFound();
            }


            return View(product);
        }
    }
}