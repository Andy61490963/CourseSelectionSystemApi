# Course Selection System 選課系統

這是一個以 ASP.NET Core Web Api 為基礎建構的簡易學生選課系統後端Api，結合 Dapper 作為 ORM，搭配 SQL Server 作為後端資料庫，實作 RESTful API 並支援 Swagger UI 文件展示。

---

## 專案簡介

- 使用 ASP.NET Core 8 + Web API
- 搭配 Dapper 進行 SQL 操作
- 採用 JWT 驗證與角色授權
- 提供學生註冊 / 課程查詢 / 選課功能...
- Swagger 產生 OpenAPI 說明文件
- 支援單元測試（xUnit）

---

## 專案架構

```text
CourseSelectionSystem/
├── Controllers/ // API 控制器（學生、課程等）
├── Enums/ // 角色列舉
├── Helper/ // 加密、Jwt Token 生成
├── Models/ // 資料模型
├── Services/ // 業務邏輯 Service
└── Program.cs // 入口與中介軟體註冊

CourseSelectionSystem.ApiTests/
├── CustomWebApplicationFactory/ // 模擬記憶體中執行的 Web API 主機
└── StudentRegisterTests/ // 學生註冊、取得列表、單一查詢的 xUnit 測試
```

## 開發環境

- Rider
- .NET 8 SDK
- SQL Server 2019+
- Swagger

---

## 資料庫結構圖

[點我查看資料庫 ER 圖（dbdiagram.io）](https://dbdiagram.io/d/6826c0455b2fc4582fdc2aac)
