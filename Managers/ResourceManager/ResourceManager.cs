using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace AOTE_HORDE
{
	public static class ResourceWrapper
	{
		private static Dictionary<string, object> CachedObject;

		static ResourceWrapper()
		{
			SceneManager.sceneLoaded += Clear;
		}

		public static T Load<T>( string path )
		{
			object _temp;
			if ( CachedObject.TryGetValue(path, out _temp) )
			{
				return (T)_temp;
			}
			else
			{
				// Load the Asset and Add it in Dictionary
				_temp = Resources.Load(path);
				if ( (T)_temp != null )
				{
					CachedObject.Add(path, _temp);
				}
				return (T)_temp;
			}
		}

		private static void Clear( Scene scene, LoadSceneMode mode )
		{
			CachedObject.Clear();
			Resources.UnloadUnusedAssets();
		}
	}
}