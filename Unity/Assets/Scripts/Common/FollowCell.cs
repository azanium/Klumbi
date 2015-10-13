using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FollowCell : MonoBehaviour 
{
	#region MemVars & Props
	
	public UILabel textLabel;
	public UISprite positiveIcon;
	public UISprite negativeIcon;

	public System.Action<string> OnClicked;

	public string userid;
	
	#endregion
	
	
	#region Mono Methods

	public void EnableIcon(bool state)
	{
		positiveIcon.gameObject.SetActive(state);
		negativeIcon.gameObject.SetActive(!state);
	}

	private void Start()
	{
		EnableIcon(false);
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
		if (textLabel != null)
		{
			textLabel.text = text;
		}
	}


	
	#endregion
	
	
	#region Internal Methods
	
	#endregion
	
	
	#region Public Methods

	public void OnClickFollow()
	{
		if (OnClicked != null)
		{
			OnClicked(this.userid);
		}
	}
	
	#endregion
}
