using System.Collections.Generic;
using UnityEngine;
using System.IO; // 用於文件操作
using Debug = UnityEngine.Debug;

public class GeneticAlgorithmManager : MonoBehaviour
{
    public int populationSize = 20;
    public int geneLength = 9; // 9個客戶
    public int maxGenerations = 100;
    public float crossoverRate = 0.7f;
    public float mutationRate = 0.01f;

    public int vehicleCapacity = 100;
    public int maxDistance = 300;

    public float energyConsumptionPerUnitDistance = 0.1f; // 每單位距離的能耗
    public float timePerUnitDistance = 1.0f; // 每單位距離的耗時

    public List<int> customerDemands = new List<int> { 10, 20, 30, 40, 50, 60, 70, 80, 90 }; // 每個客戶需求量
    public List<List<int>> distanceMatrix = new List<List<int>> {
        new List<int>{ 0, 10, 20, 30, 40, 50, 60, 70, 80, 90 },
        new List<int>{ 10, 0, 15, 25, 35, 45, 55, 65, 75, 85 },
        new List<int>{ 20, 15, 0, 10, 20, 30, 40, 50, 60, 70 },
        new List<int>{ 30, 25, 10, 0, 15, 25, 35, 45, 55, 65 },
        new List<int>{ 40, 35, 20, 15, 0, 20, 30, 40, 50, 60 },
        new List<int>{ 50, 45, 30, 25, 20, 0, 10, 20, 30, 40 },
        new List<int>{ 60, 55, 40, 35, 30, 10, 0, 10, 20, 30 },
        new List<int>{ 70, 65, 50, 45, 40, 20, 10, 0, 10, 20 },
        new List<int>{ 80, 75, 60, 55, 50, 30, 20, 10, 0, 10 },
        new List<int>{ 90, 85, 70, 65, 60, 40, 30, 20, 10, 0 }
    };

    private Population population;
    private List<float> fitnessHistory = new List<float>(); // 保存每一代的最佳適應度
    private List<int> generationHistory = new List<int>(); // 保存每一代數

    void Start()
    {
        // 創建並打開文件進行寫入
        string filePath = Application.persistentDataPath + "/FitnessData.txt";
        using (StreamWriter writer = new StreamWriter(filePath, false))
        {
            writer.WriteLine("Generation,Fitness");

            population = new Population(populationSize, geneLength);
            for (int t = 0; t < maxGenerations; t++)
            {
                // 計算每個個體的適應度
                population.EvaluatePopulation(vehicleCapacity, maxDistance, customerDemands, distanceMatrix, energyConsumptionPerUnitDistance, timePerUnitDistance);

                // 記錄每一代的最佳適應度與當前代數
                float bestFitness = population.bestFitness;
                fitnessHistory.Add(bestFitness);
                generationHistory.Add(t + 1);

                // 寫入每一代的數據到文件
                writer.WriteLine($"{t + 1},{bestFitness}");

                // 顯示當前世代的最佳解以及每輛車的分配情況
                UnityEngine.Debug.Log($"Generation {t + 1}: Best Fitness = {bestFitness}");
                DisplayVehicleAssignments(t + 1, population.bestIndividual);

                // 進行進化
                population.Evolve(crossoverRate, mutationRate);
            }

            Debug.Log($"Data successfully written to {filePath}");
        }
    }

    // 顯示每輛車負責的客戶（包括世代和車輛分配）
    void DisplayVehicleAssignments(int generation, DNA individual)
    {
        int currentLoad = 0;
        int currentVehicleDistance = 0;
        int currentVehicleCapacity = vehicleCapacity;
        int lastCustomer = 0;
        int vehicleNumber = 1;

        // 初始化一個字串來顯示車輛分配情況
        string vehicleAssignmentMessage = $"Generation {generation} - Vehicle Assignments:\n";

        vehicleAssignmentMessage += $"Vehicle {vehicleNumber}: ";

        HashSet<int> servedCustomers = new HashSet<int>();

        for (int i = 0; i < individual.genes.Count; i++)
        {
            int customer = individual.genes[i];
            int demand = customerDemands[customer - 1];

            // 如果當前車輛無法負擔新的客戶，或者客戶已經被服務過，則換車
            if (currentLoad + demand > currentVehicleCapacity || currentVehicleDistance + distanceMatrix[lastCustomer][customer] > maxDistance || servedCustomers.Contains(customer))
            {
                vehicleAssignmentMessage += $"[Return to depot, total distance: {currentVehicleDistance}]\n";
                
                // 換車並重置數據
                vehicleNumber++;
                vehicleAssignmentMessage += $"Vehicle {vehicleNumber}: ";
                currentLoad = 0;
                currentVehicleDistance = 0;
                lastCustomer = 0; // 回到配送中心
            }

            // 累加當前車輛的負載與距離，並標記客戶已被服務
            vehicleAssignmentMessage += $"Customer {customer} (Demand: {demand}), ";
            currentLoad += demand;
            currentVehicleDistance += distanceMatrix[lastCustomer][customer];
            lastCustomer = customer;
            servedCustomers.Add(customer);
        }

        // 最後一輛車返回配送中心
        vehicleAssignmentMessage += $"[Return to depot, total distance: {currentVehicleDistance}]\n";

        // 確保所有客戶都被服務到
        for (int i = 1; i <= customerDemands.Count; i++)
        {
            if (!servedCustomers.Contains(i))
            {
                vehicleAssignmentMessage += $"Warning: Customer {i} was not served!\n";
            }
        }

        // 在 Console 中顯示完整的車輛分配訊息
        Debug.Log(vehicleAssignmentMessage);
    }
}

