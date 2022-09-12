using Microsoft.Extensions.Logging;
using Quartz;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace UrlGenerator.Jobs
{
    [DisallowConcurrentExecution]
    public class RequestUrlGeneratingJob : IJob
    {
        private static int mutualFundIndex = 0;
        private const int TotalMutualFunds = 50;
        private const int TotalConcuurentTasks = 10;
        private readonly ILogger<HelloWorldJob> _logger;
        private readonly DateTime _startDate;
        private readonly DateTime _endDate;
        private readonly string _baseUrl = "https://portal.amfiindia.com/DownloadNAVHistoryReport_Po.aspx?mf={0}&tp={1}&frmdt={2}&todt={3}";
        
        private readonly IDictionary _monthIntToStringConverter = new Dictionary<string,string>()
        {
            {"01", "Jan" },
            {"02", "Feb" },
            {"03", "Mar" },
            {"04", "Apr" },
            {"05", "May" },
            {"06", "Jun" },
            {"07", "Jul" },
            {"08", "Aug" },
            {"09", "Sep" },
            {"10", "Oct" },
            {"11", "Nov" },
            {"12", "Dec" }
        };
        public RequestUrlGeneratingJob(ILogger<HelloWorldJob> logger)
        {
            _logger = logger;
            _startDate = DateTime.Today.AddDays(-1);
            _endDate = DateTime.Today.AddDays(-1);
        }

        public Task Execute(IJobExecutionContext context)
        {
            _logger.LogInformation("Hello from url job!");

            List<Task> tasks = new List<Task>();
            for (int i = 0; i <= TotalConcuurentTasks; i++)
            {
                tasks.Add(DoWorkAsync());
            }

            return Task.WhenAll(tasks);
        }

        private Task DoWorkAsync()
        {
            return Task.Run(async () => {
                    while (true)
                    {
                        var x = Interlocked.Increment(ref mutualFundIndex);
                        if (x > TotalMutualFunds)
                        {
                            break;
                        }
                        await Task.Delay(TimeSpan.FromSeconds(2));
                        var stdate = _startDate.Date.ToShortDateString();
                        var temp = stdate.Split("-");
                        stdate = temp[0] + "-" + _monthIntToStringConverter[temp[1]] + "-" + temp[2];
                        var enddate = _endDate.Date.ToShortDateString();
                        temp = enddate.Split("-");
                        enddate = temp[0] + "-" + _monthIntToStringConverter[temp[1]] + "-" + temp[2];
                        _logger.LogInformation(String.Format(this._baseUrl,x,1,stdate,enddate));
                    }
                    return Task.CompletedTask;
                });
        }
    }
}
