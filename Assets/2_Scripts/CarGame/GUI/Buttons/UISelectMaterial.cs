using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UISelectMaterial : MonoBehaviour {
	[System.Serializable]
	public struct LightMat {
		public Material matOn;
		public Material matOff;
		public CarColor color;

	}

	public LightMat[] lightMats;
	public Renderer light1;
	public Renderer light2;
	#region private members
	#endregion private members
	 
	#region Mono
	void OnEnable () {
		Messenger.AddListener <Vehicle> (EventManager.GUI.UPDATE_VEHICLE.ToString (), HandleUpdateVehicle);
	}

	public LightMat GetMatByColor (CarColor _color) {
		
		for (int i = 0; i < lightMats.Length; i++) {
			if (lightMats [i].color == _color)
				return lightMats [i];
		}
		return lightMats[0];
	}

	void Start () {
	}

	void OnDisable () {
		Messenger.RemoveListener <Vehicle> (EventManager.GUI.UPDATE_VEHICLE.ToString (), HandleUpdateVehicle);
	}
	#endregion Mono

	#region public functions
	#endregion public functions

	#region private functions

	void HandleUpdateVehicle (Vehicle _vehicle) {
		light1.material = GetMatByColor (_vehicle.carMats[0].color).matOn;
		light2.material = GetMatByColor (_vehicle.carMats[1].color).matOn;

	}

	#endregion private functions
}
