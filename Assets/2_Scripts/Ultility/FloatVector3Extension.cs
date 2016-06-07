using UnityEngine;
using System.Collections;

public static class FloatVector3Extension {

	public static Vector3 ToVector3(this float fl){
		return new Vector3 (fl, fl, fl);
	}

	public static float ToFloat(this Vector3 vc3){
		return vc3.x;
	}
}
