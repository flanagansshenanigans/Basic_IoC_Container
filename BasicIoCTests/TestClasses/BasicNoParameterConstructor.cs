using System;
using System.Collections.Generic;
using System.Text;

namespace BasicIoCTests.TestClasses
{
    public class BasicNoParameterConstructor : IConstructor
    {
        public bool? Value { get; }

        public BasicNoParameterConstructor()
        {
            this.Value = true;
        }
    }
}
