using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransitionAnimation : MonoBehaviour
{
    public Animator transition;

    public float transitionTime = 1f;

    public void SceneTransition()
    {
        StartCoroutine(SceneTransitionCoroutine());
    }

    public IEnumerator SceneTransitionCoroutine()
    {
        transition.SetTrigger("Start");

        yield return new WaitForSeconds(transitionTime);
    }
}
