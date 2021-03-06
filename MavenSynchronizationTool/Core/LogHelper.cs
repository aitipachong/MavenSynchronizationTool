﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MavenSynchronizationTool.Core
{
    public class LogHelper
    {
        public static readonly log4net.ILog loginfo = log4net.LogManager.GetLogger("loginfo");
        public static readonly log4net.ILog logerror = log4net.LogManager.GetLogger("logerror");

        public static void WriteLog(string info)
        {
            if(loginfo.IsInfoEnabled)
            {
                loginfo.Info(info);
            }
        }

        public static void WriteLog(string info, Exception ex)
        {
            if(logerror.IsErrorEnabled)
            {
                logerror.Error(info, ex);
            }
        }

        public static void WriteLogAndExitApplication(string info)
        {
            if (logerror.IsErrorEnabled)
            {
                logerror.Error(info);
            }
            MessageBox.Show(info, "异常", MessageBoxButtons.OK, MessageBoxIcon.Error);
            Application.Exit();
        }
    }
}