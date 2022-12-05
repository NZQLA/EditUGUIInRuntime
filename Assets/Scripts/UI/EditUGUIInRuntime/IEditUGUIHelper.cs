using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;


namespace BaseToolsForUnity.UI
{
	/// <summary>
	/// 接口：编辑UGUI辅助UI
	/// </summary>
	public interface IEditUGUIHelper
	{
		/// <summary>修改类型</summary>
		EditUGUIHelperType EditType { get; set; }

		/// <summary>水平方向处于目标的那一侧</summary>
		HorizontalType HorizontalType { get; set; }

		/// <summary>垂直方向处于目标的那一侧</summary>
		VerticalType VerticalType { get; set; }


		/// <summary>更新自己的UI覆盖区域</summary>
		/// <param name="target">目标</param>
		/// <param name="allowance">容差</param>
		void UpdateSelfRect(RectTransform target, float allowance);

		/// <summary>事件：点击时</summary>
		event Action<EditUGUIHelper, PointerEventData> PointDownHandler;

		/// <summary>事件：拖拽时</summary>
		event Action<EditUGUIHelper, PointerEventData> DragHandler;
	}



	/// <summary>
	/// 编辑UGUI辅助UI类型
	/// </summary>
	public enum EditUGUIHelperType
	{
		/// <summary>
		/// 不修改
		/// </summary>
		NonEdit,

		/// <summary>
		/// 内部用于移动
		/// </summary>
		InsideForMoveTrigger,

		/// <summary>
		/// 四周边界用于修改UI水平方向的尺寸
		/// </summary>
		EdgeForResizeHorTrigger,

		/// <summary>
		/// 四周边界用于修改UI垂直方向的尺寸
		/// </summary>
		EdgeForResizeVerTrigger,

		/// <summary>
		/// 交叉点用于同时修改UI水平和垂直方向的尺寸
		/// </summary>
		CrossForResizeHorAndVerTrigger,
	}
}
