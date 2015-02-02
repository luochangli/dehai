using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace FormUI.Camera
{
    public partial class MultiEventHelper:IDisposable
    {
        /// <summary>
        /// System.ComponentModel.EventHandlerList包含了一个委托链表的容器
        /// 实现了多事件存放在一个容器之中的包装
        /// EventHandlerList使用的是链表数据结构
        /// </summary>
        private readonly EventHandlerList _event;

        public MultiEventHelper()
        {
            _event = new EventHandlerList();
        }

        public void Dispose()
        {
            _event.Dispose();
        }
    }

    public partial class MultiEventHelper:IDisposable
    {
        /// <summary>
        /// 事件委托原型
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public delegate void ListeningEvent1();
        /// <summary>
        /// 静态对象
        /// </summary>
        protected static readonly Object EventKey1 = new object();
        /// <summary>
        /// 定义一组订阅、取消订阅事件的方法
        /// 注意EventHandlerList并不提供线程同步，所以在add和remove方法前加上线程同步属性
        /// 读者可以采取lock机制来代替
        /// </summary>
        public event ListeningEvent1 CameraDeviceEvnet
        {
            [MethodImpl(MethodImplOptions.Synchronized)] 
            add { _event.AddHandler(EventKey1, value); }

            [MethodImpl(MethodImplOptions.Synchronized)]
            remove { _event.RemoveHandler(EventKey1, value); }
        }

        /// <summary>
        /// 触发事件
        /// </summary>                          
        /// <param name="path"></param>
        protected virtual void OnCameraDeviceEvent()
        {
            _event[EventKey1].DynamicInvoke();
        }

    }
}