// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.Azure.WebJobs.Script.Description
{
    internal class SplunkHttpEventCollectorBindingMetadata : BindingMetadata
    {
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
    }
}
