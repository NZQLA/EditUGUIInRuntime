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
    public class EditUGUIInRuntime : MonoBehaviour, ILimitUGUIInParentArea, IShowBlankAreaInfoOnDragUGUI
    {

        [Tooltip("用于自定义光标样式的脚本")]
        [SerializeField]
        private CustomCursorOnEditUGUI CustomCursorOnEditUGUI;

        [Tooltip("拖拽区域触发器的宽度")]
        [SerializeField]
        private float DragTriggerWidth = 10;

        [Tooltip("指定你要被修改的UI父物体")]
        [SerializeField]
        private RectTransform self;

        /// <summary>
        /// 将作为被修改UI的临时父物体
        /// <para>被修改的UI将被限定与这个UI父物体的内部</para>
        /// </summary>
        [Tooltip(" 将作为被修改UI的临时父物体")]
        [SerializeField]
        private RectTransform parent;

        #region 触发器脚本
        [SerializeField]
        private EditUGUIHelper UIShowInside;

        [SerializeField]
        private EditUGUIHelper UIShowEdgeLeft;

        [SerializeField]
        private EditUGUIHelper UIShowEdgeRight;

        [SerializeField]
        private EditUGUIHelper UIShowEdgeTop;

        [SerializeField]
        private EditUGUIHelper UIShowEdgeBottom;

        [SerializeField]
        private EditUGUIHelper UIShowCrossLT;

        [SerializeField]
        private EditUGUIHelper UIShowCrossRT;

        [SerializeField]
        private EditUGUIHelper UIShowCrossLB;

        [SerializeField]
        private EditUGUIHelper UIShowCrossRB;

        #endregion


        /// <summary>
        /// 更新触发器自身UI区域的位置/尺寸的事件
        /// <para>触发时机：修改目标的位置/尺寸之后</para>
        /// </summary>
        public event Action<RectTransform, float> UpdateEditHelperRectAreaHandler;

        [SerializeField]
        private EditUGUITriggerType editType = EditUGUITriggerType.NonEdit;

        [ContextMenu("Init 初始化")]
        public void Init()
        {
            GenerateSelf();

            #region 初始化触发器 省得在Unity编辑器中手动去一个一个配置
            if (UIShowInside)
            {
                UIShowInside.Init(EditUGUITriggerType.InsideForMoveTrigger, HorizontalType.Center, VerticalType.Center, OnPointerDown, OnDrag, OnPointerUp);
                UpdateEditHelperRectAreaHandler += UIShowInside.UpdateSelfRect;
            }

            if (UIShowEdgeLeft)
            {
                UIShowEdgeLeft.Init(EditUGUITriggerType.EdgeForResizeHorTrigger, HorizontalType.Left, VerticalType.Center, OnPointerDown, OnDrag, OnPointerUp);
                UpdateEditHelperRectAreaHandler += UIShowEdgeLeft.UpdateSelfRect;
            }

            if (UIShowEdgeRight)
            {
                UIShowEdgeRight.Init(EditUGUITriggerType.EdgeForResizeHorTrigger, HorizontalType.Right, VerticalType.Center, OnPointerDown, OnDrag, OnPointerUp);
                UpdateEditHelperRectAreaHandler += UIShowEdgeRight.UpdateSelfRect;
            }

            if (UIShowEdgeTop)
            {
                UIShowEdgeTop.Init(EditUGUITriggerType.EdgeForResizeVerTrigger, HorizontalType.Center, VerticalType.Top, OnPointerDown, OnDrag, OnPointerUp);
                UpdateEditHelperRectAreaHandler += UIShowEdgeTop.UpdateSelfRect;
            }

            if (UIShowEdgeBottom)
            {
                UIShowEdgeBottom.Init(EditUGUITriggerType.EdgeForResizeVerTrigger, HorizontalType.Center, VerticalType.Bottom, OnPointerDown, OnDrag, OnPointerUp);
                UpdateEditHelperRectAreaHandler += UIShowEdgeBottom.UpdateSelfRect;
            }

            if (UIShowCrossLB)
            {
                UIShowCrossLB.Init(EditUGUITriggerType.CrossForResizeHorAndVerTrigger, HorizontalType.Left, VerticalType.Bottom, OnPointerDown, OnDrag, OnPointerUp);
                UpdateEditHelperRectAreaHandler += UIShowCrossLB.UpdateSelfRect;
            }

            if (UIShowCrossLT)
            {
                UIShowCrossLT.Init(EditUGUITriggerType.CrossForResizeHorAndVerTrigger, HorizontalType.Left, VerticalType.Top, OnPointerDown, OnDrag, OnPointerUp);
                UpdateEditHelperRectAreaHandler += UIShowCrossLT.UpdateSelfRect;
            }

            if (UIShowCrossRB)
            {
                UIShowCrossRB.Init(EditUGUITriggerType.CrossForResizeHorAndVerTrigger, HorizontalType.Right, VerticalType.Bottom, OnPointerDown, OnDrag, OnPointerUp);
                UpdateEditHelperRectAreaHandler += UIShowCrossRB.UpdateSelfRect;
            }

            if (UIShowCrossRT)
            {
                UIShowCrossRT.Init(EditUGUITriggerType.CrossForResizeHorAndVerTrigger, HorizontalType.Right, VerticalType.Top, OnPointerDown, OnDrag, OnPointerUp);
                UpdateEditHelperRectAreaHandler += UIShowCrossRT.UpdateSelfRect;
            }

            #endregion

            UpdateEditHelperRectAreaHandler?.Invoke(self, DragTriggerWidth);

        }

        private void GenerateSelf()
        {
            self = self != null ? self : transform as RectTransform;
        }


        private void Start()
        {
            Init();
        }

        #region 将UI限定于指定的父物体区域内部，不允许出界

        public void LimitUGUIInParentArea(RectTransform self, RectTransform parent)
        {
            if (!self || !parent)
            {
                return;
            }
        }


        #endregion



        #region 当拖拽UGUI时，显示其相对于父UI区域的信息
        //[SerializeField]
        //private UIShowUGUIRectAreaInfo areaInfo;



        public void ShowBlankAreaInfoOnDragUGUI(RectTransform self, RectTransform parent)
        {
            if (!self || !parent)
            {
                return;
            }

            //areaInfo.ShowRectArea();
        }
        #endregion



        #region 各种UI事件的响应机制
        /// <summary>临时记录点击点与UI的相对位置</summary>
        [SerializeField]
        private Vector2 offsetV2;

        [SerializeField]
        private Vector3 offsetV3;

        [SerializeField]
        private Vector2 posPointLast;

        [SerializeField]
        private Vector2 flag = Vector2.one;


        //响应拖拽事件
        public void OnDrag(EditUGUIHelper editer, PointerEventData eventData)
        {
            Log.LogAtUnityEditor($"{gameObject.name}:OnDrag");
            if (!self || !editer || eventData == null)
            {
                return;
            }

            //刷新鼠标样式
            CustomCursorOnEditUGUI?.ShowCursorAtType(editer);


            #region 修改目标UI的尺寸/位置  完成后执行修改触发器UI机制
            editType = editer.EditType;
            offsetV2 = eventData.position - posPointLast;
            offsetV3 = offsetV2;
            flag.x = (int)editer.HorizontalType;
            flag.y = (int)editer.VerticalType;
            switch (editType)
            {
                case EditUGUITriggerType.NonEdit:
                    break;
                //移动UI
                case EditUGUITriggerType.InsideForMoveTrigger:
                    self.position += offsetV3;
                    break;


                // 修改UI水平方向的尺寸
                case EditUGUITriggerType.EdgeForResizeHorTrigger:
                    self.sizeDelta += offsetV2 * flag;

                    //排除垂直方向的修改
                    offsetV3.y = 0;
                    //根据尺寸变化移动UI位置，以确保视觉上只有拖拽的那一侧大小改变
                    self.position += offsetV3 * 0.5f;
                    break;

                case EditUGUITriggerType.EdgeForResizeVerTrigger:
                    self.sizeDelta += offsetV2 * flag;

                    //排除水平方向的修改
                    offsetV3.x = 0;
                    //根据尺寸变化移动UI位置，以确保视觉上只有拖拽的那一侧大小改变
                    self.position += offsetV3 * 0.5f;
                    break;

                // 同时修改UI水平和垂直方向的尺寸
                case EditUGUITriggerType.CrossForResizeHorAndVerTrigger:
                    self.sizeDelta += offsetV2 * flag;

                    //根据尺寸变化移动UI位置，以确保视觉上只有拖拽的那一侧大小改变
                    self.position += offsetV3 * 0.5f;
                    break;
                default:
                    break;
            }

            #endregion

            //统一修改所有触发器UI的位置/尺寸
            UpdateEditHelperRectAreaHandler?.Invoke(self, DragTriggerWidth);

            //缓存当前拖拽数据
            posPointLast = eventData.position;
        }

        //响应按下事件
        public void OnPointerDown(EditUGUIHelper editer, PointerEventData eventData)
        {
            Log.LogAtUnityEditor($"{gameObject.name}:OnPointerDown");
            if (!self || !editer || eventData == null)
            {
                return;
            }
            // 记录下鼠标点击位置
            posPointLast = eventData.position;
        }

        //响应抬起事件
        public void OnPointerUp(EditUGUIHelper editer, PointerEventData eventData)
        {
            //复原鼠标样式
            CustomCursorOnEditUGUI?.ResetCursor();
        }
        #endregion



    }




    public enum VerticalType
    {
        Top = 1,
        Center = 0,
        Bottom = -1,
    }

    public enum HorizontalType
    {
        Left = -1,
        Center = 0,
        Right = 1,
    }

}
