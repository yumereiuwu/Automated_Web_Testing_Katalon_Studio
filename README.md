# KIỂM THỬ TỰ ĐỘNG TRÊN NỀN WEB BẰNG KATALON STUDIO

## 1. Giới thiệu dự án

Đây là dự án thực hành chuyên ngành của **Nhóm 2** với chủ đề:

**KIỂM THỬ TỰ ĐỘNG TRÊN NỀN WEB BẰNG KATALON STUDIO**

Dự án được thực hiện nhằm phân tích, thiết kế và kiểm thử tự động một hệ thống website bán đồ ăn vặt trên nền tảng Web.

Hệ thống được xây dựng trên nền tảng **ASP.NET MVC**, sử dụng **C#** để xử lý các nghiệp vụ và **Microsoft SQL Server** để lưu trữ, quản lý dữ liệu.

Trong giai đoạn kiểm thử, nhóm sử dụng **Katalon Studio** để xây dựng và thực hiện các Test Case nhằm kiểm tra các chức năng của website.

---

# 2. Phân tích và thiết kế hệ thống

## 2.1. Phân tích tổng quan hệ thống

### 2.1.1. Giới thiệu hệ thống

Hệ thống là một website bán đồ ăn vặt, cho phép khách hàng truy cập và thực hiện các chức năng liên quan đến việc xem, tìm kiếm và mua sản phẩm.

Hệ thống được xây dựng theo mô hình **ASP.NET MVC**, trong đó Backend chịu trách nhiệm xử lý logic nghiệp vụ, Frontend cung cấp giao diện tương tác với người dùng và Database chịu trách nhiệm lưu trữ dữ liệu.

Hệ thống gồm hai nhóm người dùng chính:

* **Khách hàng:** sử dụng các chức năng xem sản phẩm, tìm kiếm, quản lý tài khoản, giỏ hàng và đặt hàng.
* **Quản trị viên:** quản lý sản phẩm, danh mục, đơn hàng và người dùng theo quyền được cấp.

### 2.1.2. Mô tả hoạt động của hệ thống

Người dùng truy cập website thông qua trình duyệt Web. Các thao tác của người dùng được gửi đến hệ thống ASP.NET MVC để xử lý.

Quy trình hoạt động tổng quát:

```text
Người dùng
    ↓
Trình duyệt Web
    ↓
Frontend
    ↓
Backend ASP.NET MVC
    ↓
Database SQL Server
    ↓
Backend xử lý kết quả
    ↓
Frontend hiển thị kết quả
    ↓
Người dùng
```

Đối với khách hàng, quy trình mua hàng cơ bản gồm:

1. Truy cập website.
2. Xem hoặc tìm kiếm sản phẩm.
3. Xem thông tin chi tiết sản phẩm.
4. Thêm sản phẩm vào giỏ hàng.
5. Cập nhật giỏ hàng.
6. Tiến hành đặt hàng.
7. Nhập và xác nhận thông tin cần thiết.
8. Hệ thống xử lý đơn hàng.
9. Lưu thông tin đơn hàng vào cơ sở dữ liệu.
10. Hiển thị kết quả cho người dùng.

Đối với quản trị viên, hệ thống cung cấp các chức năng quản lý dữ liệu theo quyền được cấp.

### 2.1.3. Công nghệ dự kiến sử dụng

| Công nghệ/Công cụ    | Mục đích sử dụng                       |
| -------------------- | -------------------------------------- |
| ASP.NET MVC          | Xây dựng ứng dụng Web                  |
| C#                   | Xử lý Backend và logic nghiệp vụ       |
| HTML                 | Xây dựng cấu trúc giao diện            |
| CSS                  | Thiết kế giao diện                     |
| JavaScript           | Xử lý tương tác trên giao diện         |
| Microsoft SQL Server | Lưu trữ và quản lý dữ liệu             |
| Katalon Studio       | Xây dựng và thực hiện kiểm thử tự động |
| Selenium WebDriver   | Hỗ trợ tự động hóa trình duyệt         |
| Groovy               | Xây dựng Test Script trong Katalon     |
| GitHub               | Quản lý mã nguồn và làm việc nhóm      |

---

# 2.2. Phân tích yêu cầu hệ thống

## 2.2.1. Xác định các tác nhân

Hệ thống xác định hai tác nhân chính:

### Khách hàng

