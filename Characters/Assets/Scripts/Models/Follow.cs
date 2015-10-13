using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Follow : JsonModel<Follow> 
{
	public bool success { get; set; }
	public bool logged_in { get; set; }
	public bool follow { get; set; }
	public string message { get; set; }
	
	public static string CheckFollowUserApi(string userid, string friendid)
	{
		return string.Format("{0}social/user/cek_follower/{1}/{2}", Api.getApiUrl(), userid, friendid);
	}

	public static string FollowUnfollowUserApi(string userid, string friendid)
	{
		return string.Format("{0}social/user/button_follower/{1}/{2}", Api.getApiUrl(), userid, friendid);
	}
}