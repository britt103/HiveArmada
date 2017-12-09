﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AllyPathing : MonoBehaviour {

	// Use this for initialization
	void Start ()
    {
        iTween.MoveTo(gameObject, iTween.Hash("path", iTweenPath.GetPath("Ally Path"), "speed", 5f, "islocal", true,
                                                "easetype", iTween.EaseType.linear, "looptype", iTween.LoopType.loop,
                                                "oncomplete", "PauseTween", "oncompletetarget", gameObject, "orienttopath", true));
	}

    /// <summary>
    /// Calls PauseTween() 
    /// </summary>
    private void PauseTween()
    {
        StartCoroutine(PauseTween(0.1f));
    }

    /// <summary>
    /// Pauses movement for 'waitTime' seconds.
    /// </summary>
    /// <param name="waitTime"> How many seconds to pause for.</param>
    private IEnumerator PauseTween(float waitTime)
    {
        iTween.Pause(gameObject);
        yield return new WaitForSeconds(waitTime);

        iTween.Resume(gameObject);
    }
}
