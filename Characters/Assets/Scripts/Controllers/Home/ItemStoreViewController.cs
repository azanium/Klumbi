using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ItemStoreViewController : StoreViewController
{
	#region MemVars & Props

	#endregion


	#region Mono Methods

	#endregion


	#region Internal Methods

	protected override string RequestUrl (string func, int start, int limit)
	{
		var email = Konstants.GetSettingsProfile().GetString(Konstants.kdEmail);
		return Brands.GetBrandItemIndex(email, func, this.brand_id, start, limit);
	}

	#endregion


	#region Public Methods

	#endregion
}
