using UnityEngine;
using System.Collections;
using System;
using System.Linq;

public class Test : StateMachineBehaviour {


	public bool active;
	public override void OnStateEnter (Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
		Debug.Log ("Start " + layerIndex);
	}

	public override void OnStateExit (Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
		
	}

}
