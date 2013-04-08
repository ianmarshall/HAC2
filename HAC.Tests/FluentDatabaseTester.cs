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
        private EventRepository eventRepository;
     
        [TestFixtureSetUp]
        public void setUp()
        {
            eventRepository = new EventRepository();

        }

        [Test]
        public void TestGetEvents()
        {
           


           // var latestEvents = eventRepository.Events.ToList();
            Assert.IsTrue(eventRepository.GetLatestEvents(8).Count() > 1);


        }

        [Test]
        public void When_saving_should_write_to_database()
        {
            var _event = new Event()
                {
                    name = "iantest",
                    lname = "marshall",
                    Date = DateTime.Now,
                    udate = DateTime.Now,
                    EndDate = DateTime.Now,
                    Title = "test",
                    subject = "sub", Description = "me", URL = "ww.gfe"
                };

            eventRepository.Save(_event);


          
            //var ev = eventRepository.Events.FirstOrDefault(x => x.name == "iantest");


            //Assert.That(ev, Is.Not.Null);
            //Assert.That(ev.name,
            //            Is.EqualTo("iantest"));

        }
    }



}
