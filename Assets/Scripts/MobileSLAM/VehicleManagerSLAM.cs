using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.IO;
using System;
using TMPro;

// Vehicle Manager to handle UI and vehicle status
public class VehicleManagerSLAM : MonoBehaviour
{
    public List<VehicleData> vehicles = new List<VehicleData>();
    public TMP_Text vehicleStatusText;

    private IVehicleUI vehicleUI;
    private int currentVehicleIndex = 0;

    void Start()
    {
        LoadVehicleDataFromJSON();
        vehicleUI = new VehicleUI(vehicleStatusText);

        UpdateVehicleUI();
    }

    void LoadVehicleDataFromJSON()
    {
        string filePath = Path.Combine(Application.streamingAssetsPath, "vehicleData.json");
        if (File.Exists(filePath))
        {
            string jsonData = File.ReadAllText(filePath);
            VehicleData[] loadedVehicles = JsonHelper.FromJson<VehicleData>(jsonData);
            vehicles = new List<VehicleData>(loadedVehicles);
        }
        else
        {
            Debug.LogError("Vehicle data file not found at: " + filePath);
        }
    }

    public void SwitchVehicle(int vehicleIndex)
    {
        currentVehicleIndex = vehicleIndex;
        UpdateVehicleUI();
    }

    void UpdateVehicleUI()
    {
        if (vehicles.Count > currentVehicleIndex)
        {
            vehicleUI.DisplayVehicleInfo(vehicles[currentVehicleIndex]);
        }
    }
}

// Vehicle Data class
[System.Serializable]
public class VehicleData
{
    public string vehicleName;
    public float speed;
    public float batteryLevel;
    public string status;

    public VehicleData(string vehicleName, float speed, float batteryLevel, string status)
    {
        this.vehicleName = vehicleName;
        this.speed = speed;
        this.batteryLevel = batteryLevel;
        this.status = status;
    }
}

// Helper class for JSON serialization/deserialization
public static class JsonHelper
{
    public static T[] FromJson<T>(string json)
    {
        Wrapper<T> wrapper = JsonUtility.FromJson<Wrapper<T>>(json);
        return wrapper.Items;
    }

    [System.Serializable]
    private class Wrapper<T>
    {
        public T[] Items;
    }
}

// Interface for Vehicle UI
public interface IVehicleUI
{
    void DisplayVehicleInfo(VehicleData vehicleData);
}

// Concrete implementation of Vehicle UI
public class VehicleUI : IVehicleUI
{
    private TMP_Text vehicleStatusText;

    public VehicleUI(TMP_Text vehicleStatusText)
    {
        this.vehicleStatusText = vehicleStatusText;
    }

    public void DisplayVehicleInfo(VehicleData vehicleData)
    {
        vehicleStatusText.text = $"Vehicle: {vehicleData.vehicleName}\n" +
                                 $"Status: {vehicleData.status}\n" +
                                 $"Speed: {vehicleData.speed} m/s\n" +
                                 $"Battery Level: {vehicleData.batteryLevel}%";
    }
}
