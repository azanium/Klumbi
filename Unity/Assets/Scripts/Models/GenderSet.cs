using UnityEngine;

using System.Collections;
using System.Collections.Generic;
using PB.Client;

public class GenderSet : JsonModel<GenderSet>
{
    public bool success { get; set; }
	public bool logged_in { get; set; }
    public string message { get; set; }

    public static string SetGenderApi(string userid, string gender)
    {
		return string.Format("{0}user/set_gender/{1}/{2}", Api.getApiUrl(), userid, gender);
    }

    public static string GetSetBodyTypeApi(string email, string bodysize)
    {
        return string.Format("{0}api/mobile/avatar/setbodytype/{1}/{2}?rand={3}", PopBloopSettings.WebServerUrl, email, bodysize, UnityEngine.Time.deltaTime);
    }
}
