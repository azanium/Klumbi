using UnityEngine;
using System.Collections;
using PB.Client;

public class AvatarMix : JsonModel<AvatarMix>
{
    public bool success { get; set; }
	public bool logged_in { get; set; }
	public string message { get; set; }
    //public string filename { get; set; }
    //public string message { get; set; }


    public static string CreateMixApi(string email, string mixName)
    {
        return string.Format("{0}api/mobile/mix/create/{1}/{2}", PopBloopSettings.WebServerUrl, email, WWW.EscapeURL(mixName));
    }

	public static string CreateMixApiV2()
	{
		return string.Format("{0}avatarmix/add_configurations", Api.getApiUrl());
	}
}
