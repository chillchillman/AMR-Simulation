using System.Collections.Generic;
using System;
using UnityEngine;

public class DNA {
    private List<int> genes;
    private int amrCount; // AMR 數量
    private int maxStations; // 站點數量
    public float Fitness { get; private set; }

    public DNA(int amrCount, int maxStations) {
        this.amrCount = amrCount;
        this.maxStations = maxStations;
        genes = new List<int>();
        SetRandom();
    }

    public DNA(DNA other) {
        this.amrCount = other.amrCount;
        this.maxStations = other.maxStations;
        this.genes = new List<int>(other.genes);
    }

    public void SetRandom() {
        genes.Clear();
        for (int i = 0; i < maxStations; i++) {
            genes.Add(UnityEngine.Random.Range(0, amrCount));
        }
    }

    public void Mutate() {
        int stationIndex = UnityEngine.Random.Range(0, genes.Count);
        genes[stationIndex] = UnityEngine.Random.Range(0, amrCount);
    }

    public void Combine(DNA parent1, DNA parent2) {
        for (int i = 0; i < genes.Count; i++) {
            genes[i] = (i < genes.Count / 2) ? parent1.genes[i] : parent2.genes[i];
        }
    }

    public void EvaluateFitness(float[,] distanceMatrix) {
        float totalDistance = 0;

        for (int currentAMR = 0; currentAMR < amrCount; currentAMR++) {
            float amrDistance = 0;
            List<int> assignedStations = new List<int>();

            for (int i = 0; i < maxStations; i++) {
                if (genes[i] == currentAMR) {
                    assignedStations.Add(i);
                }
            }

            for (int i = 0; i < assignedStations.Count - 1; i++) {
                amrDistance += distanceMatrix[assignedStations[i], assignedStations[i + 1]];
            }

            totalDistance += amrDistance;
        }

        Fitness = -totalDistance; // 距離越小，適應度越高
    }

    // 新增 GetGenes 方法
    public List<int> GetGenes() {
        return new List<int>(genes); // 返回基因的副本以防止外部修改
    }

    public override string ToString() {
        return string.Join(", ", genes);
    }
}
