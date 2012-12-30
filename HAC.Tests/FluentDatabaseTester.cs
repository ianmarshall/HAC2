using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HAC.Domain.Repositories;
using NHibernate.Cfg;
using NHibernate.Tool.hbm2ddl;
using NUnit.Framework;
using NHibernate;
using NHibernate.Mapping;
using HAC.Domain;


namespace HAC.Tests
{
    [TestFixture]
    public class FluentDatabaseTester
    {
     
        [TestFixtureSetUp]
        public void setUp()
        {
            DataConfig.EnsureStartup();

        }

        [Test]
        public void TestGetEvents()
        {
            var session = DataConfig.GetSession();
            
                var events = session.CreateCriteria<Events>()
                                    .List<Events>();

                Assert.IsTrue(events.Count > 100);
            
        }

        [Test]
        public void When_saving_should_write_to_database()
        {
            var _event = new Events()
            {
                Name = "ian",
                Lname = "marshall",
                Date = DateTime.Now,udate = DateTime.Now, EndDate = DateTime.Now

            };
            var repository = new EventRepository();
            repository.Save(_event);
            Events loadedEvent;
            using (ISession session = DataConfig.GetSession())
            {
                loadedEvent = session.Load<Events>(
                    _event.Id);
            }
            Assert.That(loadedEvent, Is.Not.Null);
            Assert.That(loadedEvent.Name,
                        Is.EqualTo("ian"));

        }
    }



}
