using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Junkfish.Saving;

namespace AOTE_HORDE.Environment
{
	[System.Serializable]
	public class SectionSideComponent
	{
		public SectionSideType sideDirection;
		public Side sideObject;
	}

	public class Section : GameBehaviour
	{
		public BoxCollider Bound;
		public Transform MatriachSpawnPoint;

		List<Room> rooms = new List<Room> ();

		[GameSave, SerializeField]
		private bool isUnlock;

		public bool IsUnlock {
			get { return isUnlock; }
		}

		public static List<Section> sections = new List<Section> ();

		public List<SectionSideComponent> sectionSides = new List<SectionSideComponent> ();

		private List<SpawnPoint> spawnPoints;
		private List<TileFogOfWar> sectionTiles = new List<TileFogOfWar> ();


		void Awake ()
		{
			this.gameObject.GetComponentsInChildren<Room> (rooms);
			InitialisationManager.Instance.onEnviroInitialise += OnEnviroinitialize;
//			rooms.Add (gameObject.GetComponentInChildren<Room> ());

		}

		void OnEnviroinitialize ()
		{
			for (int i = 0; i < rooms.Count; i++)
				sectionTiles.AddRange (rooms [i].RoomTiles);
		}

		public override void OnPostLoad ()
		{
			base.OnPostLoad ();
			if (IsUnlock) {
				Unlock ();
			} else {
				Lock ();
			}
		}


		public void GetSpawnPoints (ref List<SpawnPoint> _spawnPoints)
		{
			for (int i = 0; i < sectionSides.Count; i++) {
				sectionSides [i].sideObject.GetSpawnPoints (this, ref _spawnPoints);
			}
		}

		#region Lock & Unlock Section

		public void Unlock ()
		{
			AddBound ();
			isUnlock = true;
			sections.Add (this);
//			RevealFog ();
			FogOfWarManager.Instance.Recalculate ();
			//	FogOfWarManager.Instance.RevealTiles (sectionTiles);
		}

		public void AddBound ()
		{
			CameraBoundsLimiter.Instance.AddBoundToList (Bound);
		}

		public void Lock ()
		{
			isUnlock = false;
			for (int i = 0; i < sectionSides.Count; i++) {
				sectionSides [i].sideObject.LockDoors ();
			}
		}

		#endregion

		#region Fog Reveal Area

		public void RevealFog ()
		{
			for (int i = 0; i < sectionTiles.Count; ++i) {
				if (sectionTiles [i] != null)
					sectionTiles [i].FOWState = TileVisibilityState.Visible;
			}
			//FogOfWarManager.Instance.Recalculate ();
		}


		#endregion
	
	}
}