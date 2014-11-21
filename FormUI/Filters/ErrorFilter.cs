using System;

namespace FormUI.Filters
{
    /// <summary>
    ///     错误筛选，放最后
    /// </summary>
    public class ErrorFilter : Filter
    {
        private const string name = "确定";

        public ErrorFilter(Filter next)
            : base(next)
        {
        }

        public override string Phone { get; protected set; }
        public override DateTime Time { get; set; }
        public override string Context { get; set; }
        public override string Name { get; set; }

        public override Filter Run()
        {
            return null;
        }
    }
}