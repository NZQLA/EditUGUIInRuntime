using System.Collections;
using UnityEngine;

namespace BaseToolsForUnity.UI
{
	/// <summary>
	/// 将UI限定于指定的父物体区域内部，不允许出界
	/// </summary>
	public interface ILimitUGUIInParentArea
	{

		/// <summary>
		/// 将UI限定于指定的父物体区域内部，不允许出界
		/// </summary>
		void LimitUGUIInParentArea(RectTransform self, RectTransform parent);
	}
}
