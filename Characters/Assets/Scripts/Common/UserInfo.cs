using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class UserInfo : MonoBehaviour 
{
	#region MemVars & Props


	#endregion

	
	#region Mono Methods

	private void Start()
	{
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
		UILabel label = GetComponentInChildren<UILabel>();
		if (label != null)
		{
			label.text = text;
		}
	}

	#endregion


	#region Internal Methods

	#endregion


	#region Public Methods

	#endregion
}
