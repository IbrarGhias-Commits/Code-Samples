using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;
using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using AOTE_HORDE.Data;

namespace AOTE_HORDE.Networking
{
	public class LocalTestingService : BaseNetworkService
	{
		private const string METADATA = "Metadata_HordeMode/metadata_horde_mode";
		private const string USERDATA = "Metadata_HordeMode/userdata_horde_mode";

		private Dictionary<string, object> userData = new Dictionary<string, object>();

		#region Initializers
		protected override void Initialize()
		{
			Debug.Log("---------- Local Testing Initialized ----------");
		}
		protected override void UnInitialize()
		{
			Debug.Log("---------- Local Testing UnInitialized ----------");
		}
		#endregion

		public override void Login( Action<LoginResult> OnSuccess, Action<PlayFabError> OnFailed )
		{
			LoginResult result = new LoginResult()
			{
				NewlyCreated = true,
				PlayFabId = "localTesting",
				SessionTicket = "localTestingSessionTicket",
				LastLoginTime = DateTime.MinValue,
				SettingsForUser = new UserSettings()
				{
					GatherDeviceInfo = true,
					GatherFocusInfo = true,
					NeedsAttribution = true
				}
			};

			if ( OnSuccess != null )
			{
				OnSuccess(result);
			}
		}

		public override void GetMetaData( Action<Dictionary<string, object>> OnSuccess, Action<PlayFabError> OnFailed )
		{
			var jsonText = Resources.Load<TextAsset>(METADATA);
			if ( jsonText != null )
			{
				//var data = JsonConvert.DeserializeObject<Dictionary<string, string>>(jsonText.text);
				var data = JsonConvert.DeserializeObject<Dictionary<string, object>>(jsonText.text);
				if ( data != null )
				{
					if ( OnSuccess != null )
					{
						OnSuccess.Invoke(data);
					}
				}
				else
				{
					if ( OnFailed != null )
					{
						OnFailed.Invoke(new PlayFabError()
						{
							Error = PlayFabErrorCode.UnknownError,
							ErrorMessage = "Can't Deserialize Json Data to Dictionary<string,string>"
						});
					}
				}
			}
			else
			{
				if ( OnFailed != null )
				{
					OnFailed.Invoke(new PlayFabError()
					{
						Error = PlayFabErrorCode.UnknownError,
						ErrorMessage = "Can't Read Metadata from Resources"
					});
				}
			}
		}

		public override void GetUserData( List<string> keys, Action<Dictionary<string, object>> OnSuccess, Action<PlayFabError> OnFailed )
		{
			var jsonText = Resources.Load<TextAsset>(USERDATA);
			if ( jsonText != null )
			{
				userData = JsonConvert.DeserializeObject<Dictionary<string, object>>(jsonText.text);
				if ( userData != null )
				{
					if ( OnSuccess != null )
					{
						OnSuccess.Invoke(userData);
					}
				}
				else
				{
					if ( OnFailed != null )
					{
						OnFailed.Invoke(new PlayFabError()
						{
							Error = PlayFabErrorCode.UnknownError,
							ErrorMessage = "Can't Deserialize Json Data to Dictionary<string,string>"
						});
					}
				}
			}
			else
			{
				if ( OnFailed != null )
				{
					OnFailed.Invoke(new PlayFabError()
					{
						Error = PlayFabErrorCode.UnknownError,
						ErrorMessage = "Can't Read Userdata from Resources"
					});
				}
			}
		}

		public override void UpdateUserData( Dictionary<string, string> data, Action<uint> OnSuccess, Action<PlayFabError> OnFailed )
		{
			foreach ( var item in data )
			{
				if ( userData.ContainsKey(item.Key) )
				{
					userData[item.Key] = item.Value;
				}
			}
			if ( OnSuccess != null )
				OnSuccess.Invoke(0);
		}

		public override void UpdateUserMutagenUpgrade( List<UserMutagenUpgrade> userMutagen, Action<uint> OnSuccess, Action<PlayFabError> OnFailed )
		{
			Dictionary<string, string> data = new Dictionary<string, string>();
			data.Add(UserMutagenUpgradeData.USER_MUTAGEN_UPGRADES_KEY, JsonConvert.SerializeObject(userMutagen));
			UpdateUserData(data, OnSuccess, OnFailed);
		}
	}
}