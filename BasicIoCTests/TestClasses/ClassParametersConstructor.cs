using System;
using System.Collections.Generic;
using System.Text;

namespace BasicIoCTests.TestClasses
{
    public class ClassParametersConstructor
    {
        public BasicConstructor BasicConstructor { get; }
        public BasicNoParameterConstructor BasicNoParameterConstructor { get; }

        public ClassParametersConstructor(BasicConstructor basicConstructor,
            BasicNoParameterConstructor noParameterConstructor)
        {
            BasicConstructor = basicConstructor;
            BasicNoParameterConstructor = noParameterConstructor;
        }
    }
}
