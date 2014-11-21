using System;
using System.Windows.Forms;
using FormUI.Filters;
using TomorrowSoft.BLL;
using TomorrowSoft.Model;

namespace FormUI.OperationLayer
{
    public class RecMesSave
    {
        private readonly ConditionService condition = new ConditionService();

        public void SaveMes(string content, string phone, string name)
        {
            if (content.Contains("喇叭"))
            {
                ConditionFilter conditionFilter = new ConditionFilter();
                try
                {
                    condition.Add(conditionFilter.FilterCondition(phone, content, name));
                }
                catch (Exception e)
                {
                    MessageBox.Show("接受到的数据参数与预设数据参数不一致。");
                }
              
            }
            else
            {
                new HistoryRecordService().Add(new HistoryRecord
                    {
                        Name = name,
                        Handler = "收信",
                        PhoneNo = phone,
                        HandlerTime = DateTime.Now.ToLocalTime(),
                        Context = content,
                    });
            }
        }
    }
}