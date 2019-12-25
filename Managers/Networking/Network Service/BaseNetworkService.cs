using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using PlayFab;
using PlayFab.ClientModels;
using AOTE_HORDE.Data;

namespace AOTE_HORDE.Networking
{
	public abstract class BaseNetworkService
	{
		#region Initializers
		public BaseNetworkService()
		{
			Initialize();
		}
		~BaseNetworkService()
		{
			UnInitialize();
		}
		protected abstract void Initialize();
		protected abstract void UnInitialize();
		#endregion

		public abstract void Login( Action<LoginResult> OnSuccess, Action<PlayFabError> OnFailed );

		public abstract void GetMetaData( Action<Dictionary<string, object>> OnSuccess, Action<PlayFabError> OnFailed );

		public abstract void GetUserData( List<string> keys, Action<Dictionary<string, object>> OnSuccess, Action<PlayFabError> OnFailed );

		public abstract void UpdateUserData( Dictionary<string, string> data, Action<uint> OnSuccess, Action<PlayFabError> OnFailed );

		public abstract void UpdateUserMutagenUpgrade( List<UserMutagenUpgrade> userMutagen, Action<uint> OnSuccess, Action<PlayFabError> OnFailed );
	}
}



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