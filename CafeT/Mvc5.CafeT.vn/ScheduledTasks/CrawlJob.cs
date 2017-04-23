using CafeT.SmartCrawler.Models;
using Quartz;
using System;

namespace Mvc5.CafeT.vn.ScheduledTasks
{
    public class CrawlJob : IJob
    {
        public void Execute(IJobExecutionContext context)
        {
            string _crawlerInput = "/App_Data/Crawlers/Input/";
            string _crawlerOutput = "/App_Data/Crawlers/Output/";
            string _appPath = AppDomain.CurrentDomain.BaseDirectory;

            string _fullInput = _appPath + _crawlerInput; 
            string _fullOutput = _appPath + _crawlerOutput; 

            CrawlerManager _crawlerManager = new CrawlerManager(_fullInput, _fullOutput);
            _crawlerManager.Run();
        }
    }
}