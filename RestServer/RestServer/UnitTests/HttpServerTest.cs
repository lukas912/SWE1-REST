using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;

namespace RestServer.UnitTests
{
    [TestFixture]
    class HttpServerTest
    {
        [Test]
        public void startServerTest()
        {
            HttpServer server = new HttpServer(7000);
            try
            {
                server.StartServer();
                return; // indicates success
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.Message);
            }

        }
    }
}
