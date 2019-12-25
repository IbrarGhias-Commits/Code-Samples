using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;
using System;
using System.Linq;
using AOTE_HORDE.Data;
using Newtonsoft.Json;

namespace AOTE_HORDE.Networking
{
	public partial class PlayFabNetworkService : BaseNetworkService
	{
		#region Variables
		const string TitleId = "2F89F";
		#endregion
		#region Initializers
		protected override void Initialize()
		{
			Debug.Log("---------- Playfab Service Initialized ----------");
			//Note: Setting title Id here can be skipped if you have set the value in Editor Extensions already.
			if ( string.IsNullOrEmpty(PlayFabSettings.TitleId) )
			{
				PlayFabSettings.TitleId = PlayFabNetworkService.TitleId; // Please change this value to your own titleId from PlayFab Game Manager
			}
		}

		protected override void UnInitialize()
		{
			Debug.Log("---------- Playfab Service UnInitialized ----------");
		}
		#endregion

		public override void Login( Action<LoginResult> OnSuccess, Action<PlayFabError> OnFailed )
		{
			LoginInternal(OnSuccess, OnFailed);
		}

		public override void GetMetaData( Action<Dictionary<string, object>> OnSuccess, Action<PlayFabError> OnFailed )
		{
			PlayFabClientAPI.GetTitleData(new GetTitleDataRequest(), ( result ) =>
			{
				var newData = result.Data.ToDictionary(pair => pair.Key, pair => (object)pair.Value);
				if ( OnSuccess != null )
					OnSuccess.Invoke(newData);
			}, OnFailed);
		}

		public override void GetUserData( List<string> keys, Action<Dictionary<string, object>> OnSuccess, Action<PlayFabError> OnFailed )
		{
			PlayFabClientAPI.GetUserData(new GetUserDataRequest() { Keys = keys }, ( result ) =>
			{
				Dictionary<string, object> data = new Dictionary<string, object>();
				foreach ( var pair in result.Data )
				{
					data.Add(pair.Key, pair.Value.Value as object);
				}

				if ( OnSuccess != null )
				{
					OnSuccess(data);
				}
			}, OnFailed);
		}

		public override void UpdateUserData( Dictionary<string, string> data, Action<uint> OnSuccess, Action<PlayFabError> OnFailed )
		{
			PlayFabClientAPI.UpdateUserData(new UpdateUserDataRequest()
			{
				Data = data
			}, ( result ) =>
			{
				if ( OnSuccess != null )
					OnSuccess(result.DataVersion);
			}, OnFailed);
		}

		public override void UpdateUserMutagenUpgrade( List<UserMutagenUpgrade> userMutagen, Action<uint> OnSuccess, Action<PlayFabError> OnFailed )
		{
			Dictionary<string, string> data = new Dictionary<string, string>();
			data.Add(UserMutagenUpgradeData.USER_MUTAGEN_UPGRADES_KEY, JsonConvert.SerializeObject(userMutagen));
			UpdateUserData(data, OnSuccess, OnFailed);
		}
	}
}