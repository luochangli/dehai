using System;

namespace FormUI.Filters
{
    public class MessageReplyFilter : Filter
    {
        private const string name = "确定";

        public MessageReplyFilter(Filter next)
            : base(next)
        {
        }

        public override string Phone { get; protected set; }
        public override DateTime Time { get; set; }
        public override string Context { get; set; }
        public override string Name { get; set; }

        public override Filter Run()
        {
            if (Content.Length == 2 && Content[0].Contains("+CMGS:") && Content[1] == "OK")
            {
                return this;
            }
            return base.Run();
        }
    }
}