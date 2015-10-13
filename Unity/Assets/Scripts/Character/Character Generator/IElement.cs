using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public interface IElement
{
	void Clear();
	bool IsLoaded { get; }
}