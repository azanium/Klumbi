using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class People : JsonModel<People> 
{
	public class UserData
	{
		public string _id { get; set; }
		public string avatarname { get; set; }
		public string fullname { get; set; }
		public string picture { get; set; }
		public string email { get; set; }
		public string username { get; set; }
		public string fb_id { get; set; }
		public string twitter_id { get; set; }
	}

	public bool success { get; set; }
	public bool logged_in { get; set; }
	public bool follow { get; set; }
	public int count { get; set; }
	public string message { get; set; }
	public List<UserData> data { get; set; }

	public static string GetPeoplesApi(int start)
	{
		return string.Format("{0}search/people/{1}", Api.getApiUrl(), start);
	}
}
