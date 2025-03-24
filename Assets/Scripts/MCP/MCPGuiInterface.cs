using UnityEngine;
using UnityEngine.Networking;
using System.Text;
using System.Collections;

public class MCPGuiInterface : MonoBehaviour
{
    private string contextJson = "{\"robots\":[]}";
    private string aiResponse = "(等待 AI 回應...)";
    private string toolName = "start_simulation";
    private string toolParams = "{}";
    private Vector2 scroll;

    public string mcpUrl = "http://localhost:3000/mcp";
    private bool showGUI = false;

    void OnGUI()
    {
        if (GUI.Button(new Rect(20, 430, 100, 30), showGUI ? "隱藏 MCP" : "顯示 MCP"))
        {
            showGUI = !showGUI;
        }

        if (!showGUI) return;

        GUI.Box(new Rect(10, 50, 360, 360), "🧠 MCP 控制面板");

        GUI.Label(new Rect(20, 80, 120, 20), "Context JSON:");
        contextJson = GUI.TextArea(new Rect(20, 100, 320, 60), contextJson);

        if (GUI.Button(new Rect(20, 170, 160, 25), "📤 Send Context To AI"))
        {
            StartCoroutine(SendContext(contextJson));
        }

        GUI.Label(new Rect(20, 205, 100, 20), "Tool 名稱:");
        toolName = GUI.TextField(new Rect(120, 205, 180, 20), toolName);

        GUI.Label(new Rect(20, 235, 120, 20), "Tool 參數(JSON):");
        toolParams = GUI.TextArea(new Rect(20, 255, 320, 50), toolParams);

        if (GUI.Button(new Rect(20, 315, 160, 25), "⚙️ 執行 MCP Tool"))
        {
            StartCoroutine(SendToolCommand(toolName, toolParams));
        }

        GUI.Label(new Rect(20, 350, 100, 20), "AI 回應:");
        GUI.Box(new Rect(20, 370, 320, 30), aiResponse);
    }

    IEnumerator SendContext(string json)
    {
        UnityWebRequest request = new UnityWebRequest(mcpUrl + "/context", "POST");
        byte[] body = Encoding.UTF8.GetBytes(json);
        request.uploadHandler = new UploadHandlerRaw(body);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
            aiResponse = request.downloadHandler.text;
        else
            aiResponse = "錯誤: " + request.error;
    }

    IEnumerator SendToolCommand(string tool, string json)
    {
        UnityWebRequest request = new UnityWebRequest(mcpUrl + "/" + tool, "POST");
        byte[] body = Encoding.UTF8.GetBytes(json);
        request.uploadHandler = new UploadHandlerRaw(body);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
            aiResponse = "✅ Tool 執行成功: " + tool;
        else
            aiResponse = "❌ Tool 執行失敗: " + request.error;
    }
}