using System;
using System.Collections.Generic;
using BLL;
using FormUI.OperationLayer;
using TomorrowSoft.Model;

namespace FormUI.Filters
{
    public class MessageFilter : Filter
    {
        private string content;
        private string phone;
        private string text;
        private DateTime time;

        public MessageFilter(Filter next) : base(next)
        {
        }

        public override string Phone { get; protected set; }
        public override DateTime Time { get; set; }
        public override string Context { get; set; }
        public override string Content1 { get; set; }
        public override bool IsQsDown { get; set; }

        public override string Name
        {
            get { return "收信"; }
        }

        public override Filter Run()
        {
            if (Content.Length >= 2 && Content[0].Contains("+CMT:"))
            {
//                new AT().SmsAnswer();
                bool isLongMessage;
                int current;
                int total;
                string identifier;
                AT.GetSmsContent(Content[1], out isLongMessage, out phone, out time, out content, out current, out total,
                                 out identifier);
                if (phone.StartsWith("86"))
                    phone = phone.Remove(0, 2);
                if (White.PhoneExists(phone) || Terminal.PhoneExists(phone))
                {
                    if (isLongMessage)
                    {
                        var service = new LongSmsService();
                        service.Add(new LongSms
                            {
                                Content = content,
                                Current = current,
                                Identifier = identifier,
                                Phone = phone,
                                Time = time,
                                Total = total
                            });

                        IList<LongSms> longSmses = service.GetBy(phone, identifier, time);
                        if (longSmses.Count < total)
                            return null;
                        content = string.Empty;
                        foreach (LongSms sms in longSmses)
                        {
                            content += sms.Content;
                        }
                    }
                    IsQsDown = false;
                    if (content.Contains("喇叭"))
                    {
                        var conditionFilter = new ConditionFilter();
                        conditionFilter.FilterCondition(phone, content, Name);
                        IsQsDown = !conditionFilter.PhotovoltaicCompare(out text);
                    }
                    Phone = phone;
                    Time = time;
                    Context = content;
                    Content1 = content;
                    if (IsQsDown)
                    {
                        Context = text + content;
                    }
                    return this;
                }
                return null;
            }
            return base.Run();
        }
    }
}