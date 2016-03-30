// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs.Host.Bindings.Runtime;
using Microsoft.Azure.WebJobs.Script.Binding;
using Microsoft.Azure.WebJobs.Script.Description;
using WebJobs.Extensions.Splunk;

namespace Microsoft.Azure.WebJobs.Script
{
    internal class SplunkHttpEventCollectorBinding : FunctionBinding
    {
        private BindingDirection _bindingDirection;

        public SplunkHttpEventCollectorBinding(ScriptHostConfiguration config, SplunkHttpEventCollectorBindingMetadata metadata, FileAccess access) : 
            base(config, metadata, access)
        {
            Host = metadata.Host;
            Source = metadata.Source;
            SourceType = metadata.SourceType;
            Index = metadata.Index;

            _bindingDirection = metadata.Direction;
        }

        /// <summary>
        /// Host which will be assigned to each event
        /// </summary>
        public string Host { get; set; }

        /// <summary>
        /// Source which will be assigned to each event
        /// </summary>
        public string Source { get; set; }

        /// <summary>
        /// SourceType which will be assigned to each event
        /// </summary>
        public string SourceType { get; set; }

        /// <summary>
        /// Index where each event will be stored
        /// </summary>
        public string Index { get; set; }

        public override bool HasBindingParameters
        {
            get
            {
                return false;
            }
        }

        public override Collection<CustomAttributeBuilder> GetCustomAttributes()
        {
            var attributeType = typeof(SplunkHttpEventCollectorAttribute);
            var props = new[]
            {
                attributeType.GetProperty("Host"),
                attributeType.GetProperty("Source"),
                attributeType.GetProperty("SourceType"),
                attributeType.GetProperty("Index")
            };

            var propValues = new object[]
            {
                Host,
                Source,
                SourceType,
                Index
            };

            var constructor = attributeType.GetConstructor(System.Type.EmptyTypes);
            return new Collection<CustomAttributeBuilder>()
            {
                new CustomAttributeBuilder(constructor, new object[] { }, props, propValues)
            };
        }

        public override async Task BindAsync(BindingContext context)
        {
           // Only output bindings are supported.
            if (Access == FileAccess.Write && _bindingDirection == BindingDirection.Out)
            {
                var attribute = new SplunkHttpEventCollectorAttribute
                {
                    Host = Host,
                    Source = Source,
                    SourceType = SourceType,
                    Index = Index
                };

                var runtimeContext = new RuntimeBindingContext(attribute);
                var collector = await context.Binder.BindAsync<IAsyncCollector<string>>(runtimeContext);
                byte[] bytes;
                using (MemoryStream ms = new MemoryStream())
                {
                    context.Value.CopyTo(ms);
                    bytes = ms.ToArray();
                }
                var inputString = Encoding.UTF8.GetString(bytes);
                //Only supports valid JSON string
                await collector.AddAsync(inputString);
            }
        }
    }
}
