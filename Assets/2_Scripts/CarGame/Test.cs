using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Test : MonoBehaviour {

	IEnumerable<int> UniqueRandom(int minInclusive, int maxInclusive)
	{
		List<int> candidates = new List<int>();
		for (int i = minInclusive; i <= maxInclusive; i++)
		{
			candidates.Add(i);
		}
		System.Random rnd = new System.Random();
		while (candidates.Count > 0)
		{
			int index = rnd.Next(candidates.Count);
			yield return candidates[index];
			candidates.RemoveAt(index);
		}
	}

	void Start () {
		foreach (int i in UniqueRandom(0, 100)) {
			Debug.Log (i);
		}
	}
}
