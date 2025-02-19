﻿using System;
using System.Collections.Generic;
using MEC;
using UnityEngine;
using Random = UnityEngine.Random;
public static class Utils
{
    /// <summary>
    /// Sometimes, when you use Unity's built-in OnValidate, it will spam you with a very annoying warning message,
    /// even though nothing has gone wrong. To avoid this, you can run your OnValidate code through this utility.
    /// </summary>
    public static class ValidationUtility
    {
        /// <summary>
        /// Call this during OnValidate.
        /// Runs <paramref name="onValidateAction"/> once, after all inspectors have been updated.
        /// </summary>
        public static void SafeOnValidate(Action onValidateAction)
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.delayCall += _OnValidate;


            void _OnValidate()
            {
                UnityEditor.EditorApplication.delayCall -= _OnValidate;

                onValidateAction();
            }
#endif
        }

        // For more details, see this forum thread: https://forum.unity.com/threads/sendmessage-cannot-be-called-during-awake-checkconsistency-or-onvalidate-can-we-suppress.537265/

    }

    public static IEnumerator<float> EmulateUpdate(System.Action func, MonoBehaviour scr)
    {
        yield return Timing.WaitForOneFrame;
        while (scr.gameObject != null)
        {
            if (scr.gameObject.activeInHierarchy && scr.enabled)
            {
                func();

                yield return Timing.WaitForOneFrame;
            }
        }
    }

    public static T GetRandom<T>(this List<T> list)
    {
        int index = Random.Range(0, list.Count - 1);
        return list[index];
    }
}
