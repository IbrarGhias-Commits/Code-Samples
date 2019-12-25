using System.Collections;
using System.Collections.Generic;
using AOTE_HORDE.Data;
using Newtonsoft.Json;
using UnityEngine;

namespace AOTE_HORDE.Networking
{
	public class UserMutagenUpgradeResponse
	{
		public readonly List<UserMutagenUpgrade> MutagenUpgradesUser;

		public UserMutagenUpgradeResponse( object value )
		{
			MutagenUpgradesUser = JsonConvert.DeserializeObject<List<UserMutagenUpgrade>>(value.ToString());
		}
	}

	public class UserWalletResponse
	{
		public readonly UserWallet UserWallet;

		public UserWalletResponse( object value )
		{
			UserWallet = JsonConvert.DeserializeObject<UserWallet>(value.ToString());
		}
	}
}