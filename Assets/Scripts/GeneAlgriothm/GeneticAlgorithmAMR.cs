using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine;
using Debug = UnityEngine.Debug;
using System.Diagnostics;

public class GeneticAlgorithmAMR : MonoBehaviour
{
    // Parameters for Genetic Algorithm
    public int populationSize = 50;
    public int generations = 100;
    public float mutationRate = 0.05f;
    public int numberOfTasks = 10;
    public int numberOfAMRs = 3;

    private List<Chromosome> population;
    private System.Random random;

    // Distance matrix representing distances between different task locations
    private float[,] distanceMatrix;
    private List<Vector3> taskLocations;

    private bool settingUpLocations = true;

    void Start()
    {
        // Ensure the task plane has the correct tag
        GameObject taskPlane = GameObject.Find("TaskPlane");
        if (taskPlane != null)
        {
            taskPlane.tag = "Ground";
        }
        random = new System.Random();
        taskLocations = new List<Vector3>();
    }

    void Update()
    {
        // Left mouse click to add task locations
        if (Input.GetMouseButtonDown(0) && settingUpLocations)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit) && hit.collider.gameObject.CompareTag("Ground"))
            {
                taskLocations.Add(hit.point);
                Debug.Log("Added Task Location at: " + hit.point);
            }
        }

        // Right mouse click to finalize locations and start calculations
        if (Input.GetMouseButtonDown(1) && settingUpLocations)
        {
            if (taskLocations.Count < numberOfTasks)
            {
                Debug.LogWarning("Not enough task locations set. Please add more.");
                return;
            }
            settingUpLocations = false;
            InitializeDistanceMatrix();
            RunGeneticAlgorithm();
        }
    }

    // Initialize the distance matrix based on task locations
    void InitializeDistanceMatrix()
    {
        numberOfTasks = taskLocations.Count;
        distanceMatrix = new float[numberOfTasks, numberOfTasks];
        for (int i = 0; i < numberOfTasks; i++)
        {
            for (int j = 0; j < numberOfTasks; j++)
            {
                if (i == j)
                {
                    distanceMatrix[i, j] = 0;
                }
                else
                {
                    distanceMatrix[i, j] = Vector3.Distance(taskLocations[i], taskLocations[j]);
                }
            }
        }
        Debug.Log("Distance matrix initialized.");
    }

    // Run the Genetic Algorithm
    void RunGeneticAlgorithm()
    {
        InitializePopulation();

        Stopwatch stopwatch = new Stopwatch();
        stopwatch.Start();

        // Run Genetic Algorithm for a specified number of generations
        for (int generation = 0; generation < generations; generation++)
        {
            EvaluateFitness();
            PerformSelectionAndCrossover();
            ApplyMutation();
        }

        stopwatch.Stop();

        // Display best solution found
        Chromosome bestSolution = GetBestSolution();
        Debug.Log("Best Solution Found:\n" + GetVehicleSchedules(bestSolution));
        Debug.Log("Best Fitness: " + bestSolution.Fitness);
        Debug.Log("Final Task Allocation Details:\n" + GetVehicleSchedules(bestSolution));
        Debug.Log("Computation Time: " + stopwatch.ElapsedMilliseconds + " ms");
    }

    // Get the vehicle schedules from the chromosome
    string GetVehicleSchedules(Chromosome chromosome)
    {
        string result = "";
        for (int i = 0; i < chromosome.TasksPerAMR.Length; i++)
        {
            result += "AMR " + (i + 1) + " Schedule: ";
            foreach (int task in chromosome.TasksPerAMR[i])
            {
                result += "Task " + (task + 1) + " -> ";
            }
            result = result.TrimEnd(' ', '-', '>');
            result += "\n";
        }
        return result;
    }

    // Initialize population with random chromosomes
    void InitializePopulation()
    {
        population = new List<Chromosome>();
        for (int i = 0; i < populationSize; i++)
        {
            Chromosome chromosome = new Chromosome(numberOfTasks, numberOfAMRs, random, distanceMatrix);
            population.Add(chromosome);
        }
    }

    // Evaluate the fitness of each chromosome
    void EvaluateFitness()
    {
        foreach (Chromosome chromosome in population)
        {
            chromosome.CalculateFitness();
        }
    }

    // Perform selection and crossover to create a new generation
    void PerformSelectionAndCrossover()
    {
        List<Chromosome> newPopulation = new List<Chromosome>();

        // Sort population based on fitness (higher is better)
        population.Sort((a, b) => b.Fitness.CompareTo(a.Fitness));

        // Elitism - Keep the best chromosomes
        int eliteCount = populationSize / 10;
        for (int i = 0; i < eliteCount; i++)
        {
            newPopulation.Add(population[i]);
        }

        // Perform crossover to fill the rest of the population
        while (newPopulation.Count < populationSize)
        {
            Chromosome parent1 = SelectParent();
            Chromosome parent2 = SelectParent();
            Chromosome child = Chromosome.Crossover(parent1, parent2, random);
            newPopulation.Add(child);
        }

        population = newPopulation;
    }

    // Apply mutation to the population
    void ApplyMutation()
    {
        foreach (Chromosome chromosome in population)
        {
            chromosome.Mutate(mutationRate, random);
        }
    }

    // Select a parent using roulette wheel selection
    Chromosome SelectParent()
    {
        float totalFitness = 0;
        foreach (Chromosome chromosome in population)
        {
            totalFitness += chromosome.Fitness;
        }

        float randomPoint = (float)(random.NextDouble() * totalFitness);
        float currentSum = 0;

        foreach (Chromosome chromosome in population)
        {
            currentSum += chromosome.Fitness;
            if (currentSum >= randomPoint)
            {
                return chromosome;
            }
        }

        return population[population.Count - 1];
    }

    // Get the best solution from the current population
    Chromosome GetBestSolution()
    {
        Chromosome best = population[0];
        foreach (Chromosome chromosome in population)
        {
            if (chromosome.Fitness > best.Fitness)
            {
                best = chromosome;
            }
        }
        Debug.Log("Best Fitness: " + best.Fitness);
        return best;
    }
}

