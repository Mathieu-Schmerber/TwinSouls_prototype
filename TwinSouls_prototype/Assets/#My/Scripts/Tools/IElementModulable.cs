using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using TwinSouls.Data;

namespace TwinSouls.Tools
{
    public interface IElementModulable
    {
		void UpdateElement(ElementData.ElementType elementType);
	}
}