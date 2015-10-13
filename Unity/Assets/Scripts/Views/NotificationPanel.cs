using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class NotificationPanel : UIPopupViewController 
{
	#region MemVars & Props
	
	public UILabel label;

	#endregion


	#region Mono Methods

	#endregion


	#region Internal Methods

	#endregion


	#region Public Methods

	public override void OnAppear ()
	{
		base.OnAppear ();

		string notification = GetParameter("notification");
		if (notification != "")
		{
			SetDescription(notification);
		}
	}

	public void SetDescription(string desc)
	{
		if (label != null)
		{
			label.text = desc;
		}
	}

	#endregion
}
