using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class CommentItem : MonoBehaviour 
{
	#region MemVars & Props

	public UITexture previewTexture;
	public UILabel avatarNameLabel;
	public UILabel commentLabel;
	public UILabel timeLabel;
	public GameObject contextMenu;

	public Action<CommentItem> onShowCloseContextMenu;
	public Action<CommentItem> onDeleteComment;

	#endregion


	#region Mono Methods

	#endregion


	#region Internal Methods

	#endregion


	#region Public Methods

	public void SetAvatarName(string avatarName)
	{
		if (avatarNameLabel)
		{
			avatarNameLabel.text = avatarName;
		}
	}

	public void SetPreviewTexture(string picturePath)
	{
		string urlImage = AvatarItems.GetImagePath(picturePath);
		Debug.Log("Downloading Comment Item Picture: " + urlImage);
		NetworkController.DownloadImageFromUrl(urlImage, (www) => {
			if (www.texture != null && previewTexture != null)
			{
				previewTexture.mainTexture = www.texture;
			}
		});
	}

	public void SetComment(string comment)
	{
		if (commentLabel)
		{
			commentLabel.text = comment;
		}
	}

	public void SetTime(string time)
	{
		if (timeLabel)
		{
			timeLabel.text = time;
		}
	}

	public void OnShowContextMenu()
	{
		if (contextMenu)
		{
			contextMenu.gameObject.SetActive(true);

			if (this.onShowCloseContextMenu != null)
			{
				onShowCloseContextMenu(this);
			}
		}
	}

	public void OnHideContextMenu()
	{
		if (contextMenu)
		{
			contextMenu.gameObject.SetActive(false);
			if (this.onShowCloseContextMenu != null)
			{
				onShowCloseContextMenu(this);
			}
		}
	}

	public void OnDelete()
	{
		contextMenu.gameObject.SetActive(false);
		if (this.onDeleteComment != null)
		{
			this.onDeleteComment(this);
		}
	}

	public void SetCallback(Action<CommentItem> OnDeleteComment, Action<CommentItem> OnShowCloseContextMenu)
	{
		this.onDeleteComment = OnDeleteComment;
		this.onShowCloseContextMenu = OnShowCloseContextMenu;
	}

	#endregion
}
