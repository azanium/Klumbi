using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using PB.Client;

public class Comments : JsonModel<Comments>
{ 
    public class Comment
    {
        public string _id { get; set; }
		public string datetime { get; set; }
		public string comment { get; set; }
		public string user_id { get; set; }
		public string sex { get; set; }
		public string fullname { get; set; } 
        public string picture { get; set; }
    }

	public bool success { get; set; }
	public bool logged_in { get; set; }
	public int count { get; set; }
	public List<Comment> data { get; set; }

    public static string ListCommentsAvatarItemApi(string avatarItemId, int start, int limit)
    {
		return string.Format("{0}social/avataritem/list_comment/{1}/{2}/{3}/", Api.getApiUrl(), avatarItemId, start, limit);
    }

    public static string ListCommentsAvatarMixApi(string mixId, int start, int limit)
    {
		return string.Format("{0}social/avatarmix/list_comment/{1}/{2}/{3}/", Api.getApiUrl(), mixId, start, limit);
    }
}

public class CommentsCount : JsonModel<CommentsCount>
{
	public bool success { get; set; }
	public bool logged_in { get; set; }
	public int count { get; set; }

    public static string GetCountCommentsAvatarMixApi(string mixId)
    {
		return string.Format("{0}social/avatarmix/count_comment/{1}", Api.getApiUrl(), mixId);
    }

    public static string GetCountCommentsAvatarItemApi(string avatarItemId)
    {
		return string.Format("{0}social/avataritem/count_comment/{1}", Api.getApiUrl(), avatarItemId);
    }

    public static string CreateCommentAvatarItemApi()
    {
		return string.Format("{0}social/avataritem/add_comment", Api.getApiUrl());
    }

    public static string CreateCommentAvatarMixApi()
    {
		return string.Format("{0}social/avatarmix/add_comment", Api.getApiUrl());
    }

	public static string DeleteCommentAvatarItemApi(string commentId)
	{
		return string.Format("{0}social/avataritem/delete_comment/{1}", Api.getApiUrl(), commentId);
	}
}
