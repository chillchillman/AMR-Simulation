using TMPro;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class GeneResultDisplayManager : MonoBehaviour
{
    public TextMeshProUGUI resultText;
    private bool isTextVisible = false;
    public float typingSpeed = 0.0001f; // 每個字符的顯示速度

    public void DisplayResults(int amrCount, DNA optimalDNA, float totalDistance)
    {
        if (optimalDNA == null)
        {
            StartCoroutine(TypeText("No optimal DNA configuration found."));
            return;
        }

        // 構建顯示文本
        string result = "Optimal DNA Configuration:\n";
        result += $"Total Distance: {totalDistance:F2}\n";

        // 獲取基因分配路徑
        List<List<int>> amrRoutes = new List<List<int>>();
        for (int i = 0; i < amrCount; i++)
        {
            amrRoutes.Add(new List<int>());
        }

        for (int i = 0; i < optimalDNA.GetGenes().Count; i++)
        {
            int gene = optimalDNA.GetGenes()[i];
            if (gene < 0 || gene >= amrCount)
            {
                Debug.LogError($"Invalid gene value: {gene}");
                return;
            }
            amrRoutes[gene].Add(i + 1); // 分配到對應車輛
        }

        // 添加每輛車的路徑到結果文本
        for (int i = 0; i < amrRoutes.Count; i++)
        {
            if (amrRoutes[i].Count > 0)
            {
                string route = "Start -> ";
                route += string.Join(" -> ", amrRoutes[i].Select(index => $"Waypoint {index}"));
                route += " -> End";
                result += $"AMR {i + 1} Route: {route}\n";
            }
            else
            {
                result += $"AMR {i + 1} Route: Start -> End (No Waypoints)\n";
            }
        }

        // 啟動逐步顯示協程
        StartCoroutine(TypeText(result));
    }

    private IEnumerator TypeText(string textToDisplay)
    {
        string currentText = ""; // 內部緩存文本
        foreach (char c in textToDisplay)
        {
            currentText += c; // 更新緩存
            resultText.text = currentText; // 僅更新 UI
            yield return new WaitForSeconds(typingSpeed); // 等待設定的速度
        }
    }

    public void ToggleTextVisibility()
    {
        isTextVisible = !isTextVisible;
        resultText.gameObject.SetActive(isTextVisible);
    }
}
