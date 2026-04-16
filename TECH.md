# Mini ERP — Technical Architecture & Trade-offs

## 🎯 Mục tiêu
- Xây dựng MVP ERP nhỏ cho doanh nghiệp vừa và nhỏ.
- Thể hiện kỹ năng full-stack: ASP.NET Core, EF Core, JWT, React, Docker, CI/CD.
- Có thể mở rộng thành hệ thống thực tế.

---

## 🏗️ Kiến trúc tổng quan
- **Backend:** ASP.NET Core Web API (modular monolith).
- **Auth:** ASP.NET Identity + JWT.
- **Database:** SQL Server (Docker container).
- **Frontend:** React + TypeScript (Vite).
- **Infra:** Docker Compose, GitHub Actions CI/CD.
- **Docs:** Swagger UI + OpenAPI YAML.

### Luồng chính
1. **Auth** → Login bằng admin → JWT.
2. **Products** → CRUD sản phẩm.
3. **PurchaseOrders** → tạo đơn nhập → nhận hàng → cộng tồn kho.
4. **Inventory** → xem tồn kho theo sản phẩm/kho.
5. **SalesOrders** → tạo đơn bán → ship → trừ tồn kho.

---

## 📂 Modules
- **Products**: quản lý SKU, name, price, category.
- **Inventory**: quản lý tồn kho theo warehouse.
- **SalesOrders**: đơn bán, ship → OUT stock.
- **PurchaseOrders**: đơn nhập, receive → IN stock.
- **Auth**: login/register, role Admin/Staff.

---

## ⚖️ Trade-offs
- **Monolith vs Microservices**: chọn modular monolith để đơn giản, dễ deploy. Có thể tách thành microservices (Product, Inventory, Orders) sau.
- **SQL Server vs PostgreSQL**: chọn SQL Server vì quen thuộc với .NET, nhưng PostgreSQL dễ dùng hơn với Docker. Có thể chuyển đổi sau.
- **Identity vs Custom Auth**: chọn ASP.NET Identity để có sẵn hashing, roles, token. Trade-off: hơi nặng cho MVP, nhưng an toàn.
- **React vs Blazor**: chọn React vì phổ biến, dễ deploy. Blazor phù hợp nếu muốn full .NET stack.
- **Docker Compose vs Kubernetes**: Compose đủ cho demo. Kubernetes phù hợp khi scale.

---

## 🔒 Bảo mật
- JWT secret lưu trong secrets, không commit.
- Password hash bằng ASP.NET Identity.
- Role-based authorization cho endpoints quan trọng.
- HTTPS bắt buộc khi deploy.

---

## 🛠️ CI/CD
- **GitHub Actions**: build, test, docker build.
- Badge hiển thị trong README.
- Có thể mở rộng: push image lên Docker Hub, deploy Azure App Service.

---

## 📈 Hướng mở rộng
- **Reporting**: thêm module báo cáo tồn kho, doanh thu.
- **Payments**: tích hợp Stripe/PayPal cho đơn bán.
- **Notifications**: email/SMS khi đơn nhập/xuất.
- **Frontend**: hoàn thiện UI/UX với Tailwind hoặc Material UI.
- **Microservices**: tách Inventory, Orders thành service riêng, dùng RabbitMQ để giao tiếp.
