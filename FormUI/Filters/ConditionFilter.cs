using System;
using System.Data;
using System.Windows.Forms;
using FormUI.Properties;
using TomorrowSoft.BLL;
using TomorrowSoft.Model;

namespace FormUI.Filters
{
    public class ConditionFilter
    {
        private  string _photovoltaic;
        private  string _battery;
        public  string content;
        public  string[] Horn;

   /*     private static volatile ConditionFilter instance;
        private static object syncRoot = new object();
        public ConditionFilter (){}
        public static ConditionFilter Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (syncRoot)
                    {
                        if (instance == null)
                        {
                            instance = new ConditionFilter();
                        }
                    }
                }
                return instance;
            }
        }*/
        public  Condition FilterCondition(string phone, string context, string name)
        {
            try
            {
                string[] result = context.Split(new[]
                {
                    "信号强度","光伏", "\r\n", "\n", "\r\t", "电池",
                    "市电", "功放", "1喇叭", "2喇叭", "3喇叭", "4喇叭"
                },
                                          StringSplitOptions.RemoveEmptyEntries);
                if (result.Length == 6 && !context.Contains("信号强度"))
                {
                    Horn = result;
                    if (result.Length >= 2)
                    {
                        _battery = result[0].Replace("V", string.Empty);
                        _photovoltaic = result[1].Replace("V", string.Empty);
                    }
                    var condition = new Condition
                    {
                        PhoneNo = phone,
                        Battery = result[0],
                        Photovoltaic = result[1],
                        Name = name,
                        Horn1 = result[2],
                        Horn2 = result[3],
                        Horn3 = result[4],
                        Horn4 = result[5],
                        HandlerTime = DateTime.Now.ToLocalTime()
                    };
                    return condition;
                }
                if (result.Length == 7 && context.Contains("信号强度"))
                {
                    Horn = result;
                    if (result.Length >= 2)
                    {
                        _battery = result[1].Replace("V", string.Empty);
                        _photovoltaic = result[2].Replace("V", string.Empty);
                    }
                    var condition = new Condition
                    {
                        PhoneNo = phone,
                        Battery = result[1],
                        Photovoltaic = result[2],
                        Name = name,
                        Horn1 = result[3],
                        Horn2 = result[4],
                        Horn3 = result[5],
                        Horn4 = result[6],
                        HandlerTime = DateTime.Now.ToLocalTime()
                    };
                    return condition;
                }
            }
            catch (Exception e)
            {
                MessageBox.Show("接受到的数据参数与预设数据参数不一致。");
            }
          
           
            return null;
        }

        public  bool PhotovoltaicCompare(out string text)
        {
            text = string.Empty;
            DataTable qs = new QsService().GetAll();
            bool isHors = true;
            bool isPhotovoltaic = true;
            bool isBattery = true;
            if (_photovoltaic == null || _battery == null) throw new ArgumentException("_photovoltaic和_battery不能为空！");
            if (qs.Rows.Count > 0)
            {
                if (double.Parse(_photovoltaic) < Convert.ToDouble(Settings.Default.RDS))
                {
                    text = string.Format("告警：光伏值：{0}低于{1}\r\n", _photovoltaic, Settings.Default.RDS);
                    isPhotovoltaic = false;
                }
                if (double.Parse(_battery) < Convert.ToDouble(Settings.Default.Battery))
                {
                    text += string.Format("告警：电池值：{0}低于{1}\r\n", _battery, Settings.Default.Battery);
                    isBattery = false;
                }
                if (Horn.Length == 6)
                {
                    for (int a = 2; a <= 5; a++)
                    {
                        if (Horn[a].Contains("0.0"))
                        {
                            text += string.Format("告警：{0}喇叭异常\r\n", a - 1);
                            isHors = false;
                        }
                    }
                }
                if (Horn.Length == 7)
                {
                    for (int a = 3; a <= 6; a++)
                    {
                        if (Horn[a].Contains("0.0"))
                        {
                            text += string.Format("告警：{0}喇叭异常\r\n", a - 2);
                            isHors = false;
                        }
                    }
                }

                return (isPhotovoltaic && isBattery && isHors);
            }
            return false;
        }
    }
}