using UnityEngine;
using System.Collections;
using PB.Client;

public class Like : JsonModel<Like> 
{
	public bool success { get; set; }
	public bool logged_in { get; set; }
    public bool like { get; set; }
    public int count { get; set; }
    public string message { get; set; }

    public static string CheckLikeMixApi(string userid, string mix_id)
    {
		return string.Format("{0}social/avatarmix/cek_like/{1}/{2}", Api.getApiUrl(), mix_id, userid);
    }

    public static string LikeMixApi(string userid, string mix_id)
    {
		return string.Format("{0}social/avatarmix/button_like/{1}/{2}", Api.getApiUrl(), mix_id, userid);
    }

    public static string CheckLikeAvatarApi(string userid, string avatar_id)
    {
		return string.Format("{0}social/avataritem/cek_like/{1}/{2}", Api.getApiUrl(), avatar_id, userid);
    }

    public static string LikeAvatarApi(string userid, string avatar_id)
    {
		return string.Format("{0}social/avataritem/button_like/{1}/{2}", Api.getApiUrl(), avatar_id, userid);
    }

	public static string CheckLikeUserPageApi(string userid, string friendid)
	{
		return string.Format("{0}social/user/cek_pagelike/{1}/{2}/", Api.getApiUrl(), userid, friendid);
	}

	public static string LikeUnlikeUserPageApi(string userid, string friendid)
	{
		return string.Format("{0}social/user/button_pagelike/{1}/{2}", Api.getApiUrl(), userid, friendid);
	}
}
