using System.Collections;
using UnityEngine;

namespace BaseToolsForUnity.UI
{
	/// <summary>
	/// 接口：当拖拽UGUI时，显示其相对于父UI区域的信息，比如：
	/// <para>相对父UI四周边界的距离</para>
	/// </summary>
	public interface IShowBlankAreaInfoOnDragUGUI
	{
		/// <summary>
		/// 当拖拽UGUI时，显示其相对于父UI区域的信息
		/// </summary>
		void ShowBlankAreaInfoOnDragUGUI(RectTransform self, RectTransform parent);
	}
}
