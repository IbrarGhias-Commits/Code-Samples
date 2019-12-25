using UnityEngine;
using System.Collections;
using UnityEditor;
using System.Linq;

namespace AOTE_HORDE.Environment
{
	[CustomEditor(typeof(Section))]
	public class SectionEditor : Editor
	{
		const string spawnName = "MatriachSpawnPoint";

		public override void OnInspectorGUI()
		{
			DrawDefaultInspector();
			Section myTarget = (Section)target;
			if ( myTarget.MatriachSpawnPoint == null )
			{
				myTarget.MatriachSpawnPoint = myTarget.transform.Find(spawnName);
			}
		}
	}
}