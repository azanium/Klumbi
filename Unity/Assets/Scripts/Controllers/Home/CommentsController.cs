using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CommentsController : UIViewController
{
	#region MemVars & Props

	public GameObject commentItemPrefab;

	public UILabel titleLabel;
	public UILabel detailLabel;
	public UITexture previewTexture;
	public UIInput inputComment; 
	public UITable tableComment;
	public UILabel commentsHeadingLabel;
	public GameObject notificationPanel;
	
	private string currentId, currentFunc;
	
	private List<GameObject> commentObjects = new List<GameObject>();
	private Dictionary<CommentItem, string> commentIds = new Dictionary<CommentItem, string>();

	
	#endregion
	
	
	#region Mono Methods
	
	#endregion
	
	
	#region UIViewController's Methods
	
	public override void OnAppear()
	{
		base.OnAppear();
	
		Clear();

		string picture = GetParameter("picture");
		string title = GetParameter("title");
		string desc = GetParameter("description");
		
		titleLabel.text = title;
		detailLabel.text = desc;
		string urlImage = AvatarItems.GetImagePath(picture);
		Debug.Log("Downloading Picture: " + urlImage);
		NetworkController.DownloadImageFromUrl(urlImage, OnImageDownloaded);
	}

	private void OnImageDownloaded(WWW www)
	{
		if (www.texture != null && previewTexture != null)
		{
			previewTexture.mainTexture = www.texture;
		}
	}
	
	public void Clear()
	{
		foreach (var obj in commentObjects)
		{
			Destroy(obj);
		}
		commentObjects.Clear();
		commentIds.Clear();

		if (notificationPanel != null)
		{
			notificationPanel.gameObject.SetActive(false);
		}
	}
	
	public override void OnAppeared()
	{
		base.OnAppeared();
		
		var param = this.controllerParameters;
		
		if (param.Count == 0 || 
		    !param.ContainsKey("f") ||
		    !param.ContainsKey("v"))
		{
			return;
		}
		
		var func = param["f"];
		var val = param["v"];
		currentFunc = func;
		currentId = val;

		if (!string.IsNullOrEmpty(func))
		{
			if (func == "character")
			{
				NetworkController.DownloadFromUrl(Comments.ListCommentsAvatarMixApi(val, 0, 0), OnCommentsDownloadFinished);
			}
			else if (func == "element")
			{
				NetworkController.DownloadFromUrl(Comments.ListCommentsAvatarItemApi(val, 0, 0), OnCommentsDownloadFinished);
			}
		}
	}
	
	#endregion
	
	
	#region Internal & Public Methods

	private void OnCommentsDownloadNotify(WWW www)
	{
		string json = www.text;
		
		Debug.LogWarning("Comments JSON: " + json);
		
		var comments = Comments.CreateObject(json);
		if (comments != null)
		{
			commentsHeadingLabel.text = comments.count.ToString();
			Clear();
			
			StartCoroutine(PublishComment(comments));
		}
		StartCoroutine(showNotification("Comment posted."));
	}

	private void OnCommentsDownloadFinished(WWW www)
	{
		string json = www.text;
		
		Debug.LogWarning("Comments JSON: " + json);
		
		var comments = Comments.CreateObject(json);
		if (comments != null)
		{
			commentsHeadingLabel.text = comments.count.ToString();
			Clear();
			
			StartCoroutine(PublishComment(comments));
		}
	}

	private IEnumerator RefreshTable()
	{
		yield return new WaitForEndOfFrame();
		
		if (tableComment != null) 
		{
			tableComment.Reposition();
		}
	}

	private void OnCommentDeleted(WWW www)
	{
		//var email = Konstants.GetSettingsProfile().GetString(Konstants.kdEmail);
		//string id = commentIds[dc];

		//Debug.Log("Delete Comment: Email: " + email + ", currentId: " + currentId + ", commentId: " + id +", response: " + www.text);
		Debug.Log("Comment deleted");
		OnReloadComments();
		
		showNotification("Comment deleted.");

	}

	private IEnumerator PublishComment(Comments comments)
	{
		foreach (var com in comments.data)
		{
			if (tableComment != null && commentItemPrefab)
			{
				GameObject obj = (GameObject)Instantiate(commentItemPrefab, Vector3.zero, Quaternion.identity);
				obj.transform.parent = tableComment.transform;
				obj.transform.localScale = new Vector3(1, 1, 1);
				obj.transform.position = Vector3.zero;

				CommentItem commentItem = obj.GetComponent<CommentItem>();

				if (commentItem != null)
				{
					commentIds.Add(commentItem, com._id);

					commentItem.SetAvatarName(com.fullname); // TODO: Should be com.username
					commentItem.SetComment(com.comment);
					commentItem.SetTime(com.datetime);
					commentItem.SetPreviewTexture(com.picture);
					commentItem.SetCallback(
					(dc) => {
						string id = commentIds[dc];
						var request = CommentsCount.DeleteCommentAvatarItemApi(id);
						NetworkController.DownloadFromUrl(request, OnCommentDeleted);
						tableComment.repositionNow = true;
					}, 
					(sc) => {
						tableComment.repositionNow = true;
					});

					inputComment.value = "";


				}
				
				commentObjects.Add(obj);
			}
		}
		
		yield return new WaitForEndOfFrame();

		if (tableComment != null) 
		{
			tableComment.Reposition();
		}
	}

	private IEnumerator showNotification(string info)
	{
		if (notificationPanel != null)
		{
			notificationPanel.gameObject.SetActive(true);
			notificationPanel.GetComponent<NotificationPanel>().SetDescription(info);
		}
		yield return new WaitForSeconds(3);

		if (notificationPanel != null)
		{
			notificationPanel.gameObject.SetActive(false);
		}
	}
	
	public void OnComment()
	{
		var text = inputComment.value;
		if (text != "" && currentId != "")
		{
			//var email = Konstants.GetSettingsProfile().GetString(Konstants.kdEmail);
			var data = GameData.Load();
			var userid = data.profile._id;

			if (currentFunc == "character")
			{
				Dictionary<string, object> post = new Dictionary<string, object>()
				{
					{ "comment", text },
					{ "user_id", userid },
					{ "mix_id", currentId }
				};

				NetworkController.DownloadFromUrl(CommentsCount.CreateCommentAvatarMixApi(), post, (www) =>
				                                  {
					NetworkController.DownloadFromUrl(Comments.ListCommentsAvatarMixApi(currentId, 0, 0), OnCommentsDownloadNotify);
				});
			}
			else if (currentFunc == "element")
			{
				Dictionary<string, object> post = new Dictionary<string, object>()
				{
					{ "comment", text },
					{ "user_id", userid },
					{ "avatar_id", currentId }
				};

				NetworkController.DownloadFromUrl(CommentsCount.CreateCommentAvatarItemApi(), post, (www) =>
				                                  {
					NetworkController.DownloadFromUrl(Comments.ListCommentsAvatarItemApi(currentId, 0, 0), OnCommentsDownloadNotify);
				});
			}
		}
	}

	public void OnReloadComments()
	{
		if (currentFunc == "character")
		{
			NetworkController.DownloadFromUrl(Comments.ListCommentsAvatarMixApi(currentId, 0, 0), OnCommentsDownloadFinished);
		}
		else if (currentFunc == "element")
		{
			NetworkController.DownloadFromUrl(Comments.ListCommentsAvatarItemApi(currentId, 0, 0), OnCommentsDownloadFinished);
		}
	}
	
	#endregion
}
