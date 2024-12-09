using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GeneUIManager : MonoBehaviour
{
    [SerializeField] public TMP_InputField inputField_InitialAMRCount;
    [SerializeField] public TMP_InputField inputField_MaxAMRCount;
    [SerializeField] public TMP_InputField inputField_PopulationSize;
    [SerializeField] public TMP_InputField inputField_MaxGenerations;


    private PopulationManager populationManager;

    private void Start()
    {
        // 獲取 PopulationManager 參考
        populationManager = FindObjectOfType<PopulationManager>();

    }

    public void OnRunButtonClicked()
    {
        if (populationManager == null)
        {
            Debug.LogError("PopulationManager not found!");
            return;
        }

        // 讀取輸入值並轉換為整數
        int initialAMRCount = int.Parse(inputField_InitialAMRCount.text);
        int maxAMRCount = int.Parse(inputField_MaxAMRCount.text);
        int populationSize = int.Parse(inputField_PopulationSize.text);
        int maxGenerations = int.Parse(inputField_MaxGenerations.text);

        // 傳遞值到 PopulationManager
        populationManager.SetParameters(initialAMRCount, maxAMRCount, populationSize, maxGenerations);

        // 啟動基因演算法
        populationManager.RunOptimization();
    }
}

