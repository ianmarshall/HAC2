using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using HAC.Domain.Repositories;
using NHibernate.Cfg;
using NUnit.Framework;
using NHibernate;
using HAC.Domain;
using NHibernate.Tool.hbm2ddl;
using System.Data.Entity;

namespace HAC.Tests
{
    [TestFixture]
    public class DatabaseTester
    {
        private MemberRepository memberRepository;

        [TestFixtureSetUp]
        public void setUp()
        {
         
            memberRepository = new MemberRepository();
        


        }


        //[Test]
        //public void TestGetMembers()
        //{
        //    var mr = new MemberRepository();

        //    var member = mr.GetMember("ian@com");
        //    Assert.NotNull(member);
        //}



        [Test]
        public void When_saving_should_write_to_database()
        {
            var member = new Member
                {
                    Email = "ian@com",
                    ForeName = "Ian",
                    SurName = "Marshall",
                    Password = "time124",
                    DOB = new DateTime(1753, 1, 1),
                };

            memberRepository.Save(member);

            HACEntities context = new HACEntities();

            Member loadedMember = context.Members.FirstOrDefault(m => m.Id == member.Id);

            Assert.That(loadedMember, Is.Not.Null);
            Assert.That(loadedMember.Email,
                       Is.EqualTo("ian@com"));
        }
    }



}
