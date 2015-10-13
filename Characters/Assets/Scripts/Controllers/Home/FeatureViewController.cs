using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FeatureViewController : StyleViewController 
{
	#region MemVars & Props

	#endregion


	#region Mono Methods

	#endregion


	#region Internal Methods

	protected override string RequestUrl(string email, string func, int start, int limit)
	{
		return AvatarItems.GetFeaturesApiV2(email, func, start, limit);
	}

	#endregion


	#region Public Methods

	public override void OnClick(GameObject sender)
	{
		var data = this.GetDataByGameObject(sender);
		
		Debug.Log(data.action);
		
		UINavigationController.PushController(data.action, (c) => {
			
			c.controllerParameters.Add("picture", data.picture);
			if (c is StyleViewController)
			{
				((StyleViewController)c).SetLabel("Features", data.title);
			}
			if (c is AvatarPreviewController)
			{
				((AvatarPreviewController)c).SetLabel(this.descriptionLabel.text, data.title);
			}
		}, null);
	}

	#endregion
}
