using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class HomeViewController : UITableViewController<Banners.Banner> 
{
	#region MemVars & Props

	public UITable tableView;
	public GameObject itemPrefab;

	#endregion


	#region Mono Methods

	public override void OnAppear ()
	{
		base.OnAppear ();

		DressRoom.HideCharacter();

		NetworkController.DownloadFromUrl(Banners.GetBannersRequest(0, 0), this.OnBannerListDownloaded);
		
		//StartCoroutine(UpdateTable());
	}

	#endregion


	#region Internal Methods

	private void OnBannerListDownloaded(WWW www)
	{
		Clear();

		Debug.Log("Banners downloaded: " + www.text);
		Banners banners = Banners.CreateObject(www.text);
		Debug.Log("Test.....");
		if (banners != null)
		{
			Debug.Log("wow==========");
			ReloadData(banners.data);
			ReloadMap(OnReloadMap);
			ReloadImage(OnReloadImage);

			RefreshTable();
		}
		else
		{
			Debug.LogError("Banners creation is failed, maybe due to invalid json: " + www.text);
		}

		StartCoroutine(UpdateTable());
	}
	
	private GameObject OnReloadMap(Banners.Banner banner)
	{
		GameObject obj = (GameObject)Instantiate(itemPrefab, Vector3.zero, Quaternion.identity);
		
		obj.transform.parent = this.tableView.transform;
		obj.transform.localScale = Vector3.one;
		obj.transform.localPosition = new Vector3(0, 0, -1);

		CoolstuffItem item = obj.GetComponent<CoolstuffItem>();
		//item.SetOnClick(OnBannerClicked);
		item.SetDetailLabel(banner.name);
		item.SetPreviewTexture((Texture2D)Resources.Load("Banners/loading"));

		var button = obj.GetComponent<UIButton>();
		button.onClick.Clear();
		button.onClick.Add(new EventDelegate(()=>{
			OnBannerClicked(item);
		}));

		Debug.Log(banner.name);

		return obj;
	}
	
	public string OnReloadImage(Banners.Banner data)
	{
		string path = string.IsNullOrEmpty(data.picture) ? "" : Banners.GetBannersImagePath(data.picture);
		Debug.LogWarning("Image : " + path);
		return path;
	}
	
	public override void RenderData(Banners.Banner data, ImageData image)
	{
		base.RenderData(data, image);
		
		GameObject obj = GetGameObjectByData(data);
		if (obj != null)
		{
			var item = obj.GetComponent<CoolstuffItem>();
			item.SetPreviewTexture(image.texture);;
		}
		StartCoroutine(UpdateTable());
	}
	
	private IEnumerator UpdateTable()
	{
		yield return new WaitForEndOfFrame();
		
		if (tableView != null)
		{
			tableView.Reposition();
		}		

		var parent = NGUITools.FindInParents<UIScrollView>(tableView.gameObject);
		if (parent != null)
		{
			parent.ResetPosition();
		}
	}

	private void OnBannerClicked(CoolstuffItem item)
	{
		Debug.Log("Clicked banner");
		var banner = this.GetDataByGameObject(item.gameObject);
		if (banner == null)
		{
			return;
		}
		
		Debug.LogWarning(banner.type);
		
		switch (banner.type)
		{
		case "url":
			Application.OpenURL(banner.dataValue);
			break;
		case "mix":
			UINavigationController.PushController(string.Format("/AvatarPreview?f=character&v={0}&id={1}", banner.dataValue, banner._id));
			break;
		}

	}


	#endregion


	#region Public Methods


	#endregion
}
