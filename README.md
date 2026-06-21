# 圖書管理系統 RESTful API (Library Management API)

這是基於 **.NET 9** 與 **ASP.NET Core Web API** 開發的圖書管理系統後端服務。專案採用「三層式架構 (N-Tier Architecture)」設計，並實作了 JWT 身分驗證、關聯式資料庫查詢優化以及 RESTful 規範。

## 技術棧 (Tech Stack)
* **框架：** .NET 9 / ASP.NET Core Web API
* **語言：** C# 
* **資料庫：** SQL Server
* **ORM：** Entity Framework Core (EF Core)
* **資安與身分驗證：** JWT (JSON Web Tokens) Bearer Authentication
* **架構模式：** Controller-Service Pattern, Dependency Injection (DI)

##  核心功能 (Features)
* **身分驗證與授權 (Auth & Authorization)：**
  * 實作 JWT 發放與驗證機制。
  * 支援 Role-based 權限控管 (如透過 `[Authorize(Roles = "Admin")]` 區分管理員與一般讀者)。
* **圖書與分類管理 (Books & Categories)：**
  * 標準 CRUD 操作，嚴格遵守 RESTful API 設計原則 (HTTP GET, POST, PUT, DELETE 與對應之 200, 201, 401 狀態碼)。
  * 實作分頁機制 (Pagination) 與 `IQueryable` 延遲執行，將資料過濾交由資料庫引擎處理，優化伺服器記憶體消耗。
* **架構與防呆設計：**
  * 廣泛使用 DTO (Data Transfer Object) ，確保 API 契約與資料庫實體 (Entity) 徹底解耦，隱蔽機敏資料。


## 系統架構 (Architecture)
採用**關注點分離 (Separation of Concerns)** 原則：
1. **Controllers：** 負責接收 HTTP 請求、驗證路由參數，並回傳相對應的 HTTP 狀態碼。
2. **Services：** 封裝核心商業邏輯，處理資料的運算與防呆檢核。
3. **Data Access (EF Core)：** 透過 DbContext 與 SQL Server 溝通，運用 Change Tracker 進行高效率的資料庫更新。