Khách hàng là người sử dụng website để xem, tìm kiếm và mua sản phẩm.

Các chức năng chính:

* Đăng ký tài khoản.
* Đăng nhập.
* Đăng xuất.
* Xem sản phẩm.
* Tìm kiếm sản phẩm.
* Xem chi tiết sản phẩm.
* Quản lý giỏ hàng.
* Đặt hàng.
* Quản lý thông tin tài khoản.

### Quản trị viên

Quản trị viên là người quản lý hoạt động và dữ liệu của hệ thống.

Các chức năng chính:

* Đăng nhập quản trị.
* Quản lý sản phẩm.
* Quản lý danh mục.
* Quản lý đơn hàng.
* Quản lý người dùng.

---

## 2.2.2. Phân tích yêu cầu chức năng

### Quản lý tài khoản

* Đăng ký tài khoản.
* Đăng nhập.
* Đăng xuất.
* Xem và cập nhật thông tin tài khoản.
* Kiểm tra thông tin đăng nhập.

### Quản lý sản phẩm

* Hiển thị danh sách sản phẩm.
* Xem chi tiết sản phẩm.
* Tìm kiếm sản phẩm.
* Phân loại sản phẩm.
* Hiển thị thông tin sản phẩm.

### Quản lý giỏ hàng

* Thêm sản phẩm vào giỏ hàng.
* Xem giỏ hàng.
* Thay đổi số lượng sản phẩm.
* Xóa sản phẩm khỏi giỏ hàng.
* Tính tổng giá trị giỏ hàng.

### Đặt hàng

* Xác nhận sản phẩm cần mua.
* Nhập thông tin giao hàng.
* Kiểm tra thông tin đơn hàng.
* Xác nhận đặt hàng.
* Lưu thông tin đơn hàng.

### Quản trị hệ thống

* Quản lý sản phẩm.
* Quản lý danh mục.
* Quản lý đơn hàng.
* Quản lý người dùng.

---

## 2.2.3. Phân tích yêu cầu phi chức năng

Hệ thống cần đáp ứng các yêu cầu phi chức năng:

* **Tính dễ sử dụng:** giao diện rõ ràng và dễ thao tác.
* **Tính bảo mật:** bảo vệ thông tin tài khoản và kiểm soát quyền truy cập.
* **Tính chính xác:** dữ liệu nhập vào và kết quả xử lý phải chính xác.
* **Tính ổn định:** hệ thống hoạt động ổn định trong quá trình sử dụng.
* **Khả năng tương thích:** website có thể hoạt động trên các trình duyệt Web phổ biến.
* **Khả năng bảo trì:** hệ thống được tổ chức rõ ràng để thuận tiện cho việc sửa đổi và phát triển.

---

# 2.3. Phân tích Use Case

## 2.3.1. Sơ đồ Use Case tổng quát

Hệ thống có hai tác nhân chính là **Khách hàng** và **Quản trị viên**.

Khách hàng tương tác với các chức năng:

* Đăng ký.
* Đăng nhập.
* Đăng xuất.
* Xem sản phẩm.
* Tìm kiếm sản phẩm.
* Xem chi tiết sản phẩm.
* Quản lý giỏ hàng.
* Đặt hàng.
* Quản lý tài khoản.

Quản trị viên tương tác với:

* Đăng nhập.
* Quản lý sản phẩm.
* Quản lý danh mục.
* Quản lý đơn hàng.
* Quản lý người dùng.

## 2.3.2. Danh sách Use Case

| Mã   | Use Case              | Tác nhân                 |
| ---- | --------------------- | ------------------------ |
| UC01 | Đăng ký               | Khách hàng               |
| UC02 | Đăng nhập             | Khách hàng/Quản trị viên |
| UC03 | Đăng xuất             | Khách hàng/Quản trị viên |
| UC04 | Xem sản phẩm          | Khách hàng               |
| UC05 | Tìm kiếm sản phẩm     | Khách hàng               |
| UC06 | Xem chi tiết sản phẩm | Khách hàng               |
| UC07 | Quản lý giỏ hàng      | Khách hàng               |
| UC08 | Đặt hàng              | Khách hàng               |
| UC09 | Quản lý sản phẩm      | Quản trị viên            |
| UC10 | Quản lý danh mục      | Quản trị viên            |
| UC11 | Quản lý đơn hàng      | Quản trị viên            |
| UC12 | Quản lý người dùng    | Quản trị viên            |

