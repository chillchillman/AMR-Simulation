using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CostCalculatorUI : MonoBehaviour
{
    [Header("Human Cost Inputs")]
    public TMP_InputField employeeCountInput;
    public TMP_InputField annualSalaryInput;

    [Header("Employee Efficiency Slider")]
    public Slider employeeEfficiencySlider;
    public TMP_Text employeeEfficiencyText;

    [Header("AMR Cost Inputs")]
    public TMP_InputField vehicleCountInput;
    public TMP_InputField investmentPerVehicleInput;

    [Header("AMR Efficiency Slider")]
    public Slider efficiencyIncreaseSlider;
    public TMP_Text efficiencyIncreaseText;

    [Header("Target Efficiency")]
    public TMP_InputField targetEfficiencyGainInput;

    [Header("Result Display")]
    public TMP_Text resultText;

    [Header("Button")]
    public Button calculateButton;

    private void Start()
    {
        employeeEfficiencySlider.onValueChanged.AddListener(OnEmployeeEfficiencyChanged);
        efficiencyIncreaseSlider.onValueChanged.AddListener(OnEfficiencyIncreaseChanged);

        UpdateSliderText(employeeEfficiencySlider, employeeEfficiencyText);
        UpdateSliderText(efficiencyIncreaseSlider, efficiencyIncreaseText);

        if (calculateButton != null)
        {
            calculateButton.onClick.AddListener(OnCalculateButtonClicked);
        }
    }

    private void OnEmployeeEfficiencyChanged(float value)
    {
        UpdateSliderText(employeeEfficiencySlider, employeeEfficiencyText);
    }

    private void OnEfficiencyIncreaseChanged(float value)
    {
        UpdateSliderText(efficiencyIncreaseSlider, efficiencyIncreaseText);
    }

    private void UpdateSliderText(Slider slider, TMP_Text text)
    {
        text.text = $"{slider.value}%";
    }

    private void OnCalculateButtonClicked()
    {
        int employeeCount = int.Parse(employeeCountInput.text);
        float annualSalary = float.Parse(annualSalaryInput.text);
        float employeeEfficiency = employeeEfficiencySlider.value;

        int vehicleCount = int.Parse(vehicleCountInput.text);
        float investmentPerVehicle = float.Parse(investmentPerVehicleInput.text);
        float efficiencyIncrease = efficiencyIncreaseSlider.value;

        float targetEfficiencyGain = float.Parse(targetEfficiencyGainInput.text);

        ICostCalculator humanCostCalculator = new HumanCostCalculator(employeeCount, annualSalary, employeeEfficiency);
        ICostCalculator amrCostCalculator = new AMRCostCalculator(vehicleCount, investmentPerVehicle, efficiencyIncrease);

        CostComparisonEvaluator evaluator = new CostComparisonEvaluator(humanCostCalculator, amrCostCalculator, targetEfficiencyGain);

        string result = EvaluateCosts(evaluator);
        resultText.text = result;
    }

    private string EvaluateCosts(CostComparisonEvaluator evaluator)
    {
        float humanCost = evaluator.HumanCost;
        float amrCost = evaluator.AmrCost;
        float annualSavings = evaluator.AnnualSavings;
        float breakEvenYears = evaluator.BreakEvenYears;

        return $"Human Cost: \n$NT {humanCost} \n\n"+
               $"AMR Cost: \n$NT {amrCost}\n\n" +
               $"Annual Savings: \n$NT {annualSavings}\n\n" +
               $"Break-Even Years: \n{Mathf.Ceil(breakEvenYears)} years";
    }
}
