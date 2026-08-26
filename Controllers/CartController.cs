using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using project_cuoiky.Data;
using project_cuoiky.Models;
using System.Text.Json;

namespace project_cuoiky.Controllers
{
    public class CartController : Controller
    {
        private readonly project_cuoikyContext _context;

        // Key dùng để lưu Cart trong Session
        public const string CARTKEY = "CART";


        public CartController(project_cuoikyContext context)
        {
            _context = context;
        }


        // =========================================================
        // LẤY GIỎ HÀNG TỪ SESSION
        // =========================================================
        private List<CartItem> GetCartItems()
        {
            string? jsonCart =
                HttpContext.Session.GetString(CARTKEY);

            if (string.IsNullOrEmpty(jsonCart))
            {
                return new List<CartItem>();
            }

            return JsonSerializer
                       .Deserialize<List<CartItem>>(jsonCart)
                   ?? new List<CartItem>();
        }


        // =========================================================
        // LƯU GIỎ HÀNG VÀO SESSION
        // =========================================================
        private void SaveCartSession(List<CartItem> cart)
        {
            string jsonCart =
                JsonSerializer.Serialize(cart);

            HttpContext.Session.SetString(
                CARTKEY,
                jsonCart
            );
        }


        // =========================================================
        // XÓA TOÀN BỘ CART KHỎI SESSION
        // =========================================================
        private void ClearCartSession()
        {
            HttpContext.Session.Remove(CARTKEY);
        }


        // =========================================================
        // HIỂN THỊ GIỎ HÀNG
        // GET: /Cart
        // =========================================================
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            List<CartItem> cart =
                GetCartItems();


            // Không có sản phẩm thì trả view luôn
            if (cart.Count == 0)
            {
                return View(cart);
            }


            // Lấy danh sách Id sản phẩm hiện đang có trong Cart
            List<int> productIds =
                cart
                    .Select(x => x.ProductId)
                    .ToList();


            // Đọc lại sản phẩm từ Database
            // để Cart luôn có giá và tồn kho mới nhất
            List<Product> products =
                await _context.Products
                    .Where(p => productIds.Contains(p.Id))
                    .ToListAsync();


            // Duyệt copy để có thể remove
            foreach (CartItem item in cart.ToList())
            {
                Product? product =
                    products.FirstOrDefault(
                        p => p.Id == item.ProductId
                    );


                // Sản phẩm đã bị xóa khỏi database
                if (product == null)
                {
                    cart.Remove(item);

                    continue;
                }


                // Sản phẩm hết hàng
                if (product.Quantity <= 0)
                {
                    cart.Remove(item);

                    continue;
                }


                // =========================
                // CẬP NHẬT GIÁ
                // =========================
                decimal salePrice =
                    GetSalePrice(product);


                item.Name = product.Name;

                item.Image = product.Image;

                item.Price = salePrice;

                item.Stock = product.Quantity;


                // Nếu Cart đang có số lượng lớn hơn tồn kho
                // tự động giảm về tồn kho tối đa
                if (item.Quantity > product.Quantity)
                {
                    item.Quantity =
                        product.Quantity;
                }
            }


            // Lưu lại cart đã đồng bộ
            SaveCartSession(cart);


            return View(cart);
        }


