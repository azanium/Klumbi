using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class CoolstuffItem : MonoBehaviour 
{
	#region MemVars & Props

	public UITexture previewTextue;
	public UILabel detailLabel;

	private Action<CoolstuffItem> onClicked = null;

	#endregion


	#region Mono Methods

	#endregion


	#region Internal Methods

	#endregion


	#region Public Methods

	public void SetPreviewTexture(Texture2D texture)
	{
		if (previewTextue != null)
		{
			previewTextue.mainTexture = texture;
		}
	}

	public void SetDetailLabel(string label)
	{
		if (detailLabel != null)
		{
			detailLabel.text = label;
		}
	}

	public void SetOnClick(Action<CoolstuffItem> clicked)
	{
		onClicked = clicked;
	}

	public void OnClick()
	{
		if (this.onClicked != null)
		{
			Debug.LogWarning("=========>");
			//this.onClicked(this);
		}
	}

	#endregion
}
