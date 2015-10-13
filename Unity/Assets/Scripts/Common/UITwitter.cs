using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

using Prime31;

public class UITwitter : MonoBehaviourGUI
{
	#region MemVars & Props

	public static event Action<bool> onLogin;
	public static event Action<object> onRequestFinished;
	public static event Action<string> onRequestFailed;
	public static Action<object> onCompleted;

	public static string loggedinUsername = "";

#if UNITY_ANDROID

	public void Awake()
	{
		TwitterAndroid.init("rtVCCtA1s9kb94ffnBsc90SCF", "IlK4fySaLtxJoiRly8TTPXAYQ2UxLFZsWqNBeSfJCGTO7sPXrT");
	}

	// Listens to all the events.  All event listeners MUST be removed before this object is disposed!
	void OnEnable()
	{
		TwitterManager.loginSucceededEvent += loginSucceeded;
		TwitterManager.loginFailedEvent += loginFailed;
		TwitterManager.requestDidFinishEvent += requestDidFinishEvent;
		TwitterManager.requestDidFailEvent += requestDidFailEvent;
		TwitterManager.tweetSheetCompletedEvent += tweetSheetCompletedEvent;
	}
	
	
	void OnDisable()
	{
		// Remove all the event handlers
		// Twitter
		TwitterManager.loginSucceededEvent -= loginSucceeded;
		TwitterManager.loginFailedEvent -= loginFailed;
		TwitterManager.requestDidFinishEvent -= requestDidFinishEvent;
		TwitterManager.requestDidFailEvent -= requestDidFailEvent;
		TwitterManager.tweetSheetCompletedEvent -= tweetSheetCompletedEvent;
	}

	// Twitter events
	void loginSucceeded( string username )
	{
		Debug.Log( "Successfully logged in to Twitter: " + username );
		if (onLogin != null)
		{
			onLogin(true);
		}
	}
	
	
	void loginFailed( string error )
	{
		Debug.Log( "Twitter login failed: " + error );

		if (onLogin != null)
		{
			onLogin(false);
		}
	}
	
	
	void requestDidFailEvent( string error )
	{
		Debug.Log( "requestDidFailEvent: " + error );

		if (onRequestFailed != null)
		{
			onRequestFailed(error);
		}
	}
	
	
	void requestDidFinishEvent( object result )
	{
		if( result != null )
		{
			Debug.Log( "requestDidFinishEvent" );
			Prime31.Utils.logObject( result );
		}

		if (onRequestFinished != null)
		{
			onRequestFinished(result);
		}

		if (onCompleted != null)
		{
			onCompleted(result);
			onCompleted = null;
		}
	}
	
	
	void tweetSheetCompletedEvent( bool didSucceed )
	{
		Debug.Log( "tweetSheetCompletedEvent didSucceed: " + didSucceed );
	}

#endif

	#endregion


	#region Mono Methods

	#endregion


	#region Internal Methods

	#endregion


	#region Public Methods

	public static bool isLoggedIn()
	{
#if UNITY_ANDROID
		return TwitterAndroid.isLoggedIn();
#else
			return false;
#endif
	}

	public static void login()
	{
#if UNITY_ANDROID
		TwitterAndroid.showLoginDialog();
#endif
	}

	public static void logout()
	{ 
#if UNITY_ANDROID
		TwitterAndroid.logout();
#endif
	}

	public static void performRequest(string methodType, string request, Dictionary<string, string> param)
	{
#if UNITY_ANDROID
		TwitterAndroid.performRequest(methodType, request, param);
#endif
	}

	public static void postImage(byte[] imageBytes, string status, System.Action<object> onCompleteHandler)
	{
#if UNITY_ANDROID
		onCompleted = onCompleteHandler;
		TwitterAndroid.postStatusUpdate(status, imageBytes);
#endif
	}

	#endregion
}
