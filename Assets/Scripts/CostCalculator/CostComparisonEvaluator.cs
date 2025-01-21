using UnityEngine;

public class CostComparisonEvaluator
{
    private ICostCalculator humanCostCalculator;
    private ICostCalculator amrCostCalculator;
    private float targetEfficiencyGain;

    public float HumanCost { get; private set; }
    public float AmrCost { get; private set; }
    public float AnnualSavings { get; private set; }
    public float BreakEvenYears { get; private set; }

    public CostComparisonEvaluator(ICostCalculator humanCost, ICostCalculator amrCost, float targetEfficiencyGain)
    {
        this.humanCostCalculator = humanCost;
        this.amrCostCalculator = amrCost;
        this.targetEfficiencyGain = Mathf.Clamp(targetEfficiencyGain, 0, 100) / 100f;

        Evaluate(); // 在構造函數中執行計算
    }

    private void Evaluate()
    {
        HumanCost = humanCostCalculator.CalculateCost(); // 計算人力成本
        AmrCost = amrCostCalculator.CalculateCost();     // 計算AMR成本
        AnnualSavings = HumanCost * targetEfficiencyGain;

        if (AnnualSavings > 0)
        {
            BreakEvenYears = AmrCost / AnnualSavings; // 計算達到回本所需的年份
        }
        else
        {
            BreakEvenYears = float.PositiveInfinity; // 如果無節省，設定為無窮大
        }
    }
}
