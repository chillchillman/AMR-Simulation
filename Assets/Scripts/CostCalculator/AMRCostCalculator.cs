using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AMRCostCalculator : ICostCalculator
{
   public int vehicleCount;
   public float investmentPerVehicle;
   public float efficiencyIncrease; //百分比(0-100)

    public AMRCostCalculator(int vehicleCount, float investmentPerVehicle, float efficiencyIncrease)
    {
        this.vehicleCount = vehicleCount;
        this.investmentPerVehicle = investmentPerVehicle;
        this.efficiencyIncrease = Mathf.Clamp(efficiencyIncrease, 0, 100) / 100f;
    }

    public float CalculateCost()
    {
        float totalCost = vehicleCount * investmentPerVehicle * efficiencyIncrease;
        Debug.Log($"AMR Cost Calculated: {totalCost}");
        return totalCost;
    }
}
