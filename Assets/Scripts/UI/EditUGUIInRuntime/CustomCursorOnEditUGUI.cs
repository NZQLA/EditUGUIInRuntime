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
	[Serializable]
	public class CustomCursorOnEditUGUI : MonoBehaviour
	{
		public Texture2D textureHor;
		public Texture2D textureVer;
		public Texture2D textureCross;
		public Texture2D textureCrossInverse;
		public Texture2D textureMove;
		public Vector2 hotspot = Vector2.zero;


		/// <summary>
		/// 根据编辑类型来刷新鼠标样式
		/// </summary>
		/// <param name="editer"></param>
		public void ShowCursorAtType(EditUGUIHelper editer)
		{
			if (!editer)
			{
				ResetCursor();
				return;
			}

			Texture2D cursorTemp = null;
			switch (editer.EditType)
			{
				//复原鼠标样式
				case EditUGUITriggerType.NonEdit:
					cursorTemp = null;
					break;
				case EditUGUITriggerType.InsideForMoveTrigger:
					if (textureMove)
					{
						cursorTemp = textureMove;
					}
					break;
				case EditUGUITriggerType.EdgeForResizeHorTrigger:
					if (textureHor)
					{
						cursorTemp = textureHor;
					}
					break;
				case EditUGUITriggerType.EdgeForResizeVerTrigger:
					if (textureVer)
					{
						cursorTemp = textureVer;
					}
					break;
				case EditUGUITriggerType.CrossForResizeHorAndVerTrigger:
					//是左下角和右上角吗
					if (editer.HorizontalType == HorizontalType.Left && editer.VerticalType == VerticalType.Bottom || editer.HorizontalType == HorizontalType.Right && editer.VerticalType == VerticalType.Top)
					{
						if (textureCross)
						{
							cursorTemp = textureCross;
						}
					}
					else
					{
						cursorTemp = textureCrossInverse;
					}

					break;
				default:
					break;
			}

			//处理光标图片偏移量
			if (cursorTemp)
			{
				hotspot.x = cursorTemp.width * 0.5f;
				hotspot.y = cursorTemp.height * 0.5f;
			}
			else
			{
				hotspot = Vector2.zero;
			}


			Cursor.SetCursor(cursorTemp, hotspot, CursorMode.Auto);
		}


		public void ResetCursor()
		{
			Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
		}


	}




}