        // =========================================================
        // THÊM SẢN PHẨM VÀO CART
        // =========================================================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Add(
            int productId,
            int quantity = 1)
        {
            Product? product =
                await _context.Products
                    .FirstOrDefaultAsync(
                        p => p.Id == productId
                    );


            if (product == null)
            {
                return NotFound(
                    "Product not found."
                );
            }


            // Không cho thêm sản phẩm hết hàng
            if (product.Quantity <= 0)
            {
                TempData["CartError"] =
                    "This product is out of stock.";

                return RedirectToAction(
                    "Detail",
                    "Product",
                    new { id = productId }
                );
            }


            // Quantity ít nhất phải là 1
            if (quantity < 1)
            {
                quantity = 1;
            }


            // Không được vượt tồn kho
            if (quantity > product.Quantity)
            {
                quantity =
                    product.Quantity;
            }


            List<CartItem> cart =
                GetCartItems();


            CartItem? cartItem =
                cart.FirstOrDefault(
                    x => x.ProductId == productId
                );


            // =========================
            // SẢN PHẨM ĐÃ CÓ TRONG CART
            // =========================
            if (cartItem != null)
            {
                int newQuantity =
                    cartItem.Quantity + quantity;


                // Không vượt số lượng trong kho
                if (newQuantity > product.Quantity)
                {
                    newQuantity =
                        product.Quantity;
                }


                cartItem.Quantity =
                    newQuantity;

                cartItem.Stock =
                    product.Quantity;

                cartItem.Price =
                    GetSalePrice(product);
            }


            // =========================
            // SẢN PHẨM CHƯA CÓ
            // =========================
            else
            {
                cart.Add(
                    new CartItem
                    {
                        ProductId = product.Id,

                        Name = product.Name,

                        Image = product.Image,

                        Price = GetSalePrice(product),

                        Quantity = quantity,

                        Stock = product.Quantity
                    }
                );
            }


            SaveCartSession(cart);


            TempData["CartMessage"] =
                "Product added to cart successfully.";


            // Thêm xong mở trang giỏ hàng
            return RedirectToAction(
                nameof(Index)
            );
        }


        // =========================================================
        // CẬP NHẬT SỐ LƯỢNG
        // =========================================================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(
            int productId,
            int quantity)
        {
            List<CartItem> cart =
                GetCartItems();


            CartItem? cartItem =
                cart.FirstOrDefault(
                    x => x.ProductId == productId
                );


            if (cartItem == null)
            {
                return RedirectToAction(
                    nameof(Index)
                );
            }


            // Quantity <= 0 thì xem như xóa
            if (quantity <= 0)
            {
                cart.Remove(cartItem);

                SaveCartSession(cart);

                return RedirectToAction(
                    nameof(Index)
                );
            }


            Product? product =
                await _context.Products
                    .FirstOrDefaultAsync(
                        p => p.Id == productId
                    );


            // Product không còn tồn tại
            if (product == null)
            {
                cart.Remove(cartItem);

                SaveCartSession(cart);

                return RedirectToAction(
                    nameof(Index)
                );
            }


            // Product hết hàng
            if (product.Quantity <= 0)
            {
                cart.Remove(cartItem);

                SaveCartSession(cart);

                TempData["CartError"] =
                    "The product is currently out of stock.";

                return RedirectToAction(
                    nameof(Index)
                );
            }


            // Không được đặt nhiều hơn số lượng tồn kho
            if (quantity > product.Quantity)
            {
                quantity =
                    product.Quantity;

                TempData["CartError"] =
                    $"Only {product.Quantity} product(s) are available.";
            }


            cartItem.Quantity =
                quantity;

            cartItem.Stock =
                product.Quantity;

            cartItem.Price =
                GetSalePrice(product);


            SaveCartSession(cart);


            return RedirectToAction(
                nameof(Index)
            );
        }


        // =========================================================
        // XÓA MỘT SẢN PHẨM
        // =========================================================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Remove(
            int productId)
        {
            List<CartItem> cart =
                GetCartItems();


            CartItem? cartItem =
                cart.FirstOrDefault(
                    x => x.ProductId == productId
                );


            if (cartItem != null)
            {
                cart.Remove(cartItem);

                SaveCartSession(cart);
            }


            return RedirectToAction(
                nameof(Index)
            );
        }


        // =========================================================
        // XÓA TOÀN BỘ GIỎ HÀNG
        // =========================================================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Clear()
        {
            ClearCartSession();


            return RedirectToAction(
                nameof(Index)
            );
        }


        // =========================================================
        // TÍNH GIÁ SAU KHUYẾN MÃI
        // =========================================================
        private decimal GetSalePrice(
            Product product)
        {
            if (product.Promotion > 0)
            {
                return product.Price
                       - (
                           product.Price
                           * product.Promotion
                           / 100
                       );
            }


            return product.Price;
        }
    }
}