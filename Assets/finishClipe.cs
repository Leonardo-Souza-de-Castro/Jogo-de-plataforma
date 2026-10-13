using UnityEngine;

public class finishClipe : StateMachineBehaviour
{
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (stateInfo.normalizedTime >= 0.95f)
            animator.speed = 0f; // congela no último frame
    }
}
