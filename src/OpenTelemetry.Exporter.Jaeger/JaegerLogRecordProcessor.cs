// <copyright file="JaegerLogRecordProcessor.cs" company="OpenTelemetry Authors">
// Copyright The OpenTelemetry Authors
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
//     http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
// </copyright>

using System.Diagnostics;
using OpenTelemetry.Logs;

namespace OpenTelemetry.Exporter.Jaeger
{
    public class JaegerLogRecordProcessor : BaseProcessor<LogRecord>
    {
        private readonly JaegerLogRecordProcessorOptions options;

        public JaegerLogRecordProcessor(JaegerLogRecordProcessorOptions options)
        {
            this.options = options ?? new JaegerLogRecordProcessorOptions();
        }

        public override void OnEnd(LogRecord data)
        {
            if (Activity.Current != null)
            {
                var tags = new ActivityTagsCollection
                {
                    { nameof(data.CategoryName), data.CategoryName },
                    { nameof(data.EventId), data.EventId },
                    { nameof(data.Exception), data.Exception },
                    { nameof(data.LogLevel), data.LogLevel },
                    { nameof(data.SpanId), data.SpanId },
                    { nameof(data.State), data.State },
                    { nameof(data.Timestamp), data.Timestamp },
                    { nameof(data.TraceFlags), data.TraceFlags },
                    { nameof(data.TraceId), data.TraceId },
                    { nameof(data.TraceState), data.TraceState },
                };

                var activityEvent = new ActivityEvent(data.CategoryName, default, tags);
                Activity.Current.AddEvent(activityEvent);
            }
        }
    }
}
