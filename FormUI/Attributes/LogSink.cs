using System;
using System.Runtime.Remoting.Messaging;
using FormUI.OperationLayer;

namespace FormUI.Attributes
{
    public class LogSink : IMessageSink
    {
        public LogSink(IMessageSink nextSink)
        {
            NextSink = nextSink;
        }

        public IMessage SyncProcessMessage(IMessage msg)
        {
            var call = msg as IMethodCallMessage;
            object[] attributes = call.MethodBase.GetCustomAttributes(false);
            Array.Exists(attributes, x => x.GetType() == typeof (OperationAttribute));
            BeforeProcess(call);
            IMessage retMsg = NextSink.SyncProcessMessage(msg);
            LogProcess(call);
            return retMsg;
        }

        public IMessageCtrl AsyncProcessMessage(IMessage msg, IMessageSink replySink)
        {
            return null;
        }

        public IMessageSink NextSink { get; private set; }

        private void BeforeProcess(IMethodCallMessage call)
        {
            object[] attributes = call.MethodBase.GetCustomAttributes(typeof (OperationAttribute), false);
            if (attributes.Length <= 0) return;
            Port.Instance.ReceiveEventEnabled = false;
        }

        private void LogProcess(IMethodCallMessage call)
        {
            object[] operation = call.MethodBase.GetCustomAttributes(typeof (OperationAttribute), false);
            if (operation.Length <= 0) return;
            Port.Instance.ReceiveEventEnabled = true;
        }
    }
}