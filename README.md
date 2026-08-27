# Automated_Web_Testing_Katalon_Studio
Dự án kiểm thử tự động trên nền web sử dụng Katalon Studio.
# KIỂM THỬ TỰ ĐỘNG TRÊN NỀN WEB BẰNG KATALON STUDIO

## Giới thiệu dự án

Đây là dự án thực hành chuyên ngành của Nhóm 2 với chủ đề **Kiểm thử tự động trên nền web bằng Katalon Studio**.

Dự án nhằm tìm hiểu và áp dụng các kiến thức về kiểm thử phần mềm tự động vào việc kiểm thử các chức năng của một ứng dụng website. Nhóm sử dụng **Katalon Studio** để xây dựng, thực hiện và đánh giá các Test Case.

Thông qua dự án, nhóm hướng tới việc tự động hóa quá trình kiểm thử, giảm thời gian thực hiện các thao tác kiểm thử thủ công và nâng cao khả năng phát hiện lỗi của hệ thống.

## Mục tiêu dự án

- Tìm hiểu về kiểm thử phần mềm tự động.
- Tìm hiểu và sử dụng công cụ Katalon Studio.
- Xây dựng các Test Case kiểm thử cho website.
- Thực hiện kiểm thử tự động các chức năng của hệ thống.
- Phân tích và đánh giá kết quả kiểm thử.
- Làm quen với quy trình làm việc nhóm và quản lý mã nguồn bằng GitHub.

## Công nghệ và công cụ sử dụng

- Katalon Studio
- Selenium WebDriver
- Groovy
- GitHub

## Thành viên Nhóm 2

| STT | Họ và tên | Vai trò |
|-----|-----------|---------|
| 1 | Nguyễn Huy Hoàng | Nhóm trưởng |
| 2 | Nguyễn Thùy Dung | Thành viên |
| 3 | Lê Xuân Mạnh | Thành viên |
| 4 | Nguyễn Hà Anh | Thành viên |
| 5 | Nguyễn Hồng Hà | Thành viên |

## Quản lý dự án

Dự án được quản lý mã nguồn bằng GitHub. Mỗi thành viên sẽ làm việc trên branch riêng theo sự phân công của nhóm trưởng trước khi cập nhật và tích hợp kết quả vào dự án chung.

## Nội dung thực hiện

Nhóm sẽ tiến hành:

1. Lựa chọn website và các chức năng cần kiểm thử.
2. Phân tích các chức năng của website.
3. Xây dựng Test Case.
4. Thực hiện kiểm thử tự động bằng Katalon Studio.
5. Ghi nhận kết quả kiểm thử.
6. Phân tích và đánh giá kết quả.
7. Hoàn thiện báo cáo dự án.
# PHÂN CHIA CÔNG VIỆC NHÓM 2

## 1. Nguyễn Huy Hoàng – Nhóm trưởng

### Công việc chung

* Tạo và quản lý Repository trên GitHub.
* Mời các thành viên tham gia Repository.
* Tạo branch riêng cho từng thành viên.
* Phân chia và theo dõi tiến độ công việc của nhóm.
* Kiểm tra, tổng hợp và tích hợp các phần công việc của các thành viên.
* Xây dựng Test Suite chung cho dự án.
* Tổng hợp kết quả và báo cáo kiểm thử.
* Hoàn thiện README và quản lý mã nguồn của dự án.

### Công việc cá nhân

Phụ trách kiểm thử các chức năng **Đăng ký – Đăng nhập – Đăng xuất – Quản lý tài khoản**.

Các nội dung kiểm thử dự kiến:

* Kiểm tra đăng ký tài khoản thành công.
* Kiểm tra đăng ký với email đã tồn tại.
* Kiểm tra đăng nhập thành công.
* Kiểm tra đăng nhập với email hoặc mật khẩu không đúng.
* Kiểm tra đăng nhập khi bỏ trống thông tin.
* Kiểm tra chức năng đăng xuất.
* Kiểm tra thông tin tài khoản sau khi đăng nhập.
* Kiểm tra chức năng xóa tài khoản.

**Branch:** `hoang-dang-nhap-dang-ky`

---

## 2. Nguyễn Thùy Dung

### Công việc

Phụ trách kiểm thử chức năng **Sản phẩm và Tìm kiếm sản phẩm**.

Các nội dung kiểm thử dự kiến:

* Kiểm tra hiển thị danh sách sản phẩm.
* Kiểm tra xem thông tin chi tiết sản phẩm.
* Kiểm tra tìm kiếm sản phẩm với từ khóa hợp lệ.
* Kiểm tra tìm kiếm sản phẩm không tồn tại.
* Kiểm tra tìm kiếm với từ khóa rỗng.
* Kiểm tra hiển thị sản phẩm theo danh mục.
* Kiểm tra hiển thị sản phẩm theo thương hiệu.
* Kiểm tra chuyển đổi giữa các danh mục sản phẩm.
* Kiểm tra chức năng đánh giá sản phẩm.

