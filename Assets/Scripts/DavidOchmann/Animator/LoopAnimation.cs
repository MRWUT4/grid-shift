using UnityEngine;
using System.Collections;

public class LoopAnimation : StateMachineBehaviour 
{
	public int loop = 0;
	public string exitState = "";

	override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) 
	{
		if( loop <= 0 )
			animator.Play( exitState );
		else
			loop--;
	}
}
