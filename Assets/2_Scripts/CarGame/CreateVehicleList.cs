using UnityEngine;
using System.Collections;
using System.IO;

public class CreateVehicleList : MonoBehaviour {

	private string[] vehicles = new string[] {"Airplane", "Bus", "Car", "Delivery Truck", "Electric Bike", "Fire Truck", "Garbage Truck", "Helicopter", 
		"Ice-cream Truck", "Jet Ski", "Kayak", "Limousine", "Motorcycle", "Navy Submarine", "Outrigger Canoe", "Police Car", "Quadbike", "Rickshaw", 
		"Space Shuttle", "Train", "Ultralight Craft", "Van", "Windjammer", "Excavator", "Yacht", "Zeppelin"}; 

	private string json;

	void Start () {
		json = JsonUtility.ToJson (vehicles);
		Debug.Log (json);
	}

	void Update () {
	}

	#region JSON functions

	#endregion JSON functions
}
