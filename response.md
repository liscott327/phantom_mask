# Response
  Current contest as an example. Feel free to edit/remove it.


1.列出在特定時間開放的所有藥店，如果需要，可以在一周中的某一天開放
2.列出給定藥房出售的所有口罩，按口罩名稱或口罩價格排序
3.列出在一個價格範圍內擁有多於或少於 x 個面膜產品的所有藥店
4.某個日期範圍內按掩碼交易總額排名前 x 的用戶
5.某個日期範圍內發生的交易的掩碼總數和美元價值
6.按名稱搜索藥房或口罩，按與搜索詞的相關性排名
7.處理用戶從藥房購買口罩，並處理原子事務中的所有相關數據更改


## 設計需求
1.參數startTime、endTime、ServiceDay，查詢起始時間~結束時間或星期幾營業
2.參數:PharmacyName、orderBy(price、name)
3.minPrice、maxPrice
4.


## 資料庫設計

* 藥局(pharmacy)
  * id
  * name
  * cashBalance
  * openingHours

* 商品(mask)
  * id
  * name

* 庫存(inventory)
  * id
  * pharmacyId
  * maskId
  * price

* 使用者(user)
  * id
  * name
  * cashBalance

* purchaseHistory(付款紀錄)
  * pharmacyId
  * maskId
  * transactionAmount
  * transactionDate






## Required
### API Document
  Import [this](#api-document) json file to Postman

### Import Data Commands
  `rake import_data:book_store[PATH_TO_FILE]`
  `rake import_data:user[PATH_TO_FILE]`

## Bonus
### Test Coverage Report
  check report [here](#test-coverage-report)

### Dockerized
  check my dockerfile [here](#dockerized)

### Demo Site Url
  demo site is ready on [heroku](#demo-site-url)
