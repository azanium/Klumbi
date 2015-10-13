using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LikeCount : JsonModel<LikeCount> 
{
	public bool success { get; set; }
	public bool logged_in { get; set; }
	public int count { get; set; }
	
	public static string CountLikeUserPageApi(string userid)
	{
		return string.Format("{0}social/user/count_pagelike/{1}", Api.getApiUrl(), userid);
	}
	
}
