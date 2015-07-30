using NLog.Targets.Lumberjack;
using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;
using System.Net;

namespace NLog.Targets.Lumberjack.Tests
{
    [TestClass]
    public class LumberjackTargetTests
    {
        class LumberjackTargetAccessor : LumberjackTarget
        {
            public static Task WriteAsyncLogEventTest()
            {
                ServicePointManager.ServerCertificateValidationCallback += (sender, certificate, chain, sslPolicyErrors) => { return true; };


                var tcs = new TaskCompletionSource<object>();

                var target = new LumberjackTargetAccessor()
                {
                    Host = "logs.zheka.by",
                    Port = 5000,
                    Fingerprint = "06E304EECEAEA7E3D0E8507B9012444503EF0053"
                };

                var evt = new Common.AsyncLogEventInfo(new LogEventInfo(LogLevel.Info, "Lumberjack", "WriteAsyncLogEventTest") { TimeStamp = DateTime.UtcNow }, e => { tcs.SetResult(null); });

                target.Write(evt);

                return tcs.Task;
            }
        
        }

        [TestMethod]
        public void WriteAsyncLogEventTest()
        {
            Task.Run(LumberjackTargetAccessor.WriteAsyncLogEventTest).GetAwaiter().GetResult();
        }
    }
}
