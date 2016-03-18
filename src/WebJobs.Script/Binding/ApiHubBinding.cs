// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using System;
using System.IO;
using System.Linq;
using System.Reflection.Emit;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs.Host.Bindings.Path;
using Microsoft.Azure.WebJobs.Script.Description;

namespace Microsoft.Azure.WebJobs.Script.Binding
{
    public class ApiHubBinding : FunctionBinding
    {
        private readonly BindingTemplate _pathBindingTemplate;

        public ApiHubBinding(ScriptHostConfiguration config, string name, string path, FileAccess access, bool isTrigger) : base(config, name, BindingType.ApiHub, access, isTrigger)
        {
            if (string.IsNullOrEmpty(path))
            {
                throw new ArgumentException("The Apihub path cannot be null or empty.");
            }

            Path = path;
            _pathBindingTemplate = BindingTemplate.FromString(Path);
        }

        public override bool HasBindingParameters
        {
            get
            {
                return _pathBindingTemplate.ParameterNames.Any();
            }
        }

        public string Path { get; private set; }

        public override CustomAttributeBuilder GetCustomAttribute()
        {
            var constructorTypes = new Type[] { typeof(string), typeof(FileAccess) };
            var constructorArguments = new object[] { Path, Access };

            return new CustomAttributeBuilder(typeof(BlobAttribute).GetConstructor(constructorTypes), constructorArguments);
        }

        public override async Task BindAsync(BindingContext context)
        {
            await Task.Delay(10);
        }
    }
}
