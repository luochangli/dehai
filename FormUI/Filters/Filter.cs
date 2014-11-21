using System;
using FormUI.OperationLayer;
using TomorrowSoft.BLL;

namespace FormUI.Filters
{
    public class Filter
    {
        protected readonly AT At = new AT();
        protected readonly string[] Content;
        protected readonly TerminalService Terminal = new TerminalService();
        protected readonly WhiteListService White = new WhiteListService();
        private readonly Filter _next;

        protected Filter(Filter next)
        {
            _next = next;
            Content = next.Content;
        }

        protected Filter(string[] content)
        {
            Content = content;
        }

        public virtual string Name { get; set; }
        public virtual string Phone { get; protected set; }
        public virtual string Context { get; set; }
        public virtual DateTime Time { get; set; }
        public virtual bool IsQsDown { get; set; }

        public virtual string Content1 { get; set; }


        public virtual Filter Run()
        {
            return _next.Run();
        }
    }
}