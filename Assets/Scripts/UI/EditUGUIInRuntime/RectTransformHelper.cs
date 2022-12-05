using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

namespace BaseToolsForUnity.UI
{
	public static class RectTransformHelper
	{


		/// <summary>
		/// 根据目标设置自身的尺寸
		/// </summary>
		/// <param name="self">自身</param>
		/// <param name="target">目标</param>
		/// <param name="setHor">水平方向设置开关</param>
		/// <param name="setVer">垂直方向设置开关</param>
		public static void SetSelfSizeWithTarget(this RectTransform self, RectTransform target, bool setHor = true, bool setVer = true)
		{
			if (!self || !target)
			{
				return;
			}

			Vector2 size = self.sizeDelta;

			if (setHor)
			{
				size.x = target.rect.size.x;
			}

			if (setVer)
			{
				size.y = target.rect.size.y;
			}
			self.sizeDelta = size;
		}


		/// <summary>
		/// 根据相对与目标的位置类型设置自身的位置
		/// </summary>
		/// <param name="self"></param>
		/// <param name="target">目标</param>
		/// <param name="horizontalType">水平方向相对于目标的中心</param>
		/// <param name="verticalType">垂直方方向相对于目标的中心</param>
		public static void SetSelfPosToTargetAtCenterType(this RectTransform self, RectTransform target, HorizontalType horizontalType = HorizontalType.Center, VerticalType verticalType = VerticalType.Center)
		{
			if (!self || !target)
			{
				return;
			}
			self.position = GenerateUGUIPosAtCenter(target, horizontalType, verticalType);
		}

		/// <summary>
		/// UGUI设置自身的尺寸为目标尺寸
		/// </summary>
		/// <param name="self">自身</param>
		/// <param name="target">目标</param>
		public static void SetSelfSizeWithTarget(this RectTransform self, RectTransform target)
		{
			if (!self || !target)
			{
				return;
			}

			self.sizeDelta = target.sizeDelta;
		}


		/// <summary>
		/// 获取相对于目标的指定类型的相对位置
		/// </summary>
		/// <param name="target">目标</param>
		/// <param name="horizontalType">水平方向相对于目标的中心</param>
		/// <param name="verticalType">垂直方方向相对于目标的中心</param>
		public static Vector2 GenerateUGUIPosAtCenter(this RectTransform target, HorizontalType horizontalType = HorizontalType.Center, VerticalType verticalType = VerticalType.Center)
		{
			if (!target)
			{
				return Vector2.zero;
			}

			var rectTemp = target.rect;
			rectTemp.position = target.position;
			if (horizontalType != HorizontalType.Center || verticalType != VerticalType.Center)
			{
				rectTemp.x = target.transform.position.x + target.rect.width * (horizontalType - HorizontalType.Center) * 0.5f;
				rectTemp.y = target.transform.position.y + target.rect.height * (verticalType - VerticalType.Center) * 0.5f;
			}

			return rectTemp.position;
		}




	}
}
