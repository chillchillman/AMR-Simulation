using System;

public class Brain {
    public DNA dna; // 基因組
    public float Fitness { get; private set; } // 適應度

    public Brain(int amrCount, int maxStations) {
        dna = new DNA(amrCount, maxStations); // 初始化基因
    }

    public void EvaluateFitness(float[,] distanceMatrix) {
    dna.EvaluateFitness(distanceMatrix); // 使用 DNA 的方法計算適應度
    Fitness = dna.Fitness; // 將適應度同步到 Brain
    }


    public override string ToString() {
        return $"Fitness: {Fitness}, DNA: {dna}";
    }
}
