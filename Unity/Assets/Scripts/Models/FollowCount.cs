using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using PB.Client;

public class FollowCount : JsonModel<FollowCount> 
{
	public bool success { get; set; }
	public bool logged_in { get; set; }
	public int count { get; set; }

	public static string CountFollowerApi(string userid)
	{
		return string.Format("{0}social/user/count_follower/{1}", Api.getApiUrl(), userid);
	}

	public static string CountFollowingApi(string userid)
	{
		return string.Format("{0}social/user/count_following/{1}", Api.getApiUrl(), userid);
	}
}
