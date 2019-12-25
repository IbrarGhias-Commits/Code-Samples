using System.Collections;
using System.Collections.Generic;
using Junkfish;
using UnityEngine;

namespace AOTE_HORDE.Environment
{
	public enum SectionSideType
	{
		none,
		left,
		right,
		top,
		bottom
	}

	public class EnvironmentManager : GameBehaviour
	{
		private static EnvironmentManager instance;
		public static EnvironmentManager Instance
		{
			get
			{
				if ( instance == null )
					instance = GameObject.FindObjectOfType<EnvironmentManager>();
				return instance;
			}
		}
		public List<Section> Sections = new List<Section>();
		private List<SpawnPoint> spawnPoints = new List<SpawnPoint>();

		public override void OnPostLoad()
		{
			base.OnPostLoad();
			GlobalEventsManager.Instance.onSideUnlocked.AddListener(DecideUnlockSection);
		}

		private void OnDestroy()
		{
			GlobalEventsManager.Instance.onSideUnlocked.RemoveListener(DecideUnlockSection);
		}

		public List<SpawnPoint> GetSpawnPoints()
		{
			spawnPoints = new List<SpawnPoint>();
			for ( int i = 0; i < Sections.Count; i++ )
			{
				if ( Sections[i].IsUnlock )
				{
					Sections[i].GetSpawnPoints(ref spawnPoints);
				}
			}
			return spawnPoints;
		}

		public void InitSections()
		{
			if ( Sections.Count <= 0 )
			{
				Debug.LogError("Can't init Sections because there isn'y any XD", this);
				return;
			}
			for ( int i = 0; i < Sections.Count; i++ )
			{
				Sections[i].Lock();
			}
			Sections[4].Unlock();
		}

		public Transform GetMatriachSpawnPoint( int index = 4 )
		{
			return Sections[index].MatriachSpawnPoint ?? this.transform;
		}

		public void DecideUnlockSection( Side _side )
		{
			Debug.Log("Unlocking Section " + _side.name, this);
			_side.UnlockSections();
		}
	}
}
