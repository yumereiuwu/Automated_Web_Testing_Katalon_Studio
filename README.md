# Kiểm thử tự động trên nền web bằng Katalon Studio

> **Báo cáo Thực hành chuyên ngành – Nhóm 2 (A2)**
> Trường Đại học Sư phạm Hà Nội 2 · Viện Công nghệ thông tin · Lớp K49A-CNTT
> Giảng viên hướng dẫn: **TS. Trần Tuấn Vinh**

## Mục lục

1. [Tổng quan đề tài](#1-tổng-quan-đề-tài)
2. [Khảo sát & lựa chọn đề tài](#2-khảo-sát--lựa-chọn-đề-tài)
3. [Project Charter](#3-project-charter)
4. [WBS, phân công & tiến độ](#4-wbs-phân-công--tiến-độ)
5. [Khảo sát hệ thống](#5-khảo-sát-hệ-thống)
6. [Yêu cầu chức năng](#6-yêu-cầu-chức-năng)
7. [Yêu cầu phi chức năng](#7-yêu-cầu-phi-chức-năng)
8. [Đặc tả SRS (các chức năng chính)](#8-đặc-tả-srs-các-chức-năng-chính)
9. [Phân tích hệ thống (UML)](#9-phân-tích-hệ-thống-uml)
10. [Thiết kế hệ thống](#10-thiết-kế-hệ-thống)
11. [Hướng kiểm thử với Katalon Studio](#11-hướng-kiểm-thử-với-katalon-studio)

---

## 1. Tổng quan đề tài

**Tên đề tài:** Kiểm thử tự động trên nền web bằng Katalon Studio.

**Đối tượng kiểm thử:** một **website bán đồ ăn vặt** đã được xây dựng sẵn (ASP.NET MVC5). Nhóm dùng website này làm môi trường thực tế để phân tích, thiết kế và viết kịch bản kiểm thử tự động bằng Katalon Studio.

**Bối cảnh:** khi website có nhiều chức năng, việc kiểm tra thủ công toàn bộ sau mỗi lần cập nhật rất tốn thời gian và dễ sai sót. Kiểm thử tự động cho phép chạy lại các kịch bản đã thiết lập, chuẩn hóa quy trình, giảm thao tác lặp lại và ghi nhận kết quả (Pass/Fail) rõ ràng.

### Mục tiêu

**Tổng quát:** áp dụng Katalon Studio để kiểm thử tự động một website thực tế, xây dựng được quy trình từ phân tích chức năng → thiết kế test case → thực hiện → đánh giá kết quả.

**Cụ thể:**
- Tìm hiểu khái niệm cơ bản về kiểm thử phần mềm và kiểm thử tự động
- Tìm hiểu môi trường và chức năng của Katalon Studio
- Khảo sát, phân tích website bán đồ ăn vặt
- Xác định yêu cầu chức năng và phi chức năng
- Phân tích và thiết kế hệ thống
- Xây dựng test case cho các chức năng cần kiểm tra
- Xây dựng và chạy kịch bản kiểm thử tự động bằng Katalon Studio
- Ghi nhận, đánh giá kết quả kiểm thử
- Hoàn thiện báo cáo

### Phạm vi

| Phạm vi hệ thống | Phạm vi kiểm thử |
|---|---|
| Quản lý tài khoản | Kiểm thử chức năng trên nền web |
| Đăng ký, đăng nhập | Kiểm thử các luồng thao tác của người dùng |
| Xem và tìm kiếm sản phẩm | Kiểm thử dữ liệu hợp lệ và không hợp lệ |
| Xem chi tiết sản phẩm | Xây dựng test case |
| Quản lý giỏ hàng | Tự động hóa một số kịch bản bằng Katalon Studio |
| Đặt hàng | Ghi nhận kết quả Pass/Fail |
| Quản lý đơn hàng | |
| Các chức năng quản trị (nếu có) | |

### Công nghệ & công cụ

| Công nghệ / công cụ | Mục đích |
|---|---|
| ASP.NET MVC5 | Xây dựng website |
| C# | Phát triển backend |
| HTML5 / CSS3 / JavaScript | Giao diện và xử lý phía client |
| SQL Server | Quản lý cơ sở dữ liệu |
| **Katalon Studio** | Xây dựng và thực hiện kiểm thử tự động |
| Git / GitHub | Quản lý mã nguồn, kiểm tra và cập nhật |

### Quy trình thực hiện

```
Khảo sát hệ thống → Phân tích yêu cầu → Thiết kế hệ thống → Xác định chức năng cần kiểm thử → Xây dựng Test Case
                                                                                                      ↓
Đánh giá & hoàn thiện ← Ghi nhận kết quả ← Thực hiện kiểm thử ← Thiết kế kịch bản trên Katalon Studio
```

### Sản phẩm đầu ra

- Website bán đồ ăn vặt (đối tượng kiểm thử)
- Tài liệu phân tích yêu cầu hệ thống (SRS)
- Thiết kế cơ sở dữ liệu (ERD)
- Sơ đồ kiến trúc hệ thống
- Thiết kế giao diện / Wireframe / Mockup
- Bộ Test Case
- Các kịch bản kiểm thử tự động trên Katalon Studio
- Kết quả thực hiện kiểm thử
- Báo cáo đề tài và slide thuyết trình

### Tiêu chí hoàn thành đề tài

- Hoàn thiện tài liệu phân tích, thiết kế theo yêu cầu môn học
- Hoàn thiện website dùng làm đối tượng kiểm thử
- Xác định được các chức năng cần kiểm thử và xây dựng Test Case tương ứng
- Xây dựng và thực hiện được kịch bản trên Katalon Studio, ghi nhận kết quả
- Hoàn thiện báo cáo và slide thuyết trình

---

## 2. Khảo sát & lựa chọn đề tài

### Xu hướng công nghệ
Hệ thống web ngày càng phổ biến (thương mại điện tử, giáo dục, giải trí, quản lý, dịch vụ trực tuyến). Số chức năng tăng lên khiến kiểm thử thủ công mất thời gian và dễ sai sót, vì vậy **kiểm thử phần mềm tự động** trở thành hướng được quan tâm. Katalon Studio hỗ trợ xây dựng, quản lý và chạy kịch bản kiểm thử cho các thao tác thường gặp: đăng nhập, tìm kiếm, thêm giỏ hàng, đặt hàng, kiểm tra kết quả trả về.

### Ba ý tưởng được đề xuất

**Ý tưởng 1 – Hệ thống quản lý bán hàng trên nền web** (sản phẩm, danh mục, đăng ký/đăng nhập, giỏ hàng, đặt hàng)
- *Ưu:* ứng dụng thực tế, nhiều chức năng, dùng công nghệ web phổ biến, dễ mở rộng
- *Nhược:* phạm vi lớn, tốn thời gian xây dựng, phải kiểm tra nhiều trường hợp

**Ý tưởng 2 – Hệ thống hỗ trợ kiểm thử tự động cho website** (đăng nhập, tìm kiếm, giỏ hàng, đặt hàng)
- *Ưu:* gắn với nhu cầu đảm bảo chất lượng, dùng được khi phát triển/bảo trì, nhiều kịch bản, đánh giá kết quả có hệ thống
- *Nhược:* cần kiến thức kiểm thử, cần làm quen công cụ, kịch bản phải khớp chức năng thực tế

**Ý tưởng 3 – Kiểm thử tự động trên nền web bằng Katalon Studio** *(được chọn)*
- *Ưu:* đúng xu hướng tự động hóa, áp dụng trên website thực tế, nhiều chức năng để xây test case, kết hợp phân tích hệ thống + phát triển web + kiểm thử
- *Nhược:* cần tìm hiểu Katalon, cần thiết kế test case phù hợp từng chức năng, kết quả phụ thuộc độ ổn định của website

### Bảng so sánh

| Tiêu chí | Ý tưởng 1 | Ý tưởng 2 | Ý tưởng 3 |
|---|---|---|---|
| Tính ứng dụng | Cao | Cao | Cao |
| Phù hợp môn học | Có | Có | Có |
| Khả năng triển khai | Khá | Khá | Tốt |
| Phạm vi thực hiện | Khá rộng | Trung bình | Có thể giới hạn |
| Dùng được website có sẵn | Có | Có | Có |
| Có kiểm thử tự động | Không bắt buộc | Có | Có |
| Đánh giá được kết quả kiểm thử | Hạn chế | Có | Có |
| Mức độ phù hợp hướng đề tài | Trung bình | Cao | Cao |

---

## 3. Project Charter

**Đối tượng sử dụng hệ thống**
- *Khách hàng:* tìm kiếm, xem và mua sản phẩm
- *Quản trị viên:* quản lý nội dung và hoạt động website theo quyền được cấp

**Đối tượng kiểm thử:** website bán đồ ăn vặt trên nền web; Katalon Studio là công cụ thực hiện kịch bản kiểm thử.

### Thành viên và vai trò

| Thành viên | Vai trò | Nhiệm vụ chính |
|---|---|---|
| Nguyễn Huy Hoàng | Nhóm trưởng – Backend / Phân tích hệ thống | Backend, phân tích & thiết kế hệ thống, tổng hợp báo cáo |
| Lê Xuân Mạnh | Frontend | Giao diện, UI/UX, Wireframe/Mockup |
| Nguyễn Thùy Dung | Database | Thiết kế & quản lý CSDL, ERD |
| Nguyễn Hà Anh | Tài liệu / PPT / hỗ trợ test | Tổng hợp nội dung, xây dựng slide, hỗ trợ tester |
| Nguyễn Hồng Hà | Tester / PPT | Xây dựng Test Case, tìm hiểu và thực hiện kiểm thử bằng Katalon Studio |

---

## 4. WBS, phân công & tiến độ

### WBS cấp 1

```
Dự án kiểm thử tự động trên nền web bằng Katalon Studio
├── 1. Khảo sát và lập kế hoạch
│   ├── 1.1 Khảo sát xu hướng công nghệ
│   ├── 1.2 Đề xuất và lựa chọn đề tài
│   ├── 1.3 Xây dựng Project Charter
│   └── 1.4 Xây dựng WBS và phân công nhóm
├── 2. Phân tích và thiết kế
│   ├── 2.1 Thu thập và phân tích yêu cầu (khảo sát hệ thống, yêu cầu chức năng, phi chức năng, SRS)
│   ├── 2.2 Thiết kế cơ sở dữ liệu (phân tích dữ liệu, xác định bảng, chuẩn hóa, ERD)
│   ├── 2.3 Thiết kế kiến trúc hệ thống (phân tích, xác định thành phần, sơ đồ kiến trúc)
│   └── 2.4 Thiết kế UI/UX (phân tích giao diện, Wireframe, Mockup)
├── 3. Kiểm thử
│   ├── 3.1 Xác định chức năng kiểm thử
│   ├── 3.2 Xây dựng Test Case
│   ├── 3.3 Xây dựng kịch bản Katalon
│   ├── 3.4 Thực hiện kiểm thử
│   └── 3.5 Ghi nhận và đánh giá kết quả
└── 4. Hoàn thiện
    ├── 4.1 Hoàn thiện báo cáo
    ├── 4.2 Hoàn thiện slide
    └── 4.3 Chuẩn bị thuyết trình
```

### Công việc giai đoạn đầu (8 tuần)

| Mã WBS | Công việc | Kết quả |
|---|---|---|
| 1.1 | Khảo sát xu hướng công nghệ | Danh sách xu hướng |
| 1.2 | Đề xuất 2–3 ý tưởng đề tài | Danh sách ý tưởng + ưu/nhược điểm |
| 1.3 | Chốt đề tài, viết Project Charter | Project Charter |
| 1.4 | Lập WBS và phân công | WBS + bảng phân công |
| 2.1 | Thu thập và phân tích yêu cầu | Bản SRS |
| 2.2 | Thiết kế CSDL | ERD bản thảo |
| 2.2 | Hoàn thiện và chuẩn hóa CSDL | ERD hoàn chỉnh |
| 2.3 | Thiết kế kiến trúc hệ thống | Bản thảo kiến trúc |
| 2.3 | Hoàn thiện kiến trúc | Sơ đồ kiến trúc |
| 2.4 | Bắt đầu thiết kế UI/UX | Wireframe |
| 2.4 | Hoàn thiện Mockup/UI/UX | Bộ Mockup hoàn chỉnh |
| 2.5 | Bảo vệ giữa kỳ A2 | Hồ sơ thiết kế đầy đủ |

### Trách nhiệm từng thành viên

**Nguyễn Huy Hoàng – Nhóm trưởng, Backend**
- Điều phối công việc của nhóm
- Tổng hợp nội dung Chương 1 và Chương 2
- Phân tích yêu cầu, thiết kế kiến trúc hệ thống
- Phát triển và kiểm tra Backend
- Kiểm tra sự thống nhất giữa yêu cầu, thiết kế, CSDL và hệ thống thực tế
- Tổng hợp báo cáo trước khi nộp

**Lê Xuân Mạnh – Frontend**
- Phát triển giao diện website, xác định các màn hình chính
- Thiết kế Wireframe, Mockup/UI/UX
- Cung cấp hình ảnh giao diện cho báo cáo

**Nguyễn Thùy Dung – Database**
- Khảo sát CSDL hiện tại, xác định các bảng
- Xác định khóa chính, khóa ngoại, kiểm tra quan hệ giữa các bảng
- Chuẩn hóa dữ liệu, xây dựng và hoàn thiện ERD
- Gửi ERD và phần mô tả CSDL để đưa vào báo cáo

**Nguyễn Hồng Hà – Tester / PPT**
- Tìm hiểu Katalon Studio, kiểm thử phần mềm
- Xác định chức năng cần kiểm thử, chuẩn bị Test Case, xây dựng kịch bản
- Thực hiện kiểm thử ở các giai đoạn sau, ghi nhận kết quả
- Phối hợp Hà Anh hoàn thiện slide thuyết trình

**Nguyễn Hà Anh – Tester / PPT**
- Nhận nội dung báo cáo từ các thành viên, tóm tắt nội dung quan trọng
- Xây dựng nội dung PowerPoint (giới thiệu, mục tiêu, phạm vi, chức năng, công nghệ)
- Thiết kế bố cục, đưa sơ đồ/hình ảnh/bảng biểu vào slide, đồng bộ font và hiệu ứng

### Tiến độ

| Giai đoạn | Nội dung | Thời gian |
|---|---|---|
| 1 | Khảo sát và đề xuất đề tài | Tuần 1 |
| 2 | Chốt đề tài, Project Charter, WBS | Tuần 2 |
| 3 | Phân tích yêu cầu, xây dựng SRS | Tuần 3 |
| 4 | Thiết kế CSDL và ERD | Tuần 4–5 |
| 5 | Thiết kế kiến trúc hệ thống | Tuần 5–6 |
| 6 | Thiết kế UX/UI và Wireframe | Tuần 6 |
| 7 | Hoàn thiện Mockup và tài liệu thiết kế | Tuần 7 |
| 8 | Bảo vệ giữa kỳ A2 | Tuần 8 |

---

## 5. Khảo sát hệ thống

**Mục đích khảo sát:** xác định đối tượng sử dụng, nhóm chức năng, luồng hoạt động, dữ liệu vào/ra, chức năng có thể tự động hóa; làm cơ sở cho SRS, thiết kế (CSDL, kiến trúc, giao diện) và Test Case/kịch bản Katalon.

### Hai khu vực khảo sát

| Khu vực người dùng (Khách hàng) | Khu vực quản trị (Quản trị viên) |
|---|---|
| Tài khoản, sản phẩm, tìm kiếm, giỏ hàng, đặt hàng | Quản lý sản phẩm, danh mục, đơn hàng và các chức năng quản trị khác |

### Quy trình mua hàng tổng quát

```
Truy cập website → Xem/tìm sản phẩm → Xem thông tin sản phẩm → Thêm vào giỏ hàng
→ Kiểm tra giỏ hàng → Đặt hàng → Hệ thống xử lý đơn → Kết quả đặt hàng
```

### Khung khảo sát từng chức năng (phục vụ kiểm thử)

| Nội dung | Câu hỏi cần xác định |
|---|---|
| Chức năng | Hệ thống thực hiện chức năng gì? |
| Actor | Ai thực hiện? |
| Đầu vào | Người dùng cần nhập dữ liệu gì? |
| Xử lý | Hệ thống xử lý dữ liệu như thế nào? |
| Đầu ra | Hệ thống trả về kết quả gì? |
| Điều kiện | Cần điều kiện gì trước khi thực hiện? |
| Trường hợp đúng | Dữ liệu hợp lệ thì điều gì xảy ra? |
| Trường hợp sai | Dữ liệu không hợp lệ thì điều gì xảy ra? |
| Khả năng tự động hóa | Có thực hiện được bằng Katalon Studio không? |

**Ví dụ – chức năng Đăng nhập:** nhập tài khoản → nhập mật khẩu → nhấn Đăng nhập → hệ thống kiểm tra → *hợp lệ:* đăng nhập thành công / *không hợp lệ:* thông báo lỗi. Từ luồng này sinh ra các test case: đúng tài khoản/mật khẩu, sai tài khoản, sai mật khẩu, bỏ trống tài khoản, bỏ trống mật khẩu, bỏ trống cả hai.

---

## 6. Yêu cầu chức năng

Ba nhóm đối tượng: **Khách vãng lai** (chưa đăng nhập), **Khách hàng** (đã đăng ký/đăng nhập), **Quản trị viên**.

### 6.1. Khách vãng lai

| Mã | Chức năng | Mô tả & kết quả |
|---|---|---|
| FR01 | Xem trang chủ | Hiển thị giới thiệu website, sản phẩm/nội dung nổi bật, banner, liên kết đến các khu vực khác |
| FR02 | Xem danh sách sản phẩm | Hiển thị tên, hình ảnh, giá bán, mô tả; chọn sản phẩm để xem chi tiết |
| FR03 | Tìm kiếm và lọc sản phẩm | Nhập từ khóa/chọn tiêu chí → hệ thống tìm/lọc → hiển thị kết quả phù hợp |
| FR04 | Xem chi tiết sản phẩm | Tên, hình ảnh, giá, mô tả, tình trạng sản phẩm, làm cơ sở quyết định thêm vào giỏ |
| FR05 | Quản lý giỏ hàng | Thêm sản phẩm, xem giỏ, đổi số lượng, xóa sản phẩm, kiểm tra tổng tiền |
| FR06 | Xem tin tức | Xem danh sách bài viết, chọn bài, xem nội dung chi tiết |
| FR07 | Xem chính sách | Xem các trang/khu vực chính sách của website |
| FR08 | Gửi thông tin liên hệ | Nhập biểu mẫu liên hệ → hệ thống kiểm tra, tiếp nhận và lưu thông tin |

> **Lưu ý của nhóm:** giỏ hàng là chức năng quan trọng với kiểm thử tự động vì có thể xây nhiều test case (thêm sản phẩm, sửa số lượng, xóa sản phẩm, kiểm tra tổng tiền).

### 6.2. Khách hàng – tài khoản

| Mã | Chức năng | Mô tả & kết quả |
|---|---|---|
| FR09 | Đăng ký tài khoản | Nhập thông tin → kiểm tra hợp lệ → tạo tài khoản nếu dữ liệu đạt yêu cầu |
| FR10 | Đăng nhập | Nhập thông tin → kiểm tra → hợp lệ thì tạo phiên đăng nhập; không hợp lệ thì báo lỗi để nhập lại |
| FR11 | Đăng xuất | Kết thúc phiên đăng nhập |
| FR12 | Đổi mật khẩu | Nhập mật khẩu hiện tại + mật khẩu mới → kiểm tra → cập nhật |
| FR13 | Quên mật khẩu | Thực hiện các bước trên giao diện để khôi phục/thiết lập lại mật khẩu |
| FR14 | Quản lý thông tin cá nhân | Xem/cập nhật họ tên, thông tin liên hệ, địa chỉ nhận hàng, thông tin tài khoản |

### 6.3. Khách hàng – đặt hàng (nhóm chức năng chính)

| Mã | Chức năng | Mô tả & kết quả |
|---|---|---|
| FR15 | Đặt hàng | Chọn sản phẩm → thêm giỏ → kiểm tra giỏ → nhập/kiểm tra thông tin nhận hàng → xác nhận → hệ thống kiểm tra và tạo đơn |
| FR16 | Chọn phương thức thanh toán | Chọn phương thức website hỗ trợ; trong đề tài dùng **COD (thanh toán khi nhận hàng)** làm luồng kiểm thử chính vì không phụ thuộc cổng thanh toán ngoài |
| FR17 | Kiểm tra & xác nhận đơn hàng | Kiểm tra lại danh sách sản phẩm, số lượng, đơn giá, tổng tiền, thông tin nhận hàng, phương thức thanh toán trước khi chốt |
| FR18 | Xem & theo dõi đơn hàng | Xem mã đơn, thời gian đặt, danh sách sản phẩm, tổng giá trị, trạng thái (cập nhật theo xử lý của quản trị viên) |

### 6.4. Quản trị viên

| Mã | Chức năng | Thao tác chính |
|---|---|---|
| FR19 | Quản lý danh mục | Xem, thêm, sửa, cập nhật trạng thái/ẩn danh mục |
| FR20 | Quản lý sản phẩm | Xem, thêm, sửa; cập nhật giá, số lượng/tồn kho, hình ảnh, mô tả, trạng thái |
| FR21 | Quản lý đơn hàng | Xem danh sách/chi tiết, cập nhật trạng thái, xử lý, hủy đơn khi cần |
| FR22 | Quản lý tài khoản khách hàng | Xem danh sách, xem thông tin, quản lý trạng thái, khóa/mở khóa |
| FR23 | Quản lý tin tức | Xem, thêm, sửa bài viết, cập nhật nội dung, đổi trạng thái hiển thị |

### 6.5. Báo cáo & thống kê (quản trị viên)

| Mã | Chức năng | Mô tả |
|---|---|---|
| FR24 | Báo cáo doanh thu | Xem doanh thu theo khoảng thời gian, tổng hợp từ dữ liệu đơn hàng |
| FR25 | Thống kê đơn hàng | Thống kê số lượng đơn theo trạng thái hoặc khoảng thời gian |
| FR26 | Thống kê sản phẩm | Theo dõi sản phẩm bán chạy, tình trạng tồn kho (nếu hệ thống hỗ trợ) |

---

## 7. Yêu cầu phi chức năng

| Mã | Nhóm | Nội dung chính | Cách đánh giá |
|---|---|---|---|
| NFR01 | Hiệu năng | Phản hồi ổn định; chuyển trang không chờ quá lâu; thao tác liên tiếp vẫn đúng; lỗi phải có thông báo thay vì dừng/hiển thị lỗi không kiểm soát. *Chưa đặt ngưỡng thời gian cụ thể, chỉ theo dõi trong quá trình kiểm thử.* | Theo dõi khi kiểm thử |
| NFR02 | Bảo mật | Phải đăng nhập trước khi dùng chức năng yêu cầu tài khoản; khách hàng không vào được khu quản trị; quản trị viên phải được xác thực; hạn chế truy cập trái phép dữ liệu cá nhân | Kiểm thử chức năng và phân quyền |
| NFR03 | Bảo mật | Kiểm tra dữ liệu đầu vào trước khi xử lý; dữ liệu nhập không được làm ảnh hưởng hoạt động hệ thống | Kiểm thử dữ liệu hợp lệ/không hợp lệ |
| NFR04 | Dễ sử dụng | Giao diện trực quan; quy trình mua hàng rõ ràng (*xem SP → chi tiết → giỏ → kiểm tra giỏ → đặt hàng → xác nhận*); nút đúng mục đích; có thông báo khi nhập sai | Kiểm thử giao diện & thao tác người dùng |
| NFR05 | Tương thích | Chạy được trên trình duyệt dùng để kiểm thử; đăng nhập, tìm kiếm, giỏ hàng, đặt hàng hoạt động bình thường; **kết quả test phải ghi rõ trình duyệt và môi trường** | Kiểm thử trên trình duyệt |
| NFR06 | Độ tin cậy | Thêm/sửa/xóa giỏ hàng cập nhật đúng; tổng tiền đúng; đơn tạo thành công phải được lưu; trạng thái đơn cập nhật đúng; không sinh bản ghi đơn hàng không hợp lệ | Kiểm tra kết quả sau thao tác |
| NFR07 | Khả năng kiểm thử | Mỗi chức năng có luồng rõ, kết quả quan sát được trên giao diện, thông báo thành công/thất bại rõ, thành phần giao diện xác định được để Katalon tương tác, test case chạy lại nhiều lần được | Xây dựng và chạy Test Case Katalon |
| NFR08 | Bảo trì | Mã nguồn tổ chức theo chức năng; đổi chức năng này ít ảnh hưởng chức năng khác; Test Case tổ chức theo **Test Case → Test Suite → Test Suite Collection** để chạy hồi quy | Kiểm thử hồi quy |
| NFR09 | Mở rộng | Có thể thêm sản phẩm, danh mục, chức năng quản lý, phương thức thanh toán, thống kê và kịch bản test mà không đổi cấu trúc | Đánh giá cấu trúc hệ thống |

**Test case bảo mật gợi ý:** đăng nhập hợp lệ; đăng nhập sai mật khẩu; truy cập chức năng cần đăng nhập khi chưa đăng nhập; tài khoản khách hàng cố truy cập khu quản trị; nhập dữ liệu không hợp lệ vào biểu mẫu.

**Ví dụ tính kiểm thử (đăng nhập):** *Input:* tài khoản + mật khẩu hợp lệ → *Action:* nhấn đăng nhập → *Expected:* đăng nhập thành công, giao diện chuyển sang trạng thái đã đăng nhập.

---

## 8. Đặc tả SRS (các chức năng chính)

**Mục đích SRS:** xác định phạm vi chức năng; mô tả chức năng theo từng đối tượng; xác định yêu cầu phi chức năng; làm cơ sở cho UML, phạm vi kiểm thử, Test Case và kịch bản Katalon; thống nhất cách hiểu trong nhóm.

### Tác nhân

| STT | Tác nhân | Mô tả |
|---|---|---|
| 1 | Khách vãng lai | Người truy cập website chưa đăng nhập |
| 2 | Khách hàng | Người đã đăng ký và đăng nhập, dùng được chức năng tài khoản, giỏ hàng, đặt hàng, theo dõi đơn |
| 3 | Quản trị viên | Người có quyền quản lý dữ liệu và vận hành chức năng quản trị |

### Nhóm chức năng trong SRS

| Nhóm | Mã |
|---|---|
| Trang thông tin | FR01 |
| Sản phẩm | FR02, FR03, FR04 |
| Giỏ hàng | FR05 |
| Nội dung | FR06, FR07 |
| Liên hệ | FR08 |
| Tài khoản | FR09 – FR14 |
| Đặt hàng / Thanh toán | FR15, FR16, FR17 |
| Đơn hàng | FR18 |
| Quản trị | FR19 – FR23 |
| Báo cáo | FR24 – FR26 |

### Đặc tả chi tiết 5 chức năng ưu tiên kiểm thử

**FR10 – Đăng nhập**

| Thành phần | Nội dung |
|---|---|
| Tác nhân | Khách hàng |
| Điều kiện trước | Người dùng đã có tài khoản |
| Đầu vào | Tên đăng nhập/thông tin tài khoản và mật khẩu |
| Xử lý | Hệ thống kiểm tra thông tin đăng nhập |
| Thành công | Đăng nhập thành công |
| Thất bại | Thông báo thông tin đăng nhập không hợp lệ |

*Test case:* thông tin hợp lệ · mật khẩu sai · dữ liệu bỏ trống · kiểm tra thông báo khi thất bại.

**FR03 – Tìm kiếm và lọc sản phẩm**

| Thành phần | Nội dung |
|---|---|
| Tác nhân | Khách vãng lai, Khách hàng |
| Điều kiện trước | Website hoạt động và có dữ liệu sản phẩm |
| Đầu vào | Từ khóa hoặc tiêu chí tìm kiếm/lọc |
| Xử lý | Tìm kiếm và lọc dữ liệu sản phẩm |
| Thành công | Hiển thị danh sách sản phẩm phù hợp |
| Không có kết quả | Hiển thị trạng thái phù hợp |

*Test case:* từ khóa tồn tại · từ khóa không tồn tại · dữ liệu rỗng · kiểm tra kết quả lọc.

**FR05 – Quản lý giỏ hàng**

| Thành phần | Nội dung |
|---|---|
| Tác nhân | Khách vãng lai, Khách hàng |
| Điều kiện trước | Có sản phẩm tồn tại trên hệ thống |
| Đầu vào | Sản phẩm và số lượng |
| Xử lý | Hệ thống cập nhật giỏ hàng |
| Thành công | Giỏ hàng hiển thị đúng sản phẩm và số lượng |

*Thao tác cần test:* thêm sản phẩm · xem giỏ hàng · đổi số lượng · xóa sản phẩm · kiểm tra tổng tiền. Đây là nhóm có nhiều khả năng xây thành Test Suite tự động.

**FR15 – Đặt hàng**

| Thành phần | Nội dung |
|---|---|
| Tác nhân | Khách hàng |
| Điều kiện trước | Đã đăng nhập và có sản phẩm trong giỏ |
| Đầu vào | Thông tin nhận hàng và thông tin đơn hàng |
| Xử lý | Hệ thống kiểm tra dữ liệu và tạo đơn |
| Thành công | Đơn hàng được tạo |
| Thất bại | Báo lỗi và yêu cầu người dùng điều chỉnh |

*Luồng test chính:* Đăng nhập → Chọn sản phẩm → Thêm vào giỏ → Kiểm tra giỏ hàng → Nhập thông tin nhận hàng → Chọn phương thức thanh toán → Xác nhận → Kiểm tra đơn hàng.

**FR20 – Quản lý sản phẩm**

| Thành phần | Nội dung |
|---|---|
| Tác nhân | Quản trị viên |
| Điều kiện trước | Quản trị viên đã đăng nhập |
| Đầu vào | Thông tin sản phẩm |
| Xử lý | Thêm, sửa hoặc cập nhật sản phẩm |
| Thành công | Thông tin sản phẩm được cập nhật |

*Test case:* thêm sản phẩm hợp lệ · thêm sản phẩm thiếu dữ liệu · sửa thông tin · cập nhật giá · cập nhật số lượng · đổi trạng thái · kiểm tra sản phẩm sau khi cập nhật.

---

## 9. Phân tích hệ thống (UML)

- **Tác nhân:** Guest (khách vãng lai), Customer (khách hàng), Admin (quản trị viên)
- **Use Case Diagram:** biểu diễn quan hệ giữa tác nhân và chức năng, xác định phạm vi sử dụng của từng nhóm người dùng
- **Đặc tả use case chính:**

| Mã | Use case |
|---|---|
| UC01 | Đăng nhập |
| UC02 | Quản lý giỏ hàng |
| UC03 | Đặt hàng |
| UC04 | Quản lý sản phẩm |
| UC05 | Quản lý đơn hàng |

- **Sequence Diagram:** Đăng nhập · Đặt hàng · Quản lý sản phẩm

> Các sơ đồ (Use Case, Sequence, Class, ERD, kiến trúc, Wireframe) là hình ảnh trong file Word gốc, không có dạng văn bản nên không được chép vào README này.

---

## 10. Thiết kế hệ thống

### 10.1. Kiến trúc hệ thống (MVC – ASP.NET MVC5)

| Thành phần | Vai trò |
|---|---|
| Người dùng | Tương tác với website qua trình duyệt |
| Trình duyệt | Hiển thị giao diện và gửi yêu cầu đến hệ thống |
| **View** | Hiển thị giao diện, nhận dữ liệu từ người dùng |
| **Controller** | Tiếp nhận yêu cầu, điều phối và trả kết quả |
| **Model** | Xử lý dữ liệu và nghiệp vụ |
| Cơ sở dữ liệu | Lưu tài khoản, sản phẩm, đơn hàng và các dữ liệu hệ thống |
| **Katalon Studio** | Chạy kiểm thử tự động trên giao diện web, ghi nhận kết quả |

Katalon Studio hoạt động **bên ngoài ứng dụng**, thao tác trên trình duyệt như người dùng thật, nên kịch bản bám theo luồng thực tế: đăng nhập, tìm kiếm, giỏ hàng, đặt hàng.

### 10.2. Class Diagram / CSDL (13 lớp)

| STT | Lớp | Vai trò |
|---|---|---|
| 1 | NguoiDung | Tài khoản khách hàng và quản trị viên |
| 2 | DanhMuc | Danh mục sản phẩm |
| 3 | NhaCungCap | Thông tin nhà cung cấp |
| 4 | SanPham | Thông tin sản phẩm |
| 5 | PhieuNhap | Phiếu nhập hàng |
| 6 | ChiTietPhieuNhap | Chi tiết sản phẩm trong phiếu nhập |
| 7 | DonHang | Thông tin đơn hàng |
| 8 | ChiTietDonHang | Sản phẩm thuộc từng đơn hàng |
| 9 | GioHang | Sản phẩm trong giỏ hàng của người dùng |
| 10 | DanhGiaSanPham | Đánh giá và số sao của người dùng |
| 11 | LienHe | Thông tin liên hệ, phản hồi của khách hàng |
| 12 | Banner | Hình ảnh/banner trên giao diện |
| 13 | ThanhToan | Giao dịch thanh toán của đơn hàng |

#### Thuộc tính chính của từng lớp (PK = khóa chính, FK = khóa ngoại)

<details>
<summary><b>NguoiDung</b></summary>

| Thuộc tính | Ý nghĩa | Khóa |
|---|---|---|
| MaNguoiDung | Mã định danh người dùng | PK |
| HoTen | Họ và tên | |
| Email | Email đăng nhập / liên hệ | |
| MatKhau | Mật khẩu | |
| SoDienThoai | Số điện thoại | |
| DiaChi | Địa chỉ | |
| VaiTro | Loại tài khoản (khách hàng / quản trị viên) | |
| AnhDaiDien | Ảnh đại diện | |
| ConHoatDong | Trạng thái hoạt động | |
| NgayTao | Ngày tạo tài khoản | |
</details>

<details>
<summary><b>DanhMuc</b></summary>

| Thuộc tính | Ý nghĩa | Khóa |
|---|---|---|
| MaDanhMuc | Mã danh mục | PK |
| TenDanhMuc | Tên danh mục | |
| MoTa | Mô tả | |
| HinhAnh | Hình ảnh đại diện | |
| ConHoatDong | Trạng thái hoạt động | |
</details>

<details>
<summary><b>NhaCungCap</b></summary>

| Thuộc tính | Ý nghĩa | Khóa |
|---|---|---|
| MaNhaCungCap | Mã nhà cung cấp | PK |
| TenNhaCungCap | Tên nhà cung cấp | |
| NguoiLienHe | Người đại diện liên hệ | |
| SoDienThoai | Số điện thoại | |
| Email | Email liên hệ | |
| DiaChi | Địa chỉ | |
| ConHoatDong | Trạng thái hợp tác | |
</details>

<details>
<summary><b>SanPham</b></summary>

| Thuộc tính | Ý nghĩa | Khóa |
|---|---|---|
| MaSanPham | Mã sản phẩm | PK |
| TenSanPham | Tên sản phẩm | |
| MaDanhMuc | Danh mục của sản phẩm | FK |
| MaNhaCungCap | Nhà cung cấp của sản phẩm | FK |
| MoTa | Mô tả chi tiết | |
| GiaNhap | Giá nhập | |
| GiaBan | Giá bán | |
| PhanTramGiam | Phần trăm giảm giá | |
| SoLuongTon | Số lượng tồn kho | |
| DonViTinh | Đơn vị tính | |
| HinhAnh | Hình ảnh | |
| ConHoatDong | Trạng thái kinh doanh | |
| NgayTao | Ngày tạo | |
</details>

<details>
<summary><b>PhieuNhap</b></summary>

| Thuộc tính | Ý nghĩa | Khóa |
|---|---|---|
| MaPhieuNhap | Mã định danh phiếu nhập | PK |
| MaPhieu | Mã phiếu để hiển thị/tra cứu | |
| MaNhaCungCap | Nhà cung cấp của phiếu nhập | FK |
| NguoiNhap | Người nhập hàng | |
| TongTien | Tổng giá trị phiếu nhập | |
| GhiChu | Ghi chú | |
| NgayNhap | Ngày lập phiếu | |
</details>

<details>
<summary><b>ChiTietPhieuNhap</b></summary>

| Thuộc tính | Ý nghĩa | Khóa |
|---|---|---|
| MaChiTiet | Mã chi tiết phiếu nhập | PK |
| MaPhieuNhap | Phiếu nhập chứa dòng chi tiết | FK |
| MaSanPham | Sản phẩm được nhập | FK |
| SoLuong | Số lượng nhập | |
| DonGiaNhap | Đơn giá nhập | |
| ThanhTien | Thành tiền | |
</details>

<details>
<summary><b>DonHang</b></summary>

| Thuộc tính | Ý nghĩa | Khóa |
|---|---|---|
| MaDonHang | Mã định danh đơn hàng | PK |
| MaDon | Mã đơn để hiển thị/tra cứu | |
| MaNguoiDung | Người dùng đặt đơn | FK |
| TenKhachHang | Tên khách nhận hàng | |
| SoDienThoai | Số điện thoại người nhận | |
| DiaChiGiao | Địa chỉ giao hàng | |
| TongTienHang | Tổng tiền hàng | |
| PhiVanChuyen | Phí vận chuyển | |
| TongThanhToan | Tổng số tiền phải thanh toán | |
| TrangThai | Trạng thái xử lý đơn | |
| HinhThucThanhToan | Hình thức thanh toán | |
| TrangThaiThanhToan | Trạng thái thanh toán | |
| GhiChu | Ghi chú | |
| NgayDat | Ngày đặt hàng | |
</details>

<details>
<summary><b>ChiTietDonHang</b></summary>

| Thuộc tính | Ý nghĩa | Khóa |
|---|---|---|
| MaChiTiet | Mã chi tiết đơn hàng | PK |
| MaDonHang | Đơn hàng chứa dòng chi tiết | FK |
| MaSanPham | Sản phẩm được đặt | FK |
| SoLuong | Số lượng đặt | |
| DonGiaBan | Đơn giá bán tại thời điểm đặt | |
| ThanhTien | Thành tiền | |
</details>

<details>
<summary><b>GioHang</b></summary>

| Thuộc tính | Ý nghĩa | Khóa |
|---|---|---|
| MaGioHang | Mã dòng giỏ hàng | PK |
| MaNguoiDung | Người dùng sở hữu giỏ hàng | FK |
| MaSanPham | Sản phẩm trong giỏ | FK |
| SoLuong | Số lượng trong giỏ | |
| NgayThem | Ngày thêm vào giỏ | |
</details>

<details>
<summary><b>DanhGiaSanPham</b></summary>

| Thuộc tính | Ý nghĩa | Khóa |
|---|---|---|
| MaDanhGia | Mã đánh giá | PK |
| MaSanPham | Sản phẩm được đánh giá | FK |
| MaNguoiDung | Người thực hiện đánh giá | FK |
| SoSao | Số sao | |
| NhanXet | Nội dung nhận xét | |
| NgayDanhGia | Ngày đánh giá | |
</details>

<details>
<summary><b>LienHe</b></summary>

| Thuộc tính | Ý nghĩa | Khóa |
|---|---|---|
| MaLienHe | Mã liên hệ | PK |
| HoTen | Họ tên người gửi | |
| Email | Email người gửi | |
| SoDienThoai | Số điện thoại người gửi | |
| TieuDe | Tiêu đề | |
| NoiDung | Nội dung liên hệ | |
| DaDoc | Trạng thái đã đọc | |
| PhanHoi | Nội dung phản hồi cho khách | |
| NgayGui | Ngày gửi | |
</details>

<details>
<summary><b>Banner</b></summary>

| Thuộc tính | Ý nghĩa | Khóa |
|---|---|---|
| MaBanner | Mã banner | PK |
| TieuDe | Tiêu đề | |
| HinhAnh | Hình ảnh | |
| DuongDan | Đường dẫn khi nhấn vào banner | |
| ConHoatDong | Trạng thái hiển thị | |
| ThuTuHienThi | Thứ tự hiển thị | |
</details>

<details>
<summary><b>ThanhToan</b></summary>

| Thuộc tính | Ý nghĩa | Khóa |
|---|---|---|
| MaThanhToan | Mã giao dịch thanh toán | PK |
| MaDonHang | Đơn hàng được thanh toán | FK |
| Provider | Đơn vị/cổng thanh toán | |
| MaGiaoDich | Mã giao dịch do đơn vị thanh toán trả về | |
| SoTien | Số tiền thanh toán | |
| TrangThai | Trạng thái giao dịch | |
| NgayThanhToan | Ngày thanh toán | |
| RawJson | Dữ liệu phản hồi gốc (JSON) từ đơn vị thanh toán | |
</details>

#### Quan hệ chính giữa các bảng

```
DanhMuc 1───n SanPham n───1 NhaCungCap
NhaCungCap 1───n PhieuNhap 1───n ChiTietPhieuNhap n───1 SanPham
NguoiDung 1───n DonHang 1───n ChiTietDonHang n───1 SanPham
NguoiDung 1───n GioHang n───1 SanPham
NguoiDung 1───n DanhGiaSanPham n───1 SanPham
DonHang 1───n ThanhToan
```
*(Suy ra từ các khóa ngoại trong bảng thuộc tính; chiều quan hệ 1–n là suy luận từ cấu trúc FK.)*

### 10.3. Thiết kế giao diện & Wireframe

**Mục tiêu:** xác định bố cục và vị trí các thành phần giao diện trước khi xây dựng giao diện hoàn chỉnh, đồng thời xác định các thành phần sẽ được kiểm thử tự động.

**Giao diện khách hàng**

| Trang | Nội dung |
|---|---|
| Trang chủ | Nội dung chính và sản phẩm của website |
| Danh sách sản phẩm | Hiển thị sản phẩm, hỗ trợ tìm kiếm/lọc |
| Chi tiết sản phẩm | Thông tin chi tiết và các thao tác liên quan |
| Giỏ hàng | Sản phẩm đã chọn, số lượng, tổng tiền |
| Đăng nhập / Đăng ký | Đăng nhập hoặc tạo tài khoản |
| Đặt hàng | Nhập thông tin giao hàng, chọn phương thức thanh toán |
| Thông tin đơn hàng | Xem thông tin và trạng thái đơn |
| Thông tin cá nhân | Quản lý thông tin tài khoản |

**Giao diện quản trị viên:** quản lý danh mục · sản phẩm · đơn hàng · tài khoản khách hàng · nội dung/banner · báo cáo và thống kê.

**Thành phần có trong Wireframe:** thanh điều hướng, khu vực nội dung, khu vực tìm kiếm, danh sách sản phẩm, các nút thao tác, khu vực thông tin người dùng, khu vực giỏ hàng, biểu mẫu nhập thông tin, khu vực hiển thị trạng thái/kết quả xử lý. Wireframe tập trung vào **bố cục và chức năng**, không chú trọng màu sắc/hình ảnh.

**Nhận xét:** các thành phần như nút đăng nhập, ô tìm kiếm, sản phẩm, giỏ hàng, biểu mẫu đặt hàng và chức năng quản trị là đối tượng để xây dựng kịch bản Katalon; Wireframe cũng là cơ sở đối chiếu giữa yêu cầu chức năng và giao diện thực tế khi viết Test Case.

---

## 11. Hướng kiểm thử với Katalon Studio

Tổng hợp các chức năng ưu tiên tự động hóa và test case dự kiến (rút từ phần SRS và phi chức năng):

| Chức năng | Mã | Các kịch bản dự kiến |
|---|---|---|
| Đăng nhập | FR10 | Hợp lệ · sai tài khoản · sai mật khẩu · bỏ trống từng trường / cả hai · kiểm tra thông báo lỗi |
| Tìm kiếm & lọc | FR03 | Từ khóa tồn tại · không tồn tại · rỗng · kết quả lọc |
| Giỏ hàng | FR05 | Thêm · xem · đổi số lượng · xóa · kiểm tra tổng tiền |
| Đặt hàng (COD) | FR15–17 | Luồng đầy đủ từ đăng nhập đến kiểm tra đơn; thiếu thông tin nhận hàng |
| Quản lý sản phẩm (admin) | FR20 | Thêm hợp lệ / thiếu dữ liệu · sửa · cập nhật giá, số lượng, trạng thái |
| Phân quyền | NFR02 | Truy cập chức năng cần đăng nhập khi chưa đăng nhập · khách hàng vào khu quản trị |
| Dữ liệu đầu vào | NFR03 | Nhập dữ liệu không hợp lệ vào các biểu mẫu |

**Tổ chức test:** Test Case → Test Suite → Test Suite Collection, để chạy hồi quy khi website thay đổi. Mỗi lần chạy cần ghi rõ **trình duyệt và môi trường**, kết quả theo dạng **Passed / Failed**.

> **Trạng thái báo cáo hiện tại:** file Word mới hoàn thành Chương 1 (khảo sát, lập kế hoạch) và Chương 2 (phân tích, thiết kế). Bộ Test Case, kịch bản Katalon và kết quả kiểm thử chưa có trong tài liệu này.
