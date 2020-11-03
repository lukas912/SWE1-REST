using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;

namespace RestServer.UnitTests
{
    [TestFixture]
    class RequestTest
    {
        [Test]
        public void getQueriesTest()
        {
            Assert.IsNotNull(Request.GetQueries("localhost:7000/messages?name=lukas"));
        }

    }
}
