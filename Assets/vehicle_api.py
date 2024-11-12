from flask import Flask, request, jsonify
import json

# Flask 是用 Python 編寫的微型網頁框架。
# 它可以讓你創建網頁服務器以托管 API 並管理請求和響應。

app = Flask(__name__)

# 模擬的車輛數據庫
# 這是一個簡單的列表，充當數據庫來存儲車輛信息。
# 在現實世界中，通常會使用 SQLite、MySQL 或 MongoDB 這樣的數據庫。
vehicle_data = [
    {
        "vehicleName": "Vehicle 1",
        "speed": 3.5,
        "batteryLevel": 85.0,
        "status": "Idle"
    },
    {
        "vehicleName": "Vehicle 2",
        "speed": 5.0,
        "batteryLevel": 60.0,
        "status": "Moving"
    },
    {
        "vehicleName": "Vehicle 3",
        "speed": 2.0,
        "batteryLevel": 45.0,
        "status": "Charging"
    }
]

# 獲取所有車輛數據的端點
# 此端點返回所有車輛及其信息的 JSON 列表。
# 它使用 HTTP GET 方法來檢索數據。
@app.route('/vehicles', methods=['GET'])
def get_vehicles():
    return jsonify(vehicle_data)

# 根據索引獲取特定車輛數據的端點
# 此端點根據車輛在列表中的索引返回特定車輛的信息。
# 如果請求的索引超出範圍，則返回 404 錯誤。
@app.route('/vehicles/<int:vehicle_index>', methods=['GET'])
def get_vehicle(vehicle_index):
    if 0 <= vehicle_index < len(vehicle_data):
        return jsonify(vehicle_data[vehicle_index])
    else:
        return jsonify({"error": "Vehicle not found"}), 404

# 更新特定車輛數據的端點
# 此端點允許使用 HTTP PUT 方法更新特定車輛的詳細信息。
# 你可以在請求的主體中發送新的 JSON 數據對象。
@app.route('/vehicles/<int:vehicle_index>', methods=['PUT'])
def update_vehicle(vehicle_index):
    if 0 <= vehicle_index < len(vehicle_data):
        vehicle = vehicle_data[vehicle_index]
        data = request.get_json()
        vehicle.update(data)
        return jsonify(vehicle)
    else:
        return jsonify({"error": "Vehicle not found"}), 404

# 腳本的主入口。
# 這段代碼運行 Flask 服務器，使用主機 0.0.0.0 和端口 5000 並啟用調試模式。
# 在生產環境中應該關閉調試模式。
if __name__ == '__main__':
    app.run(debug=True, host='0.0.0.0', port=5001)

"""
後端初學者說明：

1. **Flask 框架**：
   - Flask 是 Python 中的輕量級網頁框架，可以幫助你構建網頁應用程序和 REST API。
   - `Flask(__name__)` 創建一個應用對象，用來管理路由和處理請求。

2. **API 端點**：
   - API（應用程序接口）的目的是允許不同軟件應用程序之間的通信。這裡，我們的 API 提供車輛信息。
   - 端點（`/vehicles`、`/vehicles/<int:vehicle_index>`）是定義訪問數據的 URL。

3. **GET 方法**：
   - GET 方法用於從服務器檢索信息。例如，`/vehicles` 返回所有車輛的列表。
   - 端點 `/vehicles/<int:vehicle_index>` 返回基於索引的特定車輛信息。
   - 如果找不到車輛，則返回 `404` 狀態碼，表示“未找到”。

4. **PUT 方法**：
   - PUT 方法用於更新現有信息。例如，如果車輛的狀態或速度發生變化，可以使用此方法更新。
   - 你可以在請求主體中發送包含更新信息的 JSON 對象，然後用於更新車輛數據。

5. **模擬數據庫**：
   - `vehicle_data` 列表模擬了一個簡單的數據庫，每個字典表示一輛車輛。
   - 在現實應用中，會使用更健壯的數據庫來管理數據持久性。

6. **如何測試**：
   - 你可以使用像 `Postman` 這樣的工具或者簡單使用 `curl` 來測試這些端點。
   - 例如，`curl http://localhost:5000/vehicles` 會返回所有車輛的列表。
   - 要更新車輛：`curl -X PUT -H "Content-Type: application/json" -d '{"status": "Moving"}' http://localhost:5000/vehicles/0`。

7. **服務器配置**：
   - `app.run(debug=True, host='0.0.0.0', port=5000)` 運行服務器。
   - `debug=True` 在開發過程中很有用，因為它提供詳細的錯誤消息。但在生產環境中應關閉。
   - `host='0.0.0.0'` 表示服務器可以從任何 IP 地址訪問，這對於在多個設備上測試很有用。
"""