public class Population
{
    public List<DNA> individuals;
    public float bestFitness;
    public DNA bestIndividual;

    public Population(int populationSize, int geneLength)
    {
        individuals = new List<DNA>();
        for (int i = 0; i < populationSize; i++)
        {
            individuals.Add(new DNA(geneLength));
        }
    }

    public void EvaluatePopulation(int vehicleCapacity, int maxDistance, List<int> customerDemands, List<List<int>> distanceMatrix, float energyConsumptionPerUnitDistance, float timePerUnitDistance)
    {
        bestFitness = float.MinValue;
        foreach (var individual in individuals)
        {
            individual.CalculateFitness(vehicleCapacity, maxDistance, customerDemands, distanceMatrix, energyConsumptionPerUnitDistance, timePerUnitDistance);
            if (individual.fitness > bestFitness && individual.AllCustomersServed(customerDemands.Count))
            {
                bestFitness = individual.fitness;
                bestIndividual = individual;
            }
        }
    }

    public void Evolve(float crossoverRate, float mutationRate)
    {
        List<DNA> newPopulation = new List<DNA>();

        // Selection: Roulette Wheel Selection
        float totalFitness = 0;
        foreach (var individual in individuals)
        {
            totalFitness += individual.fitness;
        }

        for (int i = 0; i < individuals.Count; i++)
        {
            DNA parent1 = SelectParent(totalFitness);
            DNA parent2 = SelectParent(totalFitness);

            DNA child = Crossover(parent1, parent2, crossoverRate);
            child.Mutate(mutationRate);
            newPopulation.Add(child);
        }

        individuals = newPopulation;
    }

    private DNA SelectParent(float totalFitness)
    {
        float randomValue = Random.Range(0, totalFitness);
        float runningSum = 0;

        foreach (var individual in individuals)
        {
            runningSum += individual.fitness;
            if (runningSum >= randomValue)
            {
                return individual;
            }
        }

        return individuals[individuals.Count - 1];
    }

    private DNA Crossover(DNA parent1, DNA parent2, float crossoverRate)
    {
        DNA child = new DNA(parent1.genes.Count);
        for (int i = 0; i < parent1.genes.Count; i++)
        {
            child.genes[i] = Random.value < crossoverRate ? parent1.genes[i] : parent2.genes[i];
        }
        return child;
    }
}

public class DNA
{
    public List<int> genes;
    public float fitness;

    public DNA(int length)
    {
        genes = new List<int>();
        for (int i = 0; i < length; i++)
        {
            genes.Add(i + 1); // 客戶標號從1開始
        }
        ShuffleGenes();
    }

    private void ShuffleGenes()
    {
        System.Random rand = new System.Random();
        for (int i = genes.Count - 1; i > 0; i--)
        {
            int j = rand.Next(i + 1);
            int temp = genes[i];
            genes[i] = genes[j];
            genes[j] = temp;
        }
    }

    public void Mutate(float mutationRate)
    {
        System.Random rand = new System.Random();
        for (int i = 0; i < genes.Count; i++)
        {
            if (rand.NextDouble() < mutationRate)
            {
                int swapIndex = rand.Next(genes.Count);
                int temp = genes[i];
                genes[i] = genes[swapIndex];
                genes[swapIndex] = temp;
            }
        }
    }

    public void CalculateFitness(int vehicleCapacity, int maxDistance, List<int> customerDemands, List<List<int>> distanceMatrix, float energyConsumptionPerUnitDistance, float timePerUnitDistance)
    {
        int currentLoad = 0;
        int currentVehicleDistance = 0;
        int currentVehicleCapacity = vehicleCapacity;
        int lastCustomer = 0;
        float totalEnergyConsumption = 0;
        float totalTime = 0;
        float totalDistance = 0;
        fitness = 0;

        HashSet<int> servedCustomers = new HashSet<int>();

        for (int i = 0; i < genes.Count; i++)
        {
            int customer = genes[i];
            int demand = customerDemands[customer - 1];

            // 如果當前車輛無法負擔新的客戶，則換車
            if (currentLoad + demand > currentVehicleCapacity || currentVehicleDistance + distanceMatrix[lastCustomer][customer] > maxDistance || servedCustomers.Contains(customer))
            {
                totalDistance += currentVehicleDistance;
                totalEnergyConsumption += currentVehicleDistance * energyConsumptionPerUnitDistance;
                totalTime += currentVehicleDistance * timePerUnitDistance;
                currentLoad = 0;
                currentVehicleDistance = 0;
                lastCustomer = 0;
            }

            // 累加當前車輛的負載與距離
            currentLoad += demand;
            currentVehicleDistance += distanceMatrix[lastCustomer][customer];
            totalEnergyConsumption += distanceMatrix[lastCustomer][customer] * energyConsumptionPerUnitDistance;
            totalTime += distanceMatrix[lastCustomer][customer] * timePerUnitDistance;
            lastCustomer = customer;
            servedCustomers.Add(customer);
        }

        // 最後一輛車返回配送中心
        totalDistance += currentVehicleDistance;
        totalEnergyConsumption += currentVehicleDistance * energyConsumptionPerUnitDistance;
        totalTime += currentVehicleDistance * timePerUnitDistance;

        // 綜合考慮能耗、距離和時間，計算適應度
        fitness = 10000 / (1 + totalDistance + totalEnergyConsumption + totalTime); // 適應度值越高表示越優
    }

    public bool AllCustomersServed(int totalCustomers)
    {
        HashSet<int> uniqueCustomers = new HashSet<int>(genes);
        return uniqueCustomers.Count == totalCustomers;
    }
}
