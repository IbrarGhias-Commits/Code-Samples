using UnityEngine;
using System.Collections;
using UnityEditor;
using System.Linq;

namespace AOTE_HORDE.Environment
{
	[CustomEditor(typeof(EnvironmentManager))]
	public class EnvironmentManagerEditor : Editor
	{
		public override void OnInspectorGUI()
		{
			DrawDefaultInspector();

			EnvironmentManager myTarget = (EnvironmentManager)target;

			if ( myTarget.Sections == null || myTarget.Sections.Count == 0 )
				myTarget.Sections = myTarget.transform.GetComponentsInChildren<Section>().ToList();

			if ( GUILayout.Button("Assign Sections to Respective Sides") )
			{
				for ( int i = 0; i < myTarget.Sections.Count; i++ )
				{
					var _section = myTarget.Sections[i];
					for ( int j = 0; j < _section.sectionSides.Count; j++ )
					{
						var _side = _section.sectionSides[j];
						switch ( _side.sideDirection )
						{
							case SectionSideType.top:
							case SectionSideType.right:
								_side.sideObject.AssignSectionB(_section);
								break;
							case SectionSideType.bottom:
							case SectionSideType.left:
								_side.sideObject.AssignSectionA(_section);
								break;
							case SectionSideType.none:
							default:
								break;
						}
					}
				}
			}
		}

		void CopyTransform( Transform copyFrom, Transform copyTo )
		{
			copyTo.position = copyFrom.position;
			copyTo.rotation = copyFrom.rotation;
		}

	}
}