Các Use Case trên là cơ sở để nhóm xây dựng Test Scenario và Test Case trong giai đoạn kiểm thử.

---

# 2.4. Thiết kế kiến trúc hệ thống

## 2.4.1. Kiến trúc tổng thể

Hệ thống được thiết kế theo mô hình Web Application với các thành phần chính:

```text
┌─────────────────────────┐
│        Người dùng       │
│   Khách hàng / Admin    │
└────────────┬────────────┘
             ↓
┌─────────────────────────┐
│       Web Browser       │
└────────────┬────────────┘
             ↓
┌─────────────────────────┐
│        Frontend         │
│    HTML/CSS/JavaScript  │
└────────────┬────────────┘
             ↓
┌─────────────────────────┐
│    ASP.NET MVC Backend  │
│       C# / Logic        │
└────────────┬────────────┘
             ↓
┌─────────────────────────┐
│      SQL Server         │
│        Database         │
└─────────────────────────┘
```

Trong giai đoạn kiểm thử tự động, **Katalon Studio** sẽ tương tác với website thông qua trình duyệt Web để thực hiện các thao tác được mô tả trong Test Case.

---

## 2.4.2. Thiết kế mô hình MVC

### Model

Model đại diện cho dữ liệu của hệ thống và thực hiện việc trao đổi dữ liệu giữa ứng dụng và cơ sở dữ liệu.

Các nhóm dữ liệu chính gồm:

* Người dùng.
* Sản phẩm.
* Danh mục.
* Giỏ hàng.
* Đơn hàng.
* Chi tiết đơn hàng.

### View

View chịu trách nhiệm hiển thị giao diện cho người dùng.

Các giao diện chính gồm:

* Trang chủ.
* Trang đăng ký.
* Trang đăng nhập.
* Trang sản phẩm.
* Trang chi tiết sản phẩm.
* Trang giỏ hàng.
* Trang đặt hàng.
* Trang quản trị.

### Controller

Controller tiếp nhận yêu cầu từ người dùng, xử lý yêu cầu và điều phối dữ liệu giữa View và Model.

Quy trình:

```text
View
 ↓
Controller
 ↓
Model
 ↓
Database
 ↓
Model
 ↓
Controller
 ↓
View
```

---

# 2.5. Thiết kế cơ sở dữ liệu

Cơ sở dữ liệu được thiết kế nhằm lưu trữ và quản lý các thông tin cần thiết cho hoạt động của website.

Các nhóm dữ liệu chính bao gồm:

* Thông tin người dùng.
* Thông tin sản phẩm.
* Thông tin danh mục.
* Thông tin giỏ hàng.
* Thông tin đơn hàng.
* Chi tiết đơn hàng.

Cơ sở dữ liệu được xây dựng và quản lý bằng **Microsoft SQL Server**.

Các bảng, thuộc tính, khóa chính, khóa ngoại và mối quan hệ giữa các bảng sẽ được xác định dựa trên cơ sở dữ liệu thực tế của hệ thống.

---

# 2.6. Thiết kế giao diện

Giao diện hệ thống được thiết kế nhằm đảm bảo người dùng có thể dễ dàng thực hiện các thao tác.

Các giao diện chính gồm:

* Trang chủ.
* Trang đăng nhập.
* Trang đăng ký.
* Trang danh sách sản phẩm.
* Trang tìm kiếm.
* Trang chi tiết sản phẩm.
* Trang giỏ hàng.
* Trang đặt hàng/thanh toán.
* Trang quản trị.

Frontend sử dụng HTML, CSS và JavaScript kết hợp với ASP.NET MVC để hiển thị dữ liệu và xử lý tương tác với người dùng.

---

# 2.7. Thiết kế quy trình nghiệp vụ

## 2.7.1. Quy trình đăng nhập

```text
Người dùng
    ↓
Mở trang đăng nhập
    ↓
Nhập thông tin
    ↓
Gửi thông tin
    ↓
Hệ thống kiểm tra
    ↓
Thông tin hợp lệ?
   / \
 Có   Không
 ↓       ↓
Đăng    Thông báo
nhập    lỗi
thành
công
```

## 2.7.2. Quy trình mua hàng

