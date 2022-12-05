using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace BaseToolsForUnity.UI
{
	/// <summary>
	/// 在运行时直接编辑UGUI,比如
	/// <para>位置</para>
	/// <para>缩放</para>
	/// <para>自由的拖拽以修改其大小区域</para>
	/// </summary>
	[ExecuteInEditMode]
	[RequireComponent(typeof(RectTransform))]
	[Serializable]
	public class EditUGUIHelper : MonoBehaviour, IEditUGUIHelper, IDragHandler, IPointerDownHandler, IPointerUpHandler
	{
		[SerializeField]
		public RectTransform self;

		public EditUGUIHelperType EditType { get; set; }

		public HorizontalType HorizontalType { get; set; }

		public VerticalType VerticalType { get; set; }

		public event Action<EditUGUIHelper, PointerEventData> PointDownHandler;

		public event Action<EditUGUIHelper, PointerEventData> DragHandler;

		public event Action<EditUGUIHelper, PointerEventData> PointerUpHandler;



		private void OnEnable()
		{
			InitSelfRectTransform();
		}


		/// <summary>
		/// 初始化
		/// </summary>
		/// <param name="EditType">编辑类型</param>
		/// <param name="horizontalType">水平方向在目标中心点的哪一侧</param>
		/// <param name="verticalType">垂直方向在目标中心点的哪一侧</param>
		/// <param name="pointerDown">正在编辑的回调</param>
		/// <param name="onDrag">编辑完成的回调</param>
		public void Init(EditUGUIHelperType EditType, HorizontalType horizontalType, VerticalType verticalType, Action<EditUGUIHelper, PointerEventData> pointerDown, Action<EditUGUIHelper, PointerEventData> onDrag, Action<EditUGUIHelper, PointerEventData> onPointUp)
		{
			InitSelfRectTransform();

			this.EditType = EditType;
			this.HorizontalType = horizontalType;
			this.VerticalType = verticalType;
			if (pointerDown != null)
			{
				PointDownHandler += pointerDown;
			}

			if (onDrag != null)
			{
				DragHandler += onDrag;
			}

			if (onPointUp!=null)
			{
				PointerUpHandler += onPointUp;
			}
		}


		private void InitSelfRectTransform()
		{
			self = self != null ? self : transform as RectTransform;
		}

		public void UpdateSelfRect(RectTransform target, float allowance)
		{
			if (self == null || target == null)
			{
				return;
			}

			//计算中心点位置
			self.position = target.GenerateUGUIPosAtCenter(HorizontalType, VerticalType);

			//计算 rect 尺寸
			switch (EditType)
			{
				case EditUGUIHelperType.InsideForMoveTrigger:
					self.sizeDelta = target.sizeDelta - Vector2.one * allowance * 2;
					break;

				case EditUGUIHelperType.EdgeForResizeHorTrigger:
					self.sizeDelta = new Vector2(allowance * 2, target.sizeDelta.y - allowance * 2);
					break;

				case EditUGUIHelperType.EdgeForResizeVerTrigger:
					self.sizeDelta = new Vector2(target.sizeDelta.x - allowance * 2, allowance * 2);
					break;

				case EditUGUIHelperType.CrossForResizeHorAndVerTrigger:
					self.sizeDelta = Vector2.one * allowance * 2;
					break;
				default:
					break;
			}

		}


		public void OnDrag(PointerEventData eventData)
		{
			DragHandler?.Invoke(this, eventData);
			Log.LogAtUnityEditor($"{gameObject.name}:OnDrag");
		}

		public void OnPointerDown(PointerEventData eventData)
		{
			PointDownHandler?.Invoke(this, eventData);
			Log.LogAtUnityEditor($"{gameObject.name}:OnPointerDown");
		}

		public void OnPointerUp(PointerEventData eventData)
		{
			PointerUpHandler?.Invoke(this, eventData);
			Log.LogAtUnityEditor($"{gameObject.name}:OnPointerUp");
		}
	}





}
