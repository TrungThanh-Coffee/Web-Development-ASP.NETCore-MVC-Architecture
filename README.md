# Perfume Shop - ASP.NET Core MVC

Website bán nước hoa được xây dựng bằng **ASP.NET Core .NET 8**, phát triển trên **Microsoft Visual Studio 2022 (VS tím)**, sử dụng **Entity Framework Core** để kết nối **SQL Server**.

Project áp dụng kiến trúc **MVC** cho các chức năng chính như Home, Product và Cart; phần đăng ký/đăng nhập sử dụng **ASP.NET Core Identity + Razor Pages**. Giỏ hàng được lưu tạm bằng **Session** dưới dạng JSON.

---

## 1. Công nghệ sử dụng

### Ngôn ngữ

- **C#**: xử lý backend, model, controller, Entity Framework, Identity.
- **Razor / CSHTML**: tạo giao diện động phía server.
- **HTML / CSS**: xây dựng và định dạng giao diện.
- **JavaScript**: xử lý slider và tương tác giao diện.
- **SQL**: lưu trữ dữ liệu trên SQL Server.

### Công nghệ

- ASP.NET Core .NET 8
- ASP.NET Core MVC
- ASP.NET Core Identity
- Razor Pages
- Entity Framework Core
- Microsoft SQL Server
- Session State
- JSON Serialization
- Dependency Injection
- ViewComponent
- Partial View
- Entity Framework Migration
- MooTools / SlideItMoo

### Môi trường phát triển

- **Microsoft Visual Studio 2022 (VS tím)**
- .NET 8 SDK
- SQL Server / SQL Server Express
- SQL Server Management Studio (khuyến nghị)

---

## 2. Kiến trúc hệ thống

Project chủ yếu sử dụng kiến trúc **MVC - Model View Controller**.

```text
Người dùng
    |
    v
Routing
    |
    v
Controller
    |
    +----------------------+
    |                      |
    v                      v
Entity Framework         Session
    |                      |
    v                      v
SQL Server              CartItem
    |
    v
Model
    |
    v
View
    |
    v
HTML trả về trình duyệt
```

### Model

Các model chính:

```text
Product
Category
AppUser
Order
OrderDetail
CartItem
```

### View

Các giao diện chính:

```text
Views/Home/Index.cshtml
Views/Product/Index.cshtml
Views/Product/Detail.cshtml
Views/Cart/Index.cshtml
Views/Shared/_MainLayout.cshtml
Views/Shared/_LoginPartial.cshtml
Views/Shared/_CartPartial.cshtml
```

### Controller

```text
HomeController
ProductController
CartController
```

---

## 3. Cấu trúc database

Các quan hệ chính:

```text
Category 1 -------- N Product

AppUser  1 -------- N Order

Order    1 -------- N OrderDetail

Product  1 -------- N OrderDetail
```

`OrderDetail` là bảng trung gian giữa `Order` và `Product`.

### Các entity chính

**Product**

- Id
- Name
- Quantity
- Image
- Price
- Description
- CategoryId
- Promotion
- Volume
- Concentration
- FragranceFamily
- Longevity
- Sillage
- RecommendedTime

**Category**

- Id
- Name
- Description

**AppUser**

Kế thừa từ `IdentityUser` và bổ sung:

- FristName
- LastName
- Address
- Image

> Lưu ý: source hiện tại dùng tên `FristName`; về sau có thể refactor thành `FirstName`.

**Order**

- Id
- UserId
- OrderedDate
- Payment

**OrderDetail**

- Id
- OrderId
- ProductId
- Quantity
- Price

---

## 4. Entity Framework và DbContext

Project sử dụng:

```text
project_cuoikyContext
```

DbContext kế thừa:

```csharp
IdentityDbContext<AppUser>
```

Do đó cùng một DbContext quản lý:

- dữ liệu nghiệp vụ của shop;
- dữ liệu tài khoản ASP.NET Identity.

Ví dụ lấy Product kèm Category:

```csharp
_context.Products
    .Include(p => p.Category)
    .ToList();
```

`Include()` được dùng để **Eager Loading** dữ liệu Category liên quan.

---

## 5. Đặc tả chức năng

### 5.1 Trang chủ

- Lấy Product từ database.
- Hiển thị Featured Products.
- Hiển thị danh sách sản phẩm.
- Cho phép chuyển sang trang Product Detail.

Hiện tại:

```csharp
Model.Take(8)
```

được dùng cho Featured Products.

---

### 5.2 Danh mục sản phẩm

Category được lấy từ database bằng:

```text
CategoryViewComponent
```

Luồng:

```text
_MainLayout
    |
    v
CategoryViewComponent
    |
    v
DbContext.Categories
    |
    v
SQL Server
```

Khi chọn Category:

```text
CategoryId
   |
   v
ProductController.Index()
   |
   v
Lọc Product theo CategoryId
```

---

### 5.3 Chi tiết sản phẩm

Luồng:

```text
Chọn Product
    |
    v
Product Id
    |
    v
ProductController.Detail(id)
    |
    v
Query Product + Category
    |
    v
Detail View
```

Nếu không tìm thấy Product:

```text
HTTP 404 - Not Found
```

---

### 5.4 Đăng ký tài khoản

Sử dụng **ASP.NET Core Identity**.

Luồng:

```text
Register Form
    |
    v
InputModel
    |
    v
Validation
    |
    v
UserManager.CreateAsync()
    |
    v
Password Hash
    |
    v
AspNetUsers
    |
    v
SignInManager.SignInAsync()
```

Mật khẩu không được lưu trực tiếp mà được Identity lưu dưới dạng:

```text
PasswordHash
```

---

### 5.5 Đăng nhập / đăng xuất

Đăng nhập sử dụng:

```csharp
SignInManager.PasswordSignInAsync()
```

