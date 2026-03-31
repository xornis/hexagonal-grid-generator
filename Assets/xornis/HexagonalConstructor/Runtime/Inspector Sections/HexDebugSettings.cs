using System;
using System.Collections;
using UnityEngine;

namespace HexDungeon
{
    public class HexDebugSettings : MonoBehaviour
    {
        [SerializeField] private bool debugMode = false;
        [SerializeField, Tooltip("Works only in Play Mode")] private float stepDelay = 0.1f;

        public bool IsDebugMode => debugMode;
        public float StepDelay => stepDelay;

        public void EditorGenerateInternal(Action generateFunc, IEnumerator debugGenerateCoroutine)
        {
            EditorClearInternal();
            StopAllCoroutines();
            if (Application.isPlaying) StartCoroutine(debugGenerateCoroutine);
            else generateFunc();
        }

        public void EditorClearInternal()
        {
            StopAllCoroutines();

            for (int i = transform.childCount - 1; i >= 0; i--)
                DestroyImmediate(transform.GetChild(i).gameObject);
        }
    }
}
