//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using HAC.Domain.Repositories;
//using NHibernate.Cfg;
//using NUnit.Framework;
//using NHibernate;
//using HAC.Domain;
//using NHibernate.Tool.hbm2ddl;

//namespace HAC.Tests
//{
//    [TestFixture]
//    public class DatabaseTester
//    {
//        private ISessionFactory sessionFactory;

//        [TestFixtureSetUp]
//        public void setUp()
//        {
//            DataConfig.EnsureStartup();

//        }

//        //[Test, Explicit]
//        //public void CreateDatabaseSchema()
//        //{
//        //    //var export = new SchemaExport(DataConfig.BuildConfiguration());
//        //    ////export.Execute(true, true, false);
//        //    //var configuration = new Configuration().Configure();
//        //    //sessionFactory = configuration.BuildSessionFactory();
//        //    //var scm = new SchemaExport(configuration);
//        //    //scm.Create(false, true);
//        //}

//        //[Test, Explicit]
//        //public void UpdateDatabaseSchema()
//        //{
//        //    var configuration = new Configuration().Configure();
//        //    var v = new SchemaUpdate(configuration);
//        //    v.Execute(true, true);
//        }

//        //[Test, Explicit]
//        //public void OutputDatabaseScript()
//        //{

//        //    //var configuration = new Configuration().Configure();
//        //    //var seessionFactory = configuration.BuildSessionFactory();
//        //    //var scm = new SchemaExport(configuration);
//        //    //scm.SetOutputFile(@"hacdb.sql").Execute(false, false, false);
//        //}

//        [Test, Ignore]
//        public void TestGetMembers()
//        {
//            var mr = new MemberRepository();

//            var member = mr.GetMember("ian@com");
//            Assert.NotNull(member);
//        }



//        [Test]
//        public void When_saving_should_write_to_database()
//        {
//            var member = new Member
//                {
//                    Email = "ian@com",
//                    ForeName = "Ian",
//                    SurName = "Marshall",
//                    Password = "time124",
//                    DOB = new DateTime(1753,1,1),
                    
//                    Profile = new Profile
//                        {
//                            Name = "TEST"
//                        }

//                };


//            using (ISession session = DataConfig.GetSession())
//            {
//                session.BeginTransaction();
//                session.SaveOrUpdate(member);

//                session.Transaction.Commit();
//            }

//            Member loadedMember;

//            using (ISession session = DataConfig.GetSession())
//            {

//                loadedMember = session.Get<Member>(member.Id);
              

//            }
//            Assert.That(loadedMember, Is.Not.Null);
//            Assert.That(loadedMember.Email,
//                       Is.EqualTo("ian@com"));
//        }
//    }



//}
