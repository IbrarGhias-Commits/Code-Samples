using System.Collections;
using System.Collections.Generic;
using AOTE_HORDE.Waves;
using AOTE_HORDE.Environment;
using UnityEngine;
using Junkfish;

namespace AOTE_HORDE
{
	public class HordeManager : GameBehaviour
	{
		private static HordeManager instance;

		public static HordeManager Instance
		{
			get
			{
				if ( instance == null )
					instance = GameObject.FindObjectOfType<HordeManager>();
				return instance;
			}
		}

		[SerializeField]
		private Entity _playerEntity;

		public Entity PlayerEntity
		{
			get
			{
				return _playerEntity;
			}
		}

		public AOTE_HORDE.Environment.EnvironmentManager _environmentManager;
		public WaveManager _waveManager;

		public override void OnPostLoadFirst()
		{
			base.OnPostLoadFirst();
			Debug.Log("Chussar Horde Manager", this);
			Init();
		}

		void Init()
		{
			Debug.Log("Horde Manager", this);
			if ( _environmentManager != null )
			{
				_environmentManager.InitSections();
				var matriachPosition = _environmentManager.GetMatriachSpawnPoint();
				UpdateMainPlayerPosition(matriachPosition);
				// Set Matriach As Starting Player
				InitialisationManager.Instance.onGameStartEvery += SelectPlayer;
			}
			if ( _waveManager != null )
				_waveManager.Initiate(this);
		}

		private void SelectPlayer()
		{
			FindInteractableObjects.Instance.Select(_playerEntity.gameObject);
			CycleUnitsCam.Instance.PanCameraToPointTime(_playerEntity.transform.position, -1);
		}

		private void OnDestroy()
		{
			base.OnDisable();
			InitialisationManager.Instance.onGameStartEvery -= SelectPlayer;
		}

		public List<SpawnPoint> GetSpawnPoints()
		{
			return _environmentManager.GetSpawnPoints();
		}

		void UpdateMainPlayerPosition( Transform refTransform )
		{
			_playerEntity.GridObject.SetPosition(refTransform);
			EntityVision.UpdateAllVision(o => true);
			//FogOfWarManager.Instance.Recalculate(true);
		}
	}
}