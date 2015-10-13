using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Stream : JsonModel<Stream> 
{
	public class RegisteredUser
	{
		public string id { get; set; }
		public string email { get; set; }
		public bool artist { get; set; }
		public string datetime { get; set; }
		public string fullname { get; set; }
		public string sex { get; set; }
		public string picture { get; set; }
	}

	public class News
	{
	}

	public class AvatarItem
	{
		public string id { get; set; }
		public string name { get; set; }
		public string category { get; set; }
		public string tipe { get; set; }
		public string datetime { get; set; }
		public string picture { get; set; }
	}

	public class Follower
	{
		public string id { get; set; }
		public string datetime { get; set; }
		public string fullname { get; set; }
		public string avatarname { get; set; }
		public string link_id { get; set; }
		public string sex { get; set; }
		public string picture { get; set; }
	}

	public class Notification
	{
		public string id { get; set; }
		public string header { get; set; }
		public string detail { get; set; }
		public string link_id { get; set; }
		public string datetime { get; set; }
	}

	public bool success { get; set; }
	public bool logged_in { get; set; }

	//public int count_register { get; set; }
	//public List<RegisteredUser> data_new_register { get; set; }

	//public int count_news { get; set; }
	//public List<News> data_news { get; set; }

	public int count_avataritem { get; set; }
	public List<AvatarItem> data_avataritem { get; set; }

	public int count_new_follower { get; set; }
	public List<Follower> data_new_follower { get; set; }

	public int count_notification { get; set; }
	public List<Notification> data_notification { get; set; }

	public static string GetStreamAPI(string userid, int daysToUpdate)
	{
		return string.Format("{0}stream/getdata/{1}/{2}", Api.getApiUrl(), userid, daysToUpdate);
	}
}
