using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LikeUserPage : JsonModel<LikeUserPage> 
{
	public bool success { get; set; }
	public bool logged_in { get; set; }
	public bool like { get; set; }
	public string message { get; set; }
	public int count { get; set; }
	
	public static string CheckLikeUserPageApi(string userid, string friendid)
	{
		return string.Format("{0}social/user/cek_pagelike/{1}/{2}/", Api.getApiUrl(), userid, friendid);
	}
	
	public static string LikeUnlikeUserPageApi(string userid, string friendid)
	{
		return string.Format("{0}social/user/button_pagelike/{1}/{2}", Api.getApiUrl(), userid, friendid);
	}
}
