using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using PB.Client;

public class Api : MonoBehaviour 
{
	#region MemVars & Props

	/// <summary>
	/// Web Server Type 
	/// </summary>
	public PopBloopSettings.DevelopmentModeType DevelopmentMode = PopBloopSettings.DevelopmentModeType.LOCAL;
	
	/// <summary>
	/// Web Server Local
	/// </summary>
	public string serverLocal = PopBloopSettings.serverLocal;
	
	/// <summary>
	/// Web Server Staging
	/// </summary>
	public string serverDevelopment = PopBloopSettings.serverDevelopment;
	
	/// <summary>
	/// Web Server Staging
	/// </summary>
	public string serverStaging = PopBloopSettings.serverStaging;
	
	/// <summary>
	/// Web Server Production
	/// </summary>
	public string serverProduction = PopBloopSettings.serverProduction;
	
	private static Api api = null;
	public string apiPath = "koffice/api/";

	#endregion


	#region Mono Methods

	private void SetupServers()
	{
		PopBloopSettings.serverLocal = serverLocal;
		PopBloopSettings.serverStaging = serverStaging;
		PopBloopSettings.serverProduction = serverProduction;
		PopBloopSettings.serverDevelopment = serverDevelopment;
		PopBloopSettings.DevelopmentMode = DevelopmentMode;
	}

	protected void Awake()
	{
		api = this;
		SetupServers();
	}

	#endregion


	#region User Api & Static Methods

	public static string getApiUrl()
	{
		string apiPath = "koffice/api/";

		if (api != null)
		{
			apiPath = api.apiPath;
		}
		return string.Format("{0}{1}", PopBloopSettings.WebServerUrl, apiPath);
	}

	public static string GenerateApiParams(Dictionary<string, string> dict)
	{
		if (dict == null)
		{
			return "";
		}

		string param = "";
		foreach (KeyValuePair<string, string> pair in dict)
		{
			string part = string.Format("{0}={1}", pair.Key, pair.Value);
			if (param != "")
			{
				param += string.Format("&{0}", part);
			}
			else
			{
				param += part;
			}
		}

		return param.Replace(" ", "%20");
	}

	public enum UserApiRequest
	{
		GetProfile = 0,
		UserCheck = 1,
		Login = 2,
		UpdateStatus = 3,
		UpdateProperties = 4,
		NewUser = 5
	};
	

	private static string BuildUserRequest(UserApiRequest request, string parameter)
	{
		string ret = getApiUrl();
		string req = "";
		string token = string.Format("_random={0}", DateTime.Now.Ticks);
		switch (request)
		{
		case UserApiRequest.GetProfile:
			req = "user/get_profile?" + token;
			break;
			
		case UserApiRequest.Login:
			req = "user/login?" + token;
			break;
			
		case UserApiRequest.NewUser:
			req = "user/create?" + token;
			break;
			
		case UserApiRequest.UpdateProperties:
			req = "user/properties?" + token;
			break;
			
		case UserApiRequest.UpdateStatus:
			req = "user/status?" + token;
			break;
			
		case UserApiRequest.UserCheck:
			req = "user/check?" + token;
			break;
		}
		
		ret = string.Format("{0}{1}&{2}", ret, req, parameter);
		Debug.Log("Calling Api: " + ret);
		
		return ret;
	}
	
	public static void CallApi(GameObject sender, UserApiRequest request, string parameter, Action<Dictionary<string, object>> callback)
	{
		if (api != null)
		{
			api.callApi(sender, BuildUserRequest(request, parameter), callback);
		}
		else
		{
			Debug.LogWarning("Api is not iniatialized");
		}
	}
	
	public static void CallApi(UserApiRequest request, string parameter, Action<Dictionary<string, object>> callback)
	{
		CallApi(null, request, parameter, callback);
	}
	
	private void callApi(GameObject sender, string apiUrl, Action<Dictionary<string, object>> callback)
	{
		StartCoroutine(callApiThread(sender, apiUrl, callback));
	}
	
	private IEnumerator callApiThread(GameObject sender, string apiUrl, Action<Dictionary<string, object>> callBack)
	{
		WWW www = new WWW(apiUrl);
		
		if (sender != null)
		{
			sender.SetActive(false);
		}
		
		yield return www;
		
		if (www.error == null)
		{
			if (callBack != null)
			{
				var hash = Prime31.JsonExtensions.dictionaryFromJson(www.text);
				callBack(hash);
			}
		}
		else
		{
			StartCoroutine(callApiThread(sender, apiUrl, callBack));
		}
		
		if (sender != null)
		{
			sender.SetActive(true);
		}
	}
	
	
	#endregion
}
