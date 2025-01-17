﻿using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using Prime31;

#if UNITY_ANDROID
using FBConnect = FacebookAndroid;
#elif UNITY_IPHONE
using FBConnect = FacebookBinding;
#endif

public class UIFacebook : MonoBehaviourGUI
{
	public static void GetEmail(Action<bool, string> callback)
	{
		GetEmail(null, callback);
	}

	public static void GraphRequest(string path, Action<bool, Dictionary<string, object>> callback)
	{
		Facebook.instance.graphRequest(path, (e, o) => {
			if (e == null)
			{
				Dictionary<string, object> dic = (Dictionary<string, object>)o;

				Prime31.Utils.logObject(dic);

				if (callback != null)
				{
					callback(true, dic);
				}
			}
			else
			{
				if (callback != null)
				{
					callback(false, new Dictionary<string, object>());
				}
			}
		});
	}

	public static void GetEmail(GameObject sender, Action<bool, string> callback)
	{
		if (sender != null)
		{
			//sender.renderer.enabled = false;
		}

		if (Application.platform == RuntimePlatform.OSXEditor || Application.platform == RuntimePlatform.WindowsEditor)
		{
			if (callback != null)
			{
				callback(true, "syuaibi@yahoo.com");
			}
			return;
		}

		Facebook.instance.graphRequest("/me?fields=email", (e, o) => {
			if (e == null)
			{
				Dictionary<string, object> dic = (Dictionary<string, object>)o;
				string email = (string)dic["email"];
				
				if (callback != null)
				{
					callback(true, email);
				}
			}
			else
			{
				if (callback != null)
				{
					callback(false, "");
				}
			}
			//sender.renderer.enabled = true;
		});
	}
	
	#if (UNITY_ANDROID)

	public static void Logout()
	{
		Debug.Log("UIFacebook.Logout()");
		FBConnect.logout();
	}

	public static void Login()
	{
		Debug.Log("UIFacebook.Login(), IsSessionValid() = " + IsSessionValid().ToString());

		FBConnect.loginWithPublishPermissions(new string[] { "email", "user_birthday", "user_about_me", "user_friends", "user_location", "user_status","publish_actions", "manage_friendlists" });
		//FBConnect.reauthorizeWithPublishPermissions(new string[] { "publish_actions", "manage_friendlists" }, FacebookSessionDefaultAudience.Everyone);
	}

	public static void postImage(byte[] imageBytes, string message, System.Action<string, object> completionHandler)
	{
		Facebook.instance.postImage(imageBytes, message, completionHandler);
	}

	public static string GetAccessToken()
	{
		return FBConnect.getAccessToken();
	}

	public static bool IsSessionValid()
	{
		return FBConnect.isSessionValid();
	}

	public static string screenshotFilename = "kamarpas.png";

	// common event handler used for all Facebook graph requests that logs the data to the console
	void completionHandler( string error, object result )
	{
		if (error != null)
		{
			Debug.LogError(error);
		}
		else
		{
			Prime31.Utils.logObject(result);
		}
	}
	
	void Awake()
	{
		FBConnect.init();
	}
	
	void Start()
	{
		// grab a screenshot for later use
		//Application.CaptureScreenshot( screenshotFilename );
		
		// optionally enable logging of all requests that go through the Facebook class
		//Facebook.instance.debugRequests = true;
	}
	
	#else

	public static string GetAccessToken()
	{
		return "";
	}

	public static bool IsSessionValid()
	{
		return true;
	}
	
	public static bool Login()
	{
		Debug.LogWarning("Only works on Android or iPhone");
		return false;
	}
	
	#endif

	#if UNITY_ANDROID

	/*
	void OnGUI()
	{
		beginColumn();
		
		
		if( GUILayout.Button( "Initialize Facebook" ) )
		{
			FacebookAndroid.init();
		}
		
		
		if( GUILayout.Button( "Login" ) )
		{
			FacebookAndroid.loginWithReadPermissions( new string[] { "email", "user_birthday" } );
		}
		
		
		if( GUILayout.Button( "Reauthorize with Publish Permissions" ) )
		{
			FacebookAndroid.reauthorizeWithPublishPermissions( new string[] { "publish_actions", "manage_friendlists" }, FacebookSessionDefaultAudience.Everyone );
		}
		
		
		if( GUILayout.Button( "Logout" ) )
		{
			FacebookAndroid.logout();
		}
		
		
		if( GUILayout.Button( "Is Session Valid?" ) )
		{
			var isSessionValid = FacebookAndroid.isSessionValid();
			Debug.Log( "Is session valid?: " + isSessionValid );
		}
		
		
		if( GUILayout.Button( "Get Session Token" ) )
		{
			var token = FacebookAndroid.getAccessToken();
			Debug.Log( "session token: " + token );
		}
		
		
		if( GUILayout.Button( "Get Granted Permissions" ) )
		{
			var permissions = FacebookAndroid.getSessionPermissions();
			Debug.Log( "granted permissions: " + permissions.Count );
			Prime31.Utils.logObject( permissions );
		}
		
		
		endColumn( true );
		
		
		if( GUILayout.Button( "Post Image" ) )
		{
			var pathToImage = Application.persistentDataPath + "/" + screenshotFilename;
			var bytes = System.IO.File.ReadAllBytes( pathToImage );
			
			Facebook.instance.postImage( bytes, "im an image posted from Android", completionHandler );
		}
		
		
		if( GUILayout.Button( "Graph Request (me)" ) )
		{
			Facebook.instance.graphRequest( "me", completionHandler );
		}
		
		
		if( GUILayout.Button( "Post Message" ) )
		{
			Facebook.instance.postMessage( "im posting this from Unity: " + Time.deltaTime, completionHandler );
		}
		
		
		if( GUILayout.Button( "Post Message & Extras" ) )
		{
			Facebook.instance.postMessageWithLinkAndLinkToImage( "link post from Unity: " + Time.deltaTime, "http://prime31.com", "prime[31]", "http://prime31.com/assets/images/prime31logo.png", "Prime31 Logo", completionHandler );
		}
		
		
		if( GUILayout.Button( "Show Post Dialog" ) )
		{
			// parameters are optional. See Facebook's documentation for all the dialogs and paramters that they support
			var parameters = new Dictionary<string,string>
			{
				{ "link", "http://prime31.com" },
				{ "name", "link name goes here" },
				{ "picture", "http://prime31.com/assets/images/prime31logo.png" },
				{ "caption", "the caption for the image is here" }
			};
			FacebookAndroid.showDialog( "stream.publish", parameters );
		}
		
		
		if( GUILayout.Button( "Get Friends" ) )
		{
			Facebook.instance.getFriends( completionHandler );
		}
		
		
		endColumn();
		
		
		if( bottomLeftButton( "Twitter Scene" ) )
		{
			Application.LoadLevel( "TwitterTestScene" );
		}
	}*/
	
	#endif
}
