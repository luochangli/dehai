using System;
using System.Collections.Generic;
using TomorrowSoft.DAL;
using TomorrowSoft.Model;

namespace BLL
{
    /// <summary>
    ///     WhiteListService
    /// </summary>
    public class LongSmsService
    {
        private readonly LongSmsRepository dal = new LongSmsRepository();

        #region  Method

        /// <summary>
        ///     增加一条数据
        /// </summary>
        public bool Add(LongSms model)
        {
            return dal.Add(model);
        }

        #endregion  Method

        public IList<LongSms> GetBy(string phone, string identifier, DateTime time)
        {
            return dal.GetBy(phone, identifier, time);
        }
    }
}