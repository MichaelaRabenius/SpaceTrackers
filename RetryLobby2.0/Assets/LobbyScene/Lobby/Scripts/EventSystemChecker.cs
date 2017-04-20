using System;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Prototype.NetworkLobby
{
    public class EventSystemChecker : MonoBehaviour
    {
        //public GameObject eventSystem;

        // Use this for initialization
        void Awake()
        {
			Debug.Log ("Nu: ESC Awake");

            if (!FindObjectOfType<EventSystem>())
			{
                //Instantiate(eventSystem);
                GameObject obj = new GameObject("EventSystem");
                obj.AddComponent<EventSystem>();
                obj.AddComponent<StandaloneInputModule>().forceModuleActive = true;

				Debug.Log ("Nu: ESC add");
            }
        }
    }
}