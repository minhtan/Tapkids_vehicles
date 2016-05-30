using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class SelectMaterialController : MonoBehaviour {
	[System.Serializable]
	public struct LightMat {
		public Material matOn;
		public Material matOff;
		public CarColor color;

	}

	public LightMat[] lightMats;
	public Renderer[] lightRenderers;

	public bool[] lights;
	#region private members
	#endregion private members
	 
	#region Mono
	void OnEnable () {
		Messenger.AddListener <Vehicle> (EventManager.GUI.UPDATE_VEHICLE.ToString (), HandleUpdateVehicle);
	}

	void Start () {
		// setup toggle group
		if (lightRenderers.Length > 0) {
			lights = new bool[lightRenderers.Length];
		}

	}

	void OnDisable () {
		Messenger.RemoveListener <Vehicle> (EventManager.GUI.UPDATE_VEHICLE.ToString (), HandleUpdateVehicle);
	}
	#endregion Mono

	#region public functions
	public LightMat GetMatByColor (CarColor _color) {
		for (int i = 0; i < lightMats.Length; i++) {
			if (lightMats [i].color == _color)
				return lightMats [i];
		}
		return lightMats[0];
	}
	#endregion public functions

	#region private functions
	void HandleUpdateVehicle (Vehicle _vehicle) {
		for (int i = 0; i < lights.Length; i++) {
			if (_vehicle.matId == i) {
				lightRenderers [i].material = GetMatByColor (_vehicle.carMats[i].color).matOff;
				lights[i] = true;
			} else {
				lightRenderers [i].material = GetMatByColor (_vehicle.carMats[i].color).matOn;
				lights[i] = false;
			}
		}
	}
	#endregion private functions
}
