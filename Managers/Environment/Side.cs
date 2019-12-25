using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Junkfish;

namespace AOTE_HORDE.Environment
{
	public class Side : GameBehaviour
	{
		[SerializeField]
		private Section SectionA;
		[SerializeField]
		private Section SectionB;

		// Left Side is A
		// Right Side is B
		// A -> B
		public List<SpawnPoint> rightSP, leftSP = null;

		public List<Door> Doors;
		List<GridObject> gridA = new List<GridObject> ();
		List<GridObject> gridB = new List<GridObject> ();

		public bool IsUnlocked {
			get {
				return (SectionA != null && SectionA.IsUnlock) && (SectionB != null && SectionB.IsUnlock);
			}
		}

		void Awake ()
		{
			//if a side has no door dont proceed
			if (Doors.Count <= 0)
				return;
			//getting all the Side gridobjects of each side 
			for (int i = 0; i < Doors.Count; i++) {
				gridA.AddRange (Doors [i].sideA);
				gridB.AddRange (Doors [i].sideB);
			}
			//adding spawn point to each gridobject
			for (int i = 0; i < gridA.Count; i++) {
				gridA [i].gameObject.AddComponent<SpawnPoint> ();
				gridB [i].gameObject.AddComponent<SpawnPoint> ();
			}
			//adding spawn points to the respective list
			for (int i = 0; i < gridA.Count; i++) {
				leftSP.Add (gridA [i].GetComponent<SpawnPoint> ());
				rightSP.Add (gridB [i].GetComponent<SpawnPoint> ());
			}
		}



		[Space]
		public GameObject spwanIconGameobject;
		[Space (2)]
		public GameObject SpawnIconsLeft;
		public GameObject SpawnIconsRight;

		public List<SpawnPoint> GetSpawnPoints (SectionSideType side)
		{
			if (IsUnlocked)
				return null;
			else if (side.Equals (SectionSideType.left)) {

				return rightSP;
			} else if (side.Equals (SectionSideType.right)) {

				return leftSP;
			} else if (side.Equals (SectionSideType.top)) {

				return rightSP;
			} else if (side.Equals (SectionSideType.bottom)) {

				return leftSP;
			} else
				return null;

		}

		public override void OnPostLoad ()
		{
			base.OnPostLoad ();
			if (IsUnlocked) {
				UnlockDoors ();
			} else {
				for (int i = 0; i < Doors.Count; i++) {
					Doors [i].onUnlocked.AddListener (SideUnlocked);
				}

			}
		}

		private void OnDestory ()
		{
			base.OnDisable ();
			for (int i = 0; i < Doors.Count && !IsUnlocked; i++) {
				Doors [i].onUnlocked.RemoveListener (SideUnlocked);
			}
		}

		public void GetSpawnPoints (Section _section, ref List<SpawnPoint> _spawnPoints)
		{
			if (_section == null || IsUnlocked)
				return;

			if (_section.Equals (SectionA) && SectionA.IsUnlock) {
				SpawnIconsLeft.gameObject.SetActive (true);
				_spawnPoints.AddRange (leftSP);
			} else {
				SpawnIconsRight.gameObject.SetActive (true);
				_spawnPoints.AddRange (rightSP);
			}
		}

		public void LockDoors ()
		{
			for (int i = 0; i < Doors.Count; i++) {
				Doors [i].Lock ();
			}

			SpawnIconsLeft.gameObject.SetActive (false);
			SpawnIconsRight.gameObject.SetActive (false);
		}

		public void UnlockDoors ()
		{
			SpawnIconsLeft.gameObject.SetActive (false);
			SpawnIconsRight.gameObject.SetActive (false);

			for (int i = 0; i < Doors.Count; i++) {
				Doors [i].onUnlocked.RemoveListener (SideUnlocked);
				Doors [i].Unlock ();
			}
		}

		void SideUnlocked ()
		{
			//for ( int i = 0; i < Doors.Count; i++ )
			//{
			//	Doors[i].onUnlocked.RemoveListener(SideUnlocked);
			//}
			if (GlobalEventsManager.Instance.onSideUnlocked != null)
				GlobalEventsManager.Instance.onSideUnlocked.Invoke (this);
		}

		#region Assign Sections

		public void AssignSectionA (Section _SectionA)
		{
			SectionA = _SectionA;
		}

		public void AssignSectionB (Section _SectionB)
		{
			SectionB = _SectionB;
		}

		public void UnlockSections ()
		{
			SectionA.Unlock ();
			SectionB.Unlock ();
			// Open Doors
			UnlockDoors ();
		}

		#endregion
	}
}
