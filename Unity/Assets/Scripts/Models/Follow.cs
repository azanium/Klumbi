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

public class FollowList : JsonModel<FollowList>
{
	public class FollowData
	{
		public string user_id { get; set; }
		public string datetime { get; set; }
		public string fullname { get; set; }
		public string sex { get; set; }
		public string picture { get; set; }
	}

	public bool success { get; set; }
	public bool logged_in { get; set; }
	public int count { get; set; }
	public List<FollowData> data { get; set; }

	public static string ListFollowingsApi(string userid)
	{
		return string.Format("{0}social/user/list_following/{1}", Api.getApiUrl(), userid);
	}

	public static string ListFollowersApi(string userid)
	{
		return string.Format("{0}social/user/list_follower/{1}", Api.getApiUrl(), userid);
	}
}