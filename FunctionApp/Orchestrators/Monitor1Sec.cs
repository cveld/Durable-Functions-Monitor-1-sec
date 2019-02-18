using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace FunctionApp.Orchestrators
{  

    public class Monitor1Sec
    {       
        [FunctionName("MonitorTest")]
        public static async Task<string> MonitorTest([OrchestrationTrigger] DurableOrchestrationContext monitorContext, ILogger log)
        {
            JObject input = monitorContext.GetInput<JObject>();
            if (!monitorContext.IsReplaying) { log.LogInformation($"Received monitor request."); }

            DateTime endTime = monitorContext.CurrentUtcDateTime.AddSeconds(10);
            if (!monitorContext.IsReplaying) { log.LogInformation($"Instantiating monitor. Expires: {endTime}."); }

            int i = 0;
            StringBuilder sb = new StringBuilder();
            while (monitorContext.CurrentUtcDateTime < endTime)
            {
                // Wait for the next checkpoint
                var nextCheckpoint = monitorContext.CurrentUtcDateTime.AddMilliseconds((int)input["duration"]);
                if (!monitorContext.IsReplaying) {
                    log.LogInformation($"Next check at {nextCheckpoint}.");                    
                }
                await monitorContext.CreateTimer(nextCheckpoint, CancellationToken.None);
                i++;
            }

            log.LogInformation("Monitor expiring.");
            return i.ToString();
        }
    }
}
