using System;
using BLL;
using FormUI.OperationLayer;
using TomorrowSoft.BLL;
using TomorrowSoft.Model;

namespace FormUI.Filters
{
    public class MessageFilter : Filter
    {
        public override string Phone { get; protected set; }
        public override DateTime Time { get; set; }
        public override string Context { get; set; }
        public override string Content1 { get; set; }
        public override bool IsQsDown { get; set; }
        
        public override string Name {
            get { return "收信"; }
        }
        private string phone;
        private DateTime time;
        private string content;
        private string text;
        public MessageFilter(Filter next) : base(next)
        {
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
                AT.GetSmsContent(Content[1],out isLongMessage, out phone, out time, out content,out current,out total,out identifier);
                if (phone.StartsWith("86"))
                    phone=phone.Remove(0, 2);
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

                        var longSmses = service.GetBy(phone, identifier, time);
                        if (longSmses.Count < total)
                            return null;
                        content = string.Empty;
                        foreach (var sms in longSmses)
                        {
                            content += sms.Content;
                        }
                       
                    }
                    IsQsDown = false;
                    if (content.Contains("喇叭"))
                    {
                        ConditionFilter conditionFilter = new ConditionFilter();
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