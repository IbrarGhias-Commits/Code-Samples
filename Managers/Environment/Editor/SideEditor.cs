using UnityEngine;
using System.Collections;
using UnityEditor;
using System.Linq;

namespace AOTE_HORDE.Environment
{
	[CustomEditor(typeof(Side))]
	public class SideEditor : Editor
	{
		// Left Side is A
		// Right Side is B
		// A -> B

		const string spawnName = "SpawnIcons";
		const string doorName = "Doors";
		Side myTarget;

		public override void OnInspectorGUI()
		{
			DrawDefaultInspector();

			myTarget = (Side)target;
			try
			{
				myTarget.SpawnIconsLeft = myTarget.transform.Find(spawnName + "_A").gameObject;
				myTarget.SpawnIconsRight = myTarget.transform.Find(spawnName + "_B").gameObject;
			}
			catch
			{
				// Just To Ignore gameobject Error
			}

			if ( GUILayout.Button("Add Spawn Icon") )
			{
				if ( myTarget.spwanIconGameobject == null )
					return;

				if ( myTarget.SpawnIconsLeft != null )
					GameObject.DestroyImmediate(myTarget.SpawnIconsLeft);
				if ( myTarget.SpawnIconsRight != null )
					GameObject.DestroyImmediate(myTarget.SpawnIconsRight);

				var leftParent = CreateNewObject(myTarget.SpawnIconsLeft, spawnName + "_A", myTarget.transform);
				var rightParent = CreateNewObject(myTarget.SpawnIconsRight, spawnName + "_B", myTarget.transform);

				for ( int i = 0; i < myTarget.rightSP.Count; i++ )
				{
					var _obj = PrefabUtility.InstantiatePrefab(myTarget.spwanIconGameobject as GameObject) as GameObject;
					CopyTransform(myTarget.rightSP[i].transform, _obj.transform, rightParent.transform);
				}

				for ( int i = 0; i < myTarget.leftSP.Count; i++ )
				{
					var _obj = PrefabUtility.InstantiatePrefab(myTarget.spwanIconGameobject as GameObject) as GameObject;
					CopyTransform(myTarget.leftSP[i].transform, _obj.transform, leftParent.transform);
				}
			}

			if ( GUILayout.Button("Assign Door") )
			{
				var _doorParent = myTarget.transform.Find(doorName);
				if ( _doorParent != null )
				{
					myTarget.Doors = _doorParent.GetComponentsInChildren<Door>().ToList();
				}
			}

			if ( GUILayout.Button("Create Unlock Door Prefab") )
			{
				for ( int i = 0; i < myTarget.Doors.Count; i++ )
				{
					DoorEditor.Instance.SetUpUnlockDoorPrefabs(myTarget.Doors[i]);
				}
			}
		}
		void ResetTransform( Transform _transform, Transform _parent = null )
		{
			if ( _parent != null )
			{
				_transform.SetParent(_parent);
			}
			_transform.localPosition = Vector3.zero;
			_transform.localRotation = Quaternion.identity;
		}

		void CopyTransform( Transform copyFrom, Transform copyTo, Transform _parent = null )
		{
			if ( _parent != null )
			{
				copyTo.SetParent(_parent);
			}
			copyTo.position = copyFrom.position;
			copyTo.rotation = copyFrom.rotation;
		}

		GameObject CreateNewObject( GameObject _object, string _spawnName, Transform _parent )
		{
			if ( _object == null )
			{
				var parent = myTarget.transform.Find(_spawnName);
				if ( parent == null )
				{
					_object = new GameObject();
					_object.name = _spawnName;
					ResetTransform(_object.transform, _parent);
				}
				else
				{
					_object = parent.gameObject;
				}
			}
			return _object;
		}
	}
}