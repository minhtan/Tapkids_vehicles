using UnityEngine;
using System.Text;
using System.Collections;
using Lean;

public class UIVehicleInfo : LeanLocalizedBehaviour {

	public int size = 18;

	private void OnEnable () {
		
		Messenger.AddListener <Vehicle> (EventManager.GUI.UPDATE_VEHICLE.ToString (), HandleUpdateVehicle);
	}
	private	void OnDisable () {
		Messenger.RemoveListener <Vehicle> (EventManager.GUI.UPDATE_VEHICLE.ToString (), HandleUpdateVehicle);
	}

	void HandleUpdateVehicle (Vehicle _vehicle) {
		UpdateTranslation (LeanLocalization.GetTranslation(_vehicle.name + PhraseName));
	}

	#region implemented abstract members of LeanLocalizedBehaviour
	public override void UpdateTranslation (LeanTranslation translation)
	{
		var text = GetComponent<TextMesh>();
		// Use translation?
		if (translation != null)
			text.text = TextWrap (translation.Text, size);
	}

	// Wrap text by line height
	private string TextWrap (string input, int _size){
		string[] words = input.Split(" "[0]);
		string result = "";
		string line = "";
		foreach(string s in words){
			string temp = line + " " + s;
			if(temp.Length > _size){
				result += line + "\n";
				line = s;
			}
			else {
				line = temp;
			}
		}
		result += line;
		return result.Substring(1,result.Length-1);
	}
	#endregion

}
