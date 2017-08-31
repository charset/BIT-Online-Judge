using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BITOJ.SiteUI.Models
{
    using BITOJ.Core;
    using BITOJ.Core.Data;
    using System;
    using System.Collections.Generic;

    public class ContestInfoModel
    {
        /// <summary>
        /// 获取或设置比赛ID。
        /// </summary>
        public string ContestId { get; set; }

        /// <summary>
        /// 获取或设置比赛标题。
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// 获取或设置比赛起始时间。
        /// </summary>
        public DateTime StartTime { get; set; }

        /// <summary>
        /// 获取或设置比赛终止时间
        /// </summary>
        public DateTime EndTime { get; set; }

        /// <summary>
        /// 计算比赛时长并以合适的时间单位生成时长描述字符串。
        /// </summary>
        /// <returns>一个字符串，代表比赛时长。</returns>
        public string GetDurationString()
        {
            int minutes = EndTime.Subtract(StartTime).Minutes;
            string durationString;
            if (minutes > 2880)
            {
                //时长两天以上时，以days为单位
                durationString = string.Format("{0:C1} day(s)", minutes / (double)1440);
            }
            else if (minutes >= 60)
            {
                durationString = string.Format("{0:C1} hour(s)", minutes / (double)60);
            }else
            {
                durationString = string.Format("{0} minute(s)", minutes);
            }
            return durationString;
        }

        /// <summary>
        /// 计算比赛的当前状态（Running/Passed/Scheduled）并以不同的颜色生成HTML代码。
        /// </summary>
        /// <returns>一段HTML代码，以不同的颜色显示不同的比赛状态。</returns>
        public string GetStatus()
        {
            string HTML;
            DateTime now = DateTime.Now;
            if(now > EndTime)
            {
                HTML = "<font color='grey'>Passed</font>";
            }else if(now >= StartTime)
            {
                HTML = "<font color='red'>Running</font>";
            }else
            {
                HTML = "<font color='green'>Scheduled</font>";
            }
            return HTML;
        }

        /// <summary>
        /// 获取或设置比赛的举办者
        /// </summary>
        public string Holder { get; set; }

    }
}