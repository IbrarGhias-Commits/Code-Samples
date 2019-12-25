using UnityEngine;
using System.Collections;
using System;
using PlayFab;
using PlayFab.ClientModels;

namespace AOTE_HORDE.Networking
{
	// Networking Main Manager
	public partial class NetworkManager : MonoBehaviour
	{
		public static Action OnConnection;
		public static Action OnConnectionInterrupted;
		public static Action OnConnectionRestored;

		private bool isFirstConnection = true;
		private bool isConnected = false;
		public bool IsConnected
		{
			get
			{
				return isConnected;
			}
			set
			{
				if ( isFirstConnection )
				{
					if ( value )
					{
						isFirstConnection = false;
						OnConnection();
					}
				}
				else { if ( value ) OnConnectionRestored(); else OnConnectionInterrupted(); }
				isConnected = value;
			}
		}

		public bool TestLocal = false;

		private DateTime PingTime;

		public const string echoServer = "http://google.com";

		private BaseNetworkService networkService;
		public BaseNetworkService NetworkService
		{
			get { return networkService; }
			set { networkService = value; }
		}

		void Awake()
		{
			isConnected = true;
			//StartCoroutine(CheckInternetConnection(( connected ) =>
			//{
			//	Debug.Log("No Internet Connection", this);
			//	// User Has Internet Connection
			//	if ( isConnected )
			//	{
			//		if ( AuthService.AuthType.Equals(Authtypes.None) )
			//			AuthService.Authenticate(Authtypes.Silent);
			//		else
			//			AuthService.Authenticate();
			//	}
			//}));

			if ( TestLocal )
			{
				NetworkService = new LocalTestingService();
			}
			else
			{
				NetworkService = new PlayFabNetworkService();
			}
		}

		IEnumerator CheckInternetConnection( Action<bool> ConnectionCreated = null )
		{
			Debug.Log("Check Internet Connection", this);
			WWW www = new WWW(echoServer);
			yield return www;
			if ( www.error != null )
			{
				Debug.Log("Is Not Connected");
				isConnected = false;

				if ( ConnectionCreated != null )
					ConnectionCreated(false);
			}
			else
			{
				Debug.Log("Is Connected");
				isConnected = true;

				if ( ConnectionCreated != null )
					ConnectionCreated(true);
			}
		}
	}
}