public class Chromosome
{
    public List<int>[] TasksPerAMR;
    public float Fitness { get; private set; }
    private float[,] distanceMatrix;

    public Chromosome(int numberOfTasks, int numberOfAMRs, System.Random random, float[,] distanceMatrix)
    {
        // Initialize tasks per AMR
        TasksPerAMR = new List<int>[numberOfAMRs];
        for (int i = 0; i < numberOfAMRs; i++)
        {
            TasksPerAMR[i] = new List<int>();
        }

        this.distanceMatrix = distanceMatrix;

        // Randomly assign tasks to AMRs ensuring all tasks are allocated
        List<int> taskList = new List<int>();
        for (int i = 0; i < numberOfTasks; i++)
        {
            taskList.Add(i);
        }

        int amrIndex = 0;
        while (taskList.Count > 0)
        {
            int task = taskList[random.Next(taskList.Count)];
            TasksPerAMR[amrIndex].Add(task);
            taskList.Remove(task);

            amrIndex = (amrIndex + 1) % numberOfAMRs; // Ensure tasks are distributed among AMRs
        }
    }

    // Calculate fitness of the chromosome based on task allocation
    public void CalculateFitness()
    {
        float totalDistance = 0;
        foreach (List<int> tasks in TasksPerAMR)
        {
            if (tasks.Count > 0)
            {
                totalDistance += CalculateTotalDistance(tasks);
            }
        }
        Fitness = 1.0f / (1.0f + totalDistance); // Lower distance is better, higher fitness
    }

    // Calculate total distance for a given set of tasks
    private float CalculateTotalDistance(List<int> tasks)
    {
        float totalDistance = 0;
        if (tasks.Count > 1)
        {
            for (int i = 0; i < tasks.Count - 1; i++)
            {
                totalDistance += distanceMatrix[tasks[i], tasks[i + 1]];
            }
        }
        return totalDistance;
    }

    // Perform crossover between two parents to create a child
    public static Chromosome Crossover(Chromosome parent1, Chromosome parent2, System.Random random)
    {
        Chromosome child = new Chromosome(parent1.TasksPerAMR.Length, parent1.TasksPerAMR.Length, random, parent1.distanceMatrix);

        // Crossover by copying half of the tasks from each parent
        for (int i = 0; i < child.TasksPerAMR.Length; i++)
        {
            if (random.NextDouble() < 0.5)
            {
                child.TasksPerAMR[i] = new List<int>(parent1.TasksPerAMR[i]);
            }
            else
            {
                child.TasksPerAMR[i] = new List<int>(parent2.TasksPerAMR[i]);
            }
        }

        return child;
    }

    // Apply mutation to the chromosome
    public void Mutate(float mutationRate, System.Random random)
    {
        if (random.NextDouble() < mutationRate)
        {
            // Swap two tasks between two AMRs
            int amrIndex1 = random.Next(TasksPerAMR.Length);
            int amrIndex2 = random.Next(TasksPerAMR.Length);
            if (TasksPerAMR[amrIndex1].Count > 0 && TasksPerAMR[amrIndex2].Count > 0)
            {
                int taskIndex1 = random.Next(TasksPerAMR[amrIndex1].Count);
                int taskIndex2 = random.Next(TasksPerAMR[amrIndex2].Count);

                int temp = TasksPerAMR[amrIndex1][taskIndex1];
                TasksPerAMR[amrIndex1][taskIndex1] = TasksPerAMR[amrIndex2][taskIndex2];
                TasksPerAMR[amrIndex2][taskIndex2] = temp;
            }
        }
    }

    // Override ToString for better logging
    public override string ToString()
    {
        string result = "";
        for (int i = 0; i < TasksPerAMR.Length; i++)
        {
            result += "AMR " + i + ": ";
            foreach (int task in TasksPerAMR[i])
            {
                result += task + " ";
            }
            result += "\n";
        }
        return result;
    }
}
