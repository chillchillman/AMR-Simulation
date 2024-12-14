using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PopulationManager : MonoBehaviour {
    public int initialAMRCount = 1;
    public int maxAMRCount = 5;
    public int populationSize = 50;
    public int maxGenerations = 10;

    private float[,] distanceMatrix;
    private int generation;

    // 儲存最佳結果
    public DNA optimalDNA;
    public int optimalAMRCount;
    private float shortestTotalDistance;

    public GeneResultDisplayManager geneResultDisplayManager;

    void Start() {
        // 獲取距離矩陣
        distanceMatrix = WaypointManager.Instance.GenerateFullDistanceMatrix();

        // 開始優化流程
        RunOptimization();
    }

    public void SetParameters(int initialAMRCount, int maxAMRCount, int populationSize, int maxGenerations)
    {
        this.initialAMRCount = initialAMRCount;
        this.maxAMRCount = maxAMRCount;
        this.populationSize = populationSize;
        this.maxGenerations = maxGenerations;

        Debug.Log($"Parameters updated: InitialAMRCount:{initialAMRCount}, MaxAMRCount:{maxAMRCount}, PopulationSize:{populationSize}, MaxGenerations:{maxGenerations}");
    }

    public void RunOptimization() {
        optimalAMRCount = initialAMRCount;
        float bestFitness = float.MinValue;
        float previousShortestDistance = float.MaxValue;

        Debug.Log("Starting AMR optimization...");

        for (int amrCount = initialAMRCount; amrCount <= maxAMRCount; amrCount++) {
            Debug.Log($"Testing with {amrCount} AMRs...");

            // 使用基因演算法運行當前 AMR 數量的測試
            var (fitness, bestDNA) = RunGeneticAlgorithm(amrCount);

            // 獲取最佳總路徑長
            float currentShortestDistance = -fitness;

            // 檢查新增車輛是否有效縮短路徑且所有車輛均被使用
            if (currentShortestDistance < previousShortestDistance && AllVehiclesUsed(amrCount, bestDNA)) {
                bestFitness = fitness;
                optimalAMRCount = amrCount;
                optimalDNA = bestDNA;
                previousShortestDistance = currentShortestDistance;
            } else {
                Debug.Log($"Adding {amrCount} AMRs does not improve the total distance or not all vehicles are used.");
                break; // 停止新增車輛
            }
        }

        Debug.Log($"Optimal AMR Count: {optimalAMRCount}, Shortest Total Distance: {previousShortestDistance}");
        Debug.Log($"Optimal DNA Configuration: {optimalDNA}");

        DisplayOptimalDNA(optimalAMRCount);

        // 顯示最佳結果到 UI
        if (geneResultDisplayManager != null)
        {
            float totalDistance = -optimalDNA.Fitness;
            geneResultDisplayManager.DisplayResults(optimalAMRCount, optimalDNA, totalDistance);
        }

        // 調用生成車輛並設置導航路徑
        CarSpawnerAndRouteSetter carSpawner = FindObjectOfType<CarSpawnerAndRouteSetter>();
        if (carSpawner != null)
        {
            carSpawner.GenerateCarsAndSetRoutes();
        }
        else
        {
            Debug.LogError("CarSpawnerAndRouteSetter not found in the scene!");
        }

    }

    bool AllVehiclesUsed(int amrCount, DNA dna) {
        // 確保所有車輛都被分配到任務
        var assignedVehicles = new HashSet<int>();
        foreach (var gene in dna.GetGenes()) {
            assignedVehicles.Add(gene);
        }
        return assignedVehicles.Count == amrCount;
    }

    (float fitness, DNA bestDNA)RunGeneticAlgorithm(int amrCount) {
        List<DNA> population = InitializePopulation(amrCount);
        float bestFitness = float.MinValue;
        DNA bestDNA = null; //保存最佳基因排序

        for (generation = 1; generation <= maxGenerations; generation++) {
            // 計算適應度
            foreach (var dna in population) {
                dna.EvaluateFitness(distanceMatrix);
            }

            // 按適應度排序
            population = population.OrderByDescending(dna => dna.Fitness).ToList();

            // 打印最佳結果
            LogBestResult(population, amrCount);

            // 更新最佳適應度
            if (population[0].Fitness > bestFitness) {
                bestFitness = population[0].Fitness;
                bestDNA = population[0]; // 保存當前最佳基因
            }

            // 繁殖下一代
            population = BreedNewPopulation(population);
        }

        return (bestFitness, bestDNA);
    }

    List<DNA> BreedNewPopulation(List<DNA> sortedPopulation) {
        List<DNA> newPopulation = new List<DNA>();
        int half = sortedPopulation.Count / 2;

        // 確保有足夠個體進行繁殖
        if (sortedPopulation.Count < 2) {
            Debug.LogWarning("Population size is too small for breeding.");
            return sortedPopulation;
        }

        for (int i = half; i < sortedPopulation.Count; i++) {
            DNA parent1 = sortedPopulation[i];
            DNA parent2 = sortedPopulation[i - 1];

            DNA offspring = new DNA(parent1);
            offspring.Combine(parent1, parent2);

            if (Random.Range(0, 100) < 1) {
                offspring.Mutate();
            }

            newPopulation.Add(offspring);
        }

        return newPopulation;
    }

    List<DNA> InitializePopulation(int amrCount) {
        if (populationSize < 2) {
            Debug.LogError("Population size must be at least 2.");
            populationSize = 2; // 自動調整為最小值
        }

        List<DNA> population = new List<DNA>();
        for (int i = 0; i < populationSize; i++) {
            population.Add(new DNA(amrCount, WaypointManager.Instance.GetWaypointPositions().Count));
        }
        return population;
    }

    void LogBestResult(List<DNA> population, int amrCount) {
        DNA bestDNA = population[0];

        Debug.Log($"Generation {generation}, AMR Count: {amrCount}");
        Debug.Log($"Best Fitness: {bestDNA.Fitness} (Total Distance: {-bestDNA.Fitness})");
        Debug.Log($"Best DNA: {bestDNA}");
    }
    

    void DisplayOptimalDNA(int amrCount) {
        if (optimalDNA == null) {
           Debug.Log("No optimal DNA found.");
            return;
        }

        float totalDistance = -optimalDNA.Fitness;

        // 獲取最佳基因的分配路徑
        List<List<int>> amrRoutes = new List<List<int>>();
        for (int i = 0; i < amrCount; i++) {
            amrRoutes.Add(new List<int>());
       }
        for (int i = 0; i < optimalDNA.GetGenes().Count; i++) {
            amrRoutes[optimalDNA.GetGenes()[i]].Add(i + 1); // 分配到對應車輛
        }

        // 打印最佳基因的路徑排程
        Debug.Log("Optimal DNA Configuration:");
        Debug.Log($"Total Distance: {totalDistance}");

       for (int i = 0; i < amrRoutes.Count; i++) {
           if (amrRoutes[i].Count > 0) {
                string route = "Start -> ";
                route += string.Join(" -> ", amrRoutes[i].Select(index => $"Waypoint {index}"));
                route += " -> End";
               Debug.Log($"AMR {i + 1} Route: {route}");
           } else {
               Debug.Log($"AMR {i + 1} Route: Start -> End (No Waypoints)");
           }
       }
    }

}
