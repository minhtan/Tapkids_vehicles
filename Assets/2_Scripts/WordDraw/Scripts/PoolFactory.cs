using UnityEngine;
using System.Collections;

public class PoolFactory
{

	public static GameObject Instantiate (GameObject prefab)
	{
		return Instantiate (prefab) as GameObject;
	}
}
