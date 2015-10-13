using UnityEngine;

using System.Collections;
using System.Collections.Generic;
using PB.Client;

public class AvatarConfig : JsonModel<AvatarConfig>
{
    public string user_id { get; set; }
	public bool logged_in { get; set; }
    public bool success { get; set; }
    //public string bodyType { get; set; }
    public string configurations { get; set; }
	public string configurations2 { get; set; } 

	public static string GetDefaultAvatarConfigApiV2(string userid, string bodySize, string gender)
	{
		Dictionary<string, string> param = new Dictionary<string, string>()
		{
			{ "user_id", userid },
			{ "bodysize", bodySize },
			{ "gender", gender }
		};
		return string.Format("{0}avatar/active_configurations?{1}", Api.getApiUrl(), Api.GenerateApiParams(param));
	}

	public static string GetDefaultAvatarConfigV2(string userid)
	{
		Dictionary<string, string> param = new Dictionary<string, string>()
		{
			{ "user_id", userid }
		};
		return string.Format("{0}avatar/active_configurations?{1}", Api.getApiUrl(), Api.GenerateApiParams(param));
	}

}

public class AvatarSetConfig : JsonModel<AvatarSetConfig>
{
	public bool success { get; set; }
	public bool logged_in { get; set; }

	public static string SetAvatarConfigurationApi()
	{
		return string.Format("{0}avatar/setactive_configurations", Api.getApiUrl());
	}
}
