using System;
using System.Collections.Generic;
using System.Text;

namespace BasicIoCTests.TestClasses
{
    public class BasicConstructor : IConstructor
    {
        public bool? Value { get; }

        public BasicConstructor(bool value)
        {
            this.Value = value;
        }
    }
}
