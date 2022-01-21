# Response

## 資料庫設計

### 資料表

* Pharmacy
  * Id
  * Name
  * CashBalance
  * OpeningHours

* Mask
  * Id
  * Name

* Inventory
  * Id
  * PharmacyId
  * MaskId
  * Price

* User
  * Id
  * Name
  * CashBalance

* PurchaseHistory
  * PharmacyId
  * MaskId
  * TransactionAmount
  * TransactionDate

### 資料庫轉model指令

Scaffold-DbContext "Server=localhost;Database=Pharmacy;User ID=sa;Password=taipay.mssql.5517;Trusted_Connection=True;Integrated Security=False;" Microsoft.EntityFrameworkCore.SqlServer -OutputDir Models/pharmacy -DataAnnotations -Context Context  -UseDatabaseNames -CoNtextDir Data -force

### 關聯圖

![relationShip](https://i.imgur.com/LPkhcmB.png)

### 建立SQL server資料庫指令

* 在SQL server建立路徑中執行指令 : `..\document\sql script.script`

## Required
### API Document

* 請先匯入以下資料進postman
  * 匯入原始資料進資料庫用:`..\document\Import.postman_collection.json`
  * 指定完成題目:`..\document\Query.postman_collection.json`

### Import Data Commands

* 使用postman中Import兩個功能: `匯入PharmacyData`、`匯入UserData`
  (已代入參數不必再選擇原始檔案)

## Bonus
### Test Coverage Report
  check report [here](#test-coverage-report)

### Dockerized
  check my dockerfile [here](#dockerized)

### Demo Site Url
  demo site is ready on [heroku](#demo-site-url)