```text
Xem sản phẩm
     ↓
Tìm kiếm / Lựa chọn
     ↓
Xem chi tiết
     ↓
Thêm vào giỏ hàng
     ↓
Kiểm tra giỏ hàng
     ↓
Cập nhật số lượng
     ↓
Đặt hàng
     ↓
Nhập thông tin
     ↓
Xác nhận
     ↓
Tạo đơn hàng
     ↓
Lưu Database
```

## 2.7.3. Quy trình quản lý sản phẩm

```text
Quản trị viên đăng nhập
          ↓
   Vào trang quản trị
          ↓
  Quản lý sản phẩm
          ↓
   Hiển thị danh sách
          ↓
     ┌────┼────┐
     ↓    ↓    ↓
    Thêm  Sửa  Xóa
     ↓    ↓    ↓
     └────┼────┘
          ↓
   Kiểm tra dữ liệu
          ↓
   Cập nhật Database
```

---

# 2.8. Thiết kế định hướng kiểm thử

## 2.8.1. Các chức năng cần kiểm thử

Các nhóm chức năng dự kiến kiểm thử gồm:

* Đăng ký.
* Đăng nhập.
* Đăng xuất.
* Quản lý tài khoản.
* Sản phẩm.
* Tìm kiếm sản phẩm.
* Giỏ hàng.
* Đặt hàng.
* Thanh toán.
* Biểu mẫu và chức năng tương tác với người dùng.

## 2.8.2. Phương pháp kiểm thử

Nhóm sử dụng **Katalon Studio** để thực hiện kiểm thử tự động trên nền Web.

Quy trình dự kiến:

```text
Phân tích chức năng
       ↓
Thiết kế Test Scenario
       ↓
Thiết kế Test Case
       ↓
Thiết lập Object Repository
       ↓
Xây dựng Test Script
       ↓
Thực hiện kiểm thử
       ↓
Ghi nhận kết quả
```

## 2.8.3. Thiết kế Test Case

Mỗi Test Case dự kiến bao gồm:

| Thành phần      | Nội dung                     |
| --------------- | ---------------------------- |
| Test Case ID    | Mã Test Case                 |
| Test Case Name  | Tên Test Case                |
| Preconditions   | Điều kiện trước khi kiểm thử |
| Test Data       | Dữ liệu kiểm thử             |
| Test Steps      | Các bước thực hiện           |
| Expected Result | Kết quả mong đợi             |

Kết quả thực tế, trạng thái Pass/Fail và lỗi phát hiện sẽ được ghi nhận trong giai đoạn thực hiện kiểm thử.

---

# 2.9. Thiết kế phân quyền

Hệ thống được thiết kế với hai nhóm quyền chính.

## Khách hàng

Khách hàng có quyền:

* Xem sản phẩm.
* Tìm kiếm sản phẩm.
* Xem chi tiết sản phẩm.
* Đăng ký và đăng nhập.
* Quản lý tài khoản.
* Quản lý giỏ hàng.
* Đặt hàng.

## Quản trị viên

Quản trị viên có quyền:

* Đăng nhập khu vực quản trị.
* Quản lý sản phẩm.
* Quản lý danh mục.
* Quản lý đơn hàng.
* Quản lý người dùng.

Hệ thống cần kiểm tra quyền truy cập trước khi cho phép người dùng thực hiện các chức năng yêu cầu quyền tương ứng.

---

# 2.10. Phân công thực hiện

| STT | Thành viên           | Phụ trách                        |
| --- | -------------------- | -------------------------------- |
| 1   | **Nguyễn Huy Hoàng** | Backend và xử lý logic nghiệp vụ |
| 2   | **Lê Xuân Mạnh**     | Frontend và giao diện người dùng |
| 3   | **Nguyễn Thùy Dung** | Phân tích và thiết kế Database   |
| 4   | **Nguyễn Hà Anh**    | Kiểm thử và Katalon Studio       |
| 5   | **Nguyễn Hồng Hà**   | Kiểm thử và Katalon Studio       |

---

# 3. Kết luận

Qua quá trình phân tích và thiết kế, nhóm đã xác định được tổng quan hệ thống, các tác nhân, yêu cầu chức năng và phi chức năng, Use Case, kiến trúc hệ thống, cơ sở dữ liệu, giao diện, quy trình nghiệp vụ và định hướng kiểm thử.

Các nội dung trên là cơ sở để nhóm tiếp tục triển khai hệ thống và thực hiện kiểm thử tự động bằng Katalon Studio trong các giai đoạn tiếp theo.
