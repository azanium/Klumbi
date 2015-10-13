using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class StyleViewController : UITableViewController<AvatarItems.AvatarItem> 
{
	#region MemVars & Props

	public UIWidgetContainer widgetContainer;
	public GameObject itemPrefab;

	public UILabel titleLabel;
	public UILabel descriptionLabel;

	#endregion


	#region Mono & UIViewController's Methods

	public override void OnAppear()
	{
		base.OnAppear();

		DressRoom.HideCharacter();

		UINavigationBar.ShowMenus(ShowMenus());
	}

	public override void OnAppeared()
	{
		base.OnAppeared();

		var func = GetParameter("f");
		Debug.LogWarning("Func: " + func);
		if (string.IsNullOrEmpty(func) == false)
		{
			var email = Konstants.GetSettingsProfile().GetString(Konstants.kdEmail);

			int start = 0;
			int limit = 0;

			Request(email, func, start, limit);
		}
	}

	public virtual bool ShowMenus()
	{
		return false;
	}

	#endregion


	#region Internal Methods

	protected void Request(string email, string func, int start, int limit)
	{
		Clear();

		string request = RequestUrl(email, func, start, limit);
		Debug.LogWarning("StyleViewController Request: " + request);

		NetworkController.DownloadFromUrl(request, OnDownloaded, null);
	}

	protected virtual string RequestUrl(string email, string func, int start, int limit)
	{
		return AvatarItems.GetStylesApiV2(email, func, "", "asc", start, limit);
	}

	protected void OnDownloaded(WWW www)
	{
		Debug.Log("StyleViewController received response: " + www.text);
		var items = AvatarItems.CreateObject(www.text);

		if (items != null)
		{
			ReloadData(items.data);
			ReloadMap(OnReloadMap);
			ReloadImage(OnReloadImage);

			RefreshTable();
		}

		StartCoroutine(UpdateContainer());
	}

	protected string OnReloadImage(AvatarItems.AvatarItem data)
	{
		return string.IsNullOrEmpty(data.picture) ? "" : AvatarItems.GetImagePath(data.picture);
	}

	protected GameObject OnReloadMap(AvatarItems.AvatarItem item)
	{
		GameObject obj = (GameObject)Instantiate(itemPrefab, Vector3.zero, Quaternion.identity);

		obj.transform.parent = this.widgetContainer.transform;
		obj.transform.localScale = Vector3.one;
		obj.transform.localPosition = new Vector3(0, 0, -1);

		UIButton button = obj.GetComponent<UIButton>();
		if (button != null)
		{
			button.defaultColor = Color.white;
		}

		UIButtonMessage actionButton = obj.GetComponent<UIButtonMessage>();
		if (actionButton != null)
		{
			actionButton.target = this.gameObject;
		}

		var label = obj.GetComponentInChildren<UILabel>();
		var texture = obj.GetComponent<UITexture>();

		if (texture != null)
		{
			texture.mainTexture = (Texture2D)Resources.Load("Banners/loading");
			texture.color = Color.white;
		}

		if (label != null)
		{
			label.text = item.title;
		}
		return obj;
	}

	public override void RenderData(AvatarItems.AvatarItem data, ImageData image)
	{
		base.RenderData(data, image);

		GameObject obj = GetGameObjectByData(data);
		if (obj != null)
		{
			var texture = obj.GetComponent<UITexture>();
			if (texture != null)
			{
				texture.mainTexture = image.texture;
				texture.color = Color.white;
			}
		}


		StartCoroutine(UpdateContainer());
	}

	protected IEnumerator UpdateContainer()
	{
		yield return new WaitForEndOfFrame();

		if (widgetContainer != null)
		{
			if (widgetContainer is UITable)
			{
				((UITable)widgetContainer).Reposition();
			}
			if (widgetContainer is UIGrid)
			{
				((UIGrid)widgetContainer).Reposition();
			}
		}
	}

	#endregion


	#region Events

	public virtual void OnClick(GameObject sender)
	{
		var data = this.GetDataByGameObject(sender);

		Debug.Log("Open Controller: " + data.action);

		UINavigationController.PushController(data.action, (c) => {

			c.controllerParameters.Add("picture", data.picture);
			if (c is StyleViewController)
			{
				((StyleViewController)c).SetLabel("Style", data.title);
			}
			if (c is AvatarPreviewController)
			{
				((AvatarPreviewController)c).SetLabel(this.descriptionLabel.text, data.title);
			}
		}, null);
	}

	#endregion


	#region Public Methods


	public void SetLabel(string title, string description)
	{
		if (titleLabel != null)
		{
			titleLabel.text = title;
		}
				
		if (descriptionLabel != null)
		{
			descriptionLabel.text = description;
		}
	}

	#endregion


}
