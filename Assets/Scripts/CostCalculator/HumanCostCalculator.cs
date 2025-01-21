using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HumanCostCalculator : ICostCalculator
{
    public int employeeCount;
    public float annualSarary;
    public float efficiency; // 百分比(0-100)

    public HumanCostCalculator(int employeeCount, float annualSarary, float efficiency)
    {
        this.employeeCount = employeeCount;
        this.annualSarary = annualSarary;
        this.efficiency = Mathf.Clamp(efficiency, 0, 100) / 100f;
    }

    public float CalculateCost()
    {
        float totalCost = employeeCount * annualSarary * efficiency; // 0.01f to convert percentage to decimal
        Debug.Log($"Human Cost Calculated: {totalCost}");
        return totalCost;
    }

}
