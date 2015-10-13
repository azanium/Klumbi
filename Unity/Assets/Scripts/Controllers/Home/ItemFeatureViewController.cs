using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ItemFeatureViewController : StyleViewController 
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

	#endregion
}