Đăng xuất sử dụng:

```csharp
SignInManager.SignOutAsync()
```

Authentication sử dụng Cookie để xác định user đã đăng nhập hay chưa.

---

### 5.6 Giỏ hàng

Giỏ hàng hiện tại **không lưu trong database**.

Dữ liệu Cart được lưu bằng:

```text
ASP.NET Session
```

và chuyển đổi qua JSON:

```text
List<CartItem>
      |
      v
Serialize
      |
      v
JSON
      |
      v
Session
```

Khi đọc:

```text
Session
   |
   v
JSON
   |
   v
Deserialize
   |
   v
List<CartItem>
```

Các chức năng:

- Add to Cart
- Update Quantity
- Remove Item
- Clear Cart
- Hiển thị tổng số lượng sản phẩm

---

## 6. Logic Add to Cart

```text
Product Detail
     |
     v
POST ProductId + Quantity
     |
     v
CartController.Add()
     |
     v
Load Product từ database
     |
     v
Kiểm tra Product và Stock
     |
     v
Tính giá Promotion
     |
     v
Đọc Cart từ Session
     |
     v
Thêm mới hoặc cộng số lượng
     |
     v
Serialize JSON
     |
     v
Lưu lại Session
```

### Vì sao không gửi Price từ client?

Client chỉ gửi:

```text
ProductId
Quantity
```

Price được lấy lại từ database để tránh việc người dùng sửa giá bằng DevTools hoặc gửi request giả.

---

## 7. Logic kiểm tra tồn kho

Dù giao diện đã giới hạn `max`, backend vẫn kiểm tra stock.

Ví dụ:

```text
Stock thực tế = 5
User gửi Quantity = 100
```

Server sẽ giới hạn quantity theo stock hiện tại.

Lý do:

> Validation phía client có thể bị bypass, nên nghiệp vụ quan trọng phải được kiểm tra lại phía server.

---

## 8. Logic tính giá khuyến mãi

Công thức:

```text
Giá sau giảm
=
Price - (Price × Promotion / 100)
```

Ví dụ:

```text
Price = 3.200.000
Promotion = 10%

Sale Price = 2.880.000
```

Giá được tính ở backend.

---

## 9. Logic đồng bộ Cart

Khi mở Cart, hệ thống query lại Product từ database để cập nhật:

- Name
- Image
- Price
- Promotion
- Stock

Các trường hợp:

```text
Product đã bị xóa
-> xóa khỏi Cart

Product hết hàng
-> xóa khỏi Cart

Quantity trong Cart > Stock
-> giảm Quantity về Stock hiện tại
```

Session không được xem là nguồn dữ liệu cuối cùng cho giá và tồn kho.

---

## 10. Session và Identity

Hai khái niệm này được dùng cho mục đích khác nhau.

### Identity

Dùng để xác định:

```text
User là ai?
User đã login chưa?
```

### Session

Dùng để lưu:

```text
Dữ liệu tạm của phiên làm việc
```

Trong project:

```text
Session -> Shopping Cart
```

Vì vậy user chưa đăng nhập vẫn có thể thêm sản phẩm vào giỏ.

---

## 11. ViewComponent và Partial View

### ViewComponent

Dùng cho:

```text
CategoryViewComponent
```

Có thể chứa logic riêng và query database.

### Partial View

Dùng cho:

```text
_LoginPartial
_CartPartial
```

Phù hợp để tái sử dụng các thành phần giao diện nhỏ.

---

## 12. Middleware chính

```text
Request
   |
   v
HTTPS
   |
   v
Static Files
   |
   v
Routing
   |
   v
Session
   |
   v
Authentication
   |
   v
Authorization
   |
   v
Controller / Razor Page
   |
   v
Response
```

---

## 13. Chức năng hiện tại chưa triển khai

Project hiện chưa hoàn thiện các chức năng:

- Checkout
- Payment
- Tạo Order từ Cart
- Tạo OrderDetail khi đặt hàng
- Order History
- Product CRUD hoàn chỉnh cho Admin
- Category CRUD
- Role-based Authorization
- Search backend hoàn chỉnh
- Persistent Cart trong database

`Order` và `OrderDetail` đã có trong database nhưng chưa có luồng checkout để ghi dữ liệu vào hai bảng này.

---

## 14. Cấu hình database

Connection string nằm trong:

```text
appsettings.json
```

Ví dụ:

```json
{
  "ConnectionStrings": {
    "project_cuoikyContextConnection": "Server=YOUR_SERVER;Database=PerfumeShop;Trusted_Connection=True;TrustServerCertificate=True"
  }
}
```

Thay `YOUR_SERVER` bằng SQL Server instance của máy.

---

## 15. Chạy project bằng Visual Studio

1. Mở project bằng **Microsoft Visual Studio 2022 (VS tím)**.
2. Kiểm tra connection string trong `appsettings.json`.
3. Mở **Package Manager Console**.
4. Nếu cần cập nhật database:

```powershell
Update-Database
```

5. Build Solution.
6. Nhấn:

```text
F5
```

để chạy Debug hoặc:

```text
Ctrl + F5
```

để chạy không Debug.

---

## 16. Tổng kết

Project thể hiện các kiến thức chính:

- ASP.NET Core MVC
- Entity Framework Core
- SQL Server
- ASP.NET Core Identity
- Razor Pages
- Dependency Injection
- Session
- JSON Serialization
- ViewComponent
- Partial View
- Server-side Validation
- Entity Relationship
- Entity Framework Migration

Các luồng chính của hệ thống gồm:

```text
Home
-> Product
-> Category
-> Product Detail
-> Cart

Register / Login
-> ASP.NET Identity

Cart
-> Session
-> JSON
-> Stock Validation
-> Promotion Logic
```
