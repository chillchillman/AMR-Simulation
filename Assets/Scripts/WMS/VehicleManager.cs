using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vehicle
{
    public string VehicleID { get; private set; }
    public string Status { get; set; }

    public Vehicle(string id)
    {
        VehicleID = id;
        Status = "Idle";
    }
}

public class VehicleManager
{
    private List<Vehicle> _vehicles = new List<Vehicle>();

    public void AddVehicle(string vehicleID)
    {
        _vehicles.Add(new Vehicle(vehicleID));
    }

    public Vehicle GetAvailableVehicle()
    {
        return _vehicles.Find(v => v.Status == "Idle");
    }

    public void DispatchVehicle(string vehicleID, Vector3 destination)
    {
        Vehicle vehicle = _vehicles.Find(v => v.VehicleID == vehicleID);
        if (vehicle != null)
        {
            vehicle.Status = "In Transit";
            // 通过NavMesh等系统实现车辆移动
        }
    }
}

