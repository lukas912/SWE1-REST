using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;

namespace RestServer.UnitTests
{
    [TestFixture]
    class RequestContextTest
    {
        //List All Messages
        [Test]
        public void checkContextTest1()
        {
            RequestContext rq = new RequestContext();
            List<Attribute> queries = new List<Attribute>();
            List<Attribute> body_data = new List<Attribute>();
            try
            {
                rq.CheckContext("localhost:7000/messages/", "GET", queries, body_data);
                return; // indicates success
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.Message);
            }

        }

        //List Message
        [Test]
        public void checkContextTest2()
        {
            RequestContext rq = new RequestContext();
            List<Attribute> queries = new List<Attribute>();
            List<Attribute> body_data = new List<Attribute>();
            try
            {
                rq.CheckContext("localhost:7000/messages/", "GET", queries, body_data);
                return; // indicates success
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.Message);
            }

        }

        //Add Message
        [Test]
        public void checkContextTest3()
        {
            RequestContext rq = new RequestContext();
            List<Attribute> queries = new List<Attribute>();
            List<Attribute> body_data = new List<Attribute>();
            Attribute a = new Attribute("msg", "hallo");
            body_data.Add(a);

            try
            {
                rq.CheckContext("localhost:7000/messages/", "POST", queries, body_data);
                return; // indicates success
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.Message);
            }

        }

        //Put Message
        [Test]
        public void checkContextTest4()
        {
            RequestContext rq = new RequestContext();
            List<Attribute> queries = new List<Attribute>();
            List<Attribute> body_data = new List<Attribute>();
            Attribute a = new Attribute("msg", "hallo");
            body_data.Add(a);

            try
            {
                rq.CheckContext("localhost:7000/messages/", "POST", queries, body_data);
                Attribute b = new Attribute("msg", "hallo");
                List<Attribute> body_data2 = new List<Attribute>();
                body_data.Add(b);
                rq.CheckContext("localhost:7000/messages/1", "PUT", queries, body_data2);
                return; // indicates success
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.Message);
            }

        }

        //Delete Message
        [Test]
        public void checkContextTest5()
        {
            RequestContext rq = new RequestContext();
            List<Attribute> queries = new List<Attribute>();
            List<Attribute> body_data = new List<Attribute>();
            Attribute a = new Attribute("msg", "hallo");
            body_data.Add(a);
            rq.CheckContext("localhost:7000/messages/", "POST", queries, body_data);

            try
            {
                rq.CheckContext("localhost:7000/messages/0", "DELETE", queries, body_data);
                return; // indicates success
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.Message);
            }

        }
    }
}
