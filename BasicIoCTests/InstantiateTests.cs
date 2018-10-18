using System.ComponentModel;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BasicIoCTests
{
    using BasicIoC;
    using BasicIoCTests.TestClasses;

    [TestClass]
    public class InstantiateTests
    {
        private Container _container;

        [TestInitialize]
        public void StartUp()
        {
            _container = Container.Instance;
        }

        [TestMethod]
        public void CanInstantiateContainer()
        {
            Assert.IsNotNull(_container);
        }

        [TestMethod]
        public void CanInstantiateBasicNoParameterContainer()
        {
            Assert.IsNotNull(_container.GetService<BasicNoParameterConstructor>().Value);
        }

        [TestMethod]
        public void CanInstantiateBasicContainer()
        {
            _container.Register(new BasicConstructor(true));
            var result = _container.GetService<BasicConstructor>();
            Assert.IsNotNull(result.Value);
        }

        [TestMethod]
        public void CanInstantiateClassParameterConstructor()
        {
            _container.Register(new BasicConstructor(true));
            _container.Register(new BasicNoParameterConstructor());

            var result = _container.GetService<ClassParametersConstructor>();
            var result2 = result;

            Assert.IsNotNull(result?.BasicConstructor?.Value);
            Assert.IsNotNull(result?.BasicNoParameterConstructor?.Value);
        }
    }
}
