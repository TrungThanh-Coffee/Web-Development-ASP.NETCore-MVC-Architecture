# project_cuoiky

## Commit: Cập nhật cấu trúc View và tài nguyên giao diện

### `feat(view): tách nội dung layout và bổ sung, chỉnh sửa tài nguyên giao diện`

Các thay đổi đã thực hiện:

- Tách phần `templatemo_content`, bao gồm cả `latest_product_gallery`, từ file `_MainLayout.cshtml` sang file `Index.cshtml`.
- Thêm thuộc tính `asp-append-version="true"` để trình duyệt nhận diện phiên bản CSS mới và tránh sử dụng file CSS cũ trong bộ nhớ cache.
- Thêm thư mục `assets` cùng các thư mục con và tài nguyên được cung cấp trong đề bài.
- Thêm file `templatemo_style.css` vào thư mục `css`.
- Thêm `@RenderBody()` bên dưới phần kết thúc của sidebar trong file `_MainLayout.cshtml`.
