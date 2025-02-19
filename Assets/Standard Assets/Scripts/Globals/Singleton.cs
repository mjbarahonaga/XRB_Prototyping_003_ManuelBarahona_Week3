﻿using System;
using UnityEngine;

public abstract class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
	private static T _instance;

	private static object _lock = new object();

	public static T Instance
	{
		get
		{
			if (applicationIsQuitting)
			{
				return null;
			}

			lock (_lock)
			{
				if (_instance == null)
				{
					_instance = (T)FindObjectOfType(typeof(T));

					if (FindObjectsOfType(typeof(T)).Length > 1)
					{
						Debug.LogError("[Singleton] Something went really wrong " +
							" - there should never be more than 1 singleton!" +
							" Reopening the scene might fix it.");
						return _instance;
					}

					if (_instance == null)
					{
						GameObject singleton = new GameObject();
						_instance = singleton.AddComponent<T>();
						singleton.name = "(singleton) " + typeof(T).ToString();

						DontDestroyOnLoad(singleton);

						//Debug.Log("[Singleton] An instance of " + typeof(T) +
						//	" is needed in the scene, so '" + singleton +
						//	"' was created with DontDestroyOnLoad.");
					}
					else
					{
						//Debug.Log("[Singleton] Using instance already created: " +
						//	_instance.gameObject.name);
					}
				}

				return _instance;
			}
		}
	}

	 private static bool applicationIsQuitting = false;

	protected virtual void Awake() => applicationIsQuitting = false;

	/// <summary>
	/// When Unity quits, it destroys objects in a random order.
	/// In principle, a Singleton is only destroyed when application quits.
	/// If any script calls Instance after it have been destroyed, 
	///   it will create a buggy ghost object that will stay on the Editor scene
	///   even after stopping playing the Application. Really bad!
	/// So, this was made to be sure we're not creating that buggy ghost object.
	/// </summary>
	public void OnApplicationQuit()
	{
		applicationIsQuitting = true;
	}
}
