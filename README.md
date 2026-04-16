# Mini ERP — Quản lý bán hàng & kho

**Mục tiêu:** Xây dựng MVP ERP nhỏ cho doanh nghiệp vừa và nhỏ, quản lý sản phẩm, kho, đơn nhập/xuất, đơn bán.  
**Ứng dụng:** Dùng để demo kỹ năng ASP.NET Core, EF Core, JWT, Docker, CI/CD trong CV intern.

---

## 🚀 Tech stack
- **Backend:** ASP.NET Core 7 Web API, Entity Framework Core
- **Auth:** ASP.NET Identity + JWT
- **Database:** SQL Server (Docker container)
- **Frontend:** React + TypeScript (placeholder)
- **Infra:** Docker Compose, GitHub Actions CI/CD
- **Docs:** Swagger UI, OpenAPI YAML

---

## ⚙️ Quick start (local)
1. Clone repo:
   ```bash
   git clone https://github.com/yourname/MiniErp.git
   cd MiniErp
Copy .env.example → .env và chỉnh mật khẩu DB.

Run Docker Compose:

bash
docker-compose up --build
API chạy tại: http://localhost:5000/api/v1

Swagger UI: http://localhost:5000/swagger

🔑 Default accounts
Admin: admin@mini-erp.local / Admin@123  
(seeded qua Identity, đổi mật khẩu sau khi login)

📖 Demo luồng end-to-end
Login:

POST /api/v1/auth/login với admin credentials → nhận JWT token.

Tạo sản phẩm:

POST /api/v1/products với JWT → thêm sản phẩm mới.

Nhập kho:

POST /api/v1/purchaseorders → tạo đơn nhập.

POST /api/v1/purchaseorders/{id}/receive → cộng tồn kho.

Xem tồn kho:

GET /api/v1/inventory/{productId} → kiểm tra số lượng.

Tạo đơn bán:

POST /api/v1/salesorders → tạo đơn bán.

POST /api/v1/salesorders/{id}/ship → xuất kho.

Xem đơn bán:

GET /api/v1/salesorders/{id} → chi tiết đơn.

📊 API docs
Swagger UI: /swagger

OpenAPI contract: [Looks like the result wasn't safe to show. Let's switch things up and try something else!]

🛠️ CI/CD
GitHub Actions pipeline: .github/workflows/ci.yml

Build & test

Docker build

(Optional) Push image to Docker Hub / Azure Container Registry

📂 Project structure
Code
MiniErp/
├── MiniErp.Api/        # ASP.NET Core Web API
├── MiniErp.Tests/      # Unit & integration tests
├── frontend/           # React frontend (placeholder)
├── docs/               # ERD + OpenAPI
├── .github/workflows/  # CI/CD pipeline
├── docker-compose.yml
├── README.md
└── TECH.md