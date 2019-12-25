using System.Collections;
using System.Collections.Generic;
using AOTE_HORDE.Data;
using Newtonsoft.Json;
using UnityEngine;

namespace AOTE_HORDE.Networking
{
	public class ServerResponse
	{

	}

	#region Waves Data Models
	public class WavesDataResponse
	{
		public readonly List<WavesData> WavesDataList;

		public WavesDataResponse( object value )
		{
			WavesDataList = JsonConvert.DeserializeObject<List<WavesData>>(value.ToString());
		}
	}

	public class EnemyArchtypesResponse
	{
		public readonly Dictionary<string, EnemyArchtype> EnemyArchtypes;
		public EnemyArchtypesResponse( object value )
		{
			EnemyArchtypes = JsonConvert.DeserializeObject<Dictionary<string, EnemyArchtype>>(value.ToString());
		}
	}

	public class GameplayDataResponse
	{
		public readonly GameplayData GameplayData;

		public GameplayDataResponse( object value )
		{
			GameplayData = JsonConvert.DeserializeObject<GameplayData>(value.ToString());
		}
	}

	public class MutagenSlotsResponse
	{
		public readonly Dictionary<string, Dictionary<string, MutagenSlot>> MutagenSlots;

		public MutagenSlotsResponse( object value )
		{
			MutagenSlots = JsonConvert.DeserializeObject<Dictionary<string, Dictionary<string, MutagenSlot>>>(value.ToString());
		}
	}

	public class MutagenUpgradeResponse
	{
		public readonly Dictionary<string, Dictionary<string, MutagenUpgrade>> MutagenUpgrades;

		public MutagenUpgradeResponse( object value )
		{
			MutagenUpgrades = JsonConvert.DeserializeObject<Dictionary<string, Dictionary<string, MutagenUpgrade>>>(value.ToString());
		}
	}
	#endregion
}