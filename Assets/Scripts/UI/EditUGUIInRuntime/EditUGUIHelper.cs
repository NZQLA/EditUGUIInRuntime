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
    /// 作为用户修改意图的触发器,并实现修改自身UI区域的方法
    /// <para>触发器：检测到用户行为后，调用对应的事件(把具体的响应行为绑定到这些事件上)</para>
    /// <para>使用了以下Unity内置的事件检测接口：</para>
    /// <list type="bullet">IPointerDownHandler 检测用户点击行为 这时候可以记录下点击数据</list>
    /// <list type="bullet">IDragHandler 检测用户拖拽行为 这时候可以根据拖拽数据计算出移动位置/尺寸该如何修改</list>
    /// <list type="bullet">IPointerUpHandler 检测用户松开鼠标的行为 停止相关响应动作</list>
    /// </summary>
    [ExecuteInEditMode]
    [RequireComponent(typeof(RectTransform))]
    [Serializable]
    public class EditUGUIHelper : MonoBehaviour, IEditUGUIHelper, IDragHandler, IPointerDownHandler, IPointerUpHandler
    {
        /// <summary>
        /// 触发器自身的UGUI　RectTransform
        /// </summary>
        public RectTransform self;

        /// <summary>
        /// 用户行为类型
        /// <para>初始化时机由外部控制，调用Init()实现</para>
        /// </summary>
        public EditUGUITriggerType EditType { get; set; }


        public HorizontalType HorizontalType { get; set; }

        public VerticalType VerticalType { get; set; }



        public event Action<EditUGUIHelper, PointerEventData> PointDownHandler;

        public event Action<EditUGUIHelper, PointerEventData> DragHandler;

        public event Action<EditUGUIHelper, PointerEventData> PointerUpHandler;

        private void OnEnable()
        {
            //省去了在编辑器指定的麻烦
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
        public void Init(
            EditUGUITriggerType EditType,
            HorizontalType horizontalType,
            VerticalType verticalType,
            Action<EditUGUIHelper, PointerEventData> pointerDown,
            Action<EditUGUIHelper, PointerEventData> onDrag,
            Action<EditUGUIHelper, PointerEventData> onPointUp
        )
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

            if (onPointUp != null)
            {
                PointerUpHandler += onPointUp;
            }
        }

        private void InitSelfRectTransform()
        {
            self = self != null ? self : transform as RectTransform;
        }


        /// <summary>
        /// 根据数据刷新自身的UI区域
        /// <para>根据自身相对于目标的方位计算出自身的位置</para>
        /// <para>根据触发器的宽度以及方位计算出自身的尺寸</para>
        /// </summary>
        /// <param name="target">目标UI</param>
        /// <param name="DragTriggerWidth">拖拽区域触发器的宽度</param>
        public void UpdateSelfRect(RectTransform target, float DragTriggerWidth)
        {
            if (self == null || target == null)
            {
                return;
            }

            //计算触发器中心点位置
            self.position = target.GenerateUGUIPosAtCenter(HorizontalType, VerticalType);

            //计算触发器rect 尺寸
            switch (EditType)
            {
                case EditUGUITriggerType.InsideForMoveTrigger:
                    //中心区域宽度/高度都 内收一个单位的 触发器宽度
                    self.sizeDelta = target.sizeDelta - Vector2.one * DragTriggerWidth;
                    break;

                case EditUGUITriggerType.EdgeForResizeHorTrigger:
                    //水平方向的触发器 = (目标的宽度-触发器宽度 , 触发器宽度)
                    self.sizeDelta = new Vector2(DragTriggerWidth, target.sizeDelta.y - DragTriggerWidth);
                    break;

                case EditUGUITriggerType.EdgeForResizeVerTrigger:
                    self.sizeDelta = new Vector2(target.sizeDelta.x - DragTriggerWidth, DragTriggerWidth);
                    break;

                case EditUGUITriggerType.CrossForResizeHorAndVerTrigger:
                    //四角触发器的尺寸 = (触发器宽度,触发器宽度)
                    self.sizeDelta = Vector2.one * DragTriggerWidth;
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
