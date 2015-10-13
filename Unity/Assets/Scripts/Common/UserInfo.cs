using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class UserInfo : MonoBehaviour 
{
	#region MemVars & Props

	private UILabel label;
	public UILabel textLabel;
	public System.Action<UserInfo> OnClicked;

	public System.Object userObject;

	#endregion

	
	#region Mono Methods

	private void Start()
	{
		label = GetComponentInChildren<UILabel>();
	}

	public void FetchPicture(string url)
	{
		NetworkController.DownloadFromUrl(url, (www) => {
			if (www.error != null)
			{
				return;
			}

			UITexture picture = GetComponent<UITexture>();
			if (picture != null)
			{
				picture.mainTexture = www.texture;
			}
		});
	}

	public void SetLabel(string text)
	{
		//UILabel label = GetComponentInChildren<UILabel>();
		if (label == null)
		{
			label = GetComponentInChildren<UILabel>();
			if (label == null)
			{
				label = textLabel;
			}
			if (label == null)
			{
				Debug.LogWarning(gameObject.name + " doesn't have a label");
			}
		}

		if (label != null)
		{
			label.text = text;
		}
	}

	public string GetLabel()
	{
		if (label == null)
		{
			label = GetComponentInChildren<UILabel>();
			if (label == null)
			{
				label = textLabel;
			}
			if (label == null)
			{
				Debug.LogWarning(gameObject.name + " doesn't have a label");
			}
		}

		if (label != null)
		{
			return label.text;
		}
		return "";
	}

	public void SetUserObject(System.Object obj)
	{
		this.userObject = obj;
	}

	#endregion


	#region Internal Methods

	#endregion


	#region Public Methods

	public void OnClick()
	{
		if (OnClicked != null)
		{
			OnClicked(this);
		}
	}

	#endregion
}