**Branch:** `dung-san-pham-tim-kiem`

---

## 3. Lê Xuân Mạnh

### Công việc

Phụ trách kiểm thử chức năng **Giỏ hàng**.

Các nội dung kiểm thử dự kiến:

* Kiểm tra thêm một sản phẩm vào giỏ hàng.
* Kiểm tra thêm nhiều sản phẩm vào giỏ hàng.
* Kiểm tra thông tin sản phẩm trong giỏ hàng.
* Kiểm tra số lượng sản phẩm trong giỏ hàng.
* Kiểm tra thay đổi số lượng sản phẩm.
* Kiểm tra giá sản phẩm trong giỏ hàng.
* Kiểm tra tổng giá trị đơn hàng.
* Kiểm tra xóa sản phẩm khỏi giỏ hàng.
* Kiểm tra trạng thái giỏ hàng sau khi cập nhật.

**Branch:** `manh-gio-hang`

---

## 4. Nguyễn Hà Anh

### Công việc

Phụ trách kiểm thử **Biểu mẫu và các chức năng tương tác với người dùng**.

Các nội dung kiểm thử dự kiến:

* Kiểm tra chức năng Liên hệ.
* Kiểm tra gửi biểu mẫu liên hệ thành công.
* Kiểm tra gửi biểu mẫu khi bỏ trống thông tin bắt buộc.
* Kiểm tra nhập email không đúng định dạng.
* Kiểm tra chức năng tải tệp lên.
* Kiểm tra chức năng đăng ký nhận thông tin qua email tại trang chủ.
* Kiểm tra chức năng đăng ký nhận thông tin qua email tại trang giỏ hàng.
* Kiểm tra các thông báo phản hồi của hệ thống.

Ngoài ra, thực hiện **kiểm thử theo dữ liệu (Data Driven Testing)** với nhiều bộ dữ liệu khác nhau.

**Branch:** `ha-bieu-mau-nguoi-dung`

---

## 5. Nguyễn Hồng Hà

### Công việc

Phụ trách kiểm thử chức năng **Thanh toán và Đơn hàng**.

Các nội dung kiểm thử dự kiến:

* Kiểm tra quá trình thanh toán.
* Kiểm tra thanh toán sau khi đăng ký tài khoản.
* Kiểm tra thanh toán sau khi đăng nhập.
* Kiểm tra thông tin địa chỉ giao hàng.
* Kiểm tra thông tin đơn hàng.
* Kiểm tra tổng số tiền thanh toán.
* Kiểm tra nhập thông tin thanh toán.
* Kiểm tra đặt hàng thành công.
* Kiểm tra xác nhận đơn hàng.
* Kiểm tra chức năng tải hóa đơn.

**Branch:** `hong-ha-thanh-toan-don-hang`

---

# SẢN PHẨM MỖI THÀNH VIÊN CẦN THỰC HIỆN

Mỗi thành viên chịu trách nhiệm xây dựng và hoàn thiện phần công việc được phân công, bao gồm:

* Phân tích chức năng cần kiểm thử.
* Xây dựng Test Scenario.
* Thiết kế Test Case.
* Xây dựng các Test Object trong Object Repository.
* Viết Script kiểm thử tự động bằng Katalon Studio.
* Thực hiện chạy kiểm thử.
* Ghi nhận và đánh giá kết quả kiểm thử.
* Commit và Push kết quả công việc lên branch cá nhân trên GitHub.

# TỔNG QUAN DỰ ÁN

| Thành viên       | Chức năng phụ trách                            | Branch                        |
| ---------------- | ---------------------------------------------- | ----------------------------- |
| Nguyễn Huy Hoàng | Đăng ký, Đăng nhập, Đăng xuất và quản lý dự án | `hoang-dang-nhap-dang-ky`     |
| Nguyễn Thùy Dung | Sản phẩm và Tìm kiếm                           | `dung-san-pham-tim-kiem`      |
| Lê Xuân Mạnh     | Giỏ hàng                                       | `manh-gio-hang`               |
| Nguyễn Hà Anh    | Biểu mẫu và tương tác người dùng               | `ha-bieu-mau-nguoi-dung`      |
| Nguyễn Hồng Hà   | Thanh toán và Đơn hàng                         | `hong-ha-thanh-toan-don-hang` |

Dự án được thực hiện theo hình thức làm việc nhóm và quản lý mã nguồn bằng GitHub. Mỗi thành viên làm việc trên branch riêng theo phần công việc được phân công, sau đó các kết quả sẽ được tổng hợp và tích hợp vào dự án chung.
