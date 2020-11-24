using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using PAC;



namespace PACtests
{
    [TestClass]
    public class HomeControllerTest
    {
        [TestMethod]
        public void TestMethod1()
        {

        }

        [TestMethod]
        public void TestMatchMaking_BufferTime()
        {
            List<Period> periods = new List<Period>();
            List<Disponibility> dispos = new List<Disponibility>();
            periods.Add(new Period(1, new DateTime(1990, 12, 1, 5, 0, 0), new DateTime(1990, 12, 1, 8, 0, 0)));
            dispos.Add(new Disponibility(
                "Jack", 
                new DateTime(1990, 12, 1, 5, PAC.Settings.PairingBufferInMinutes - 1, 0), 
                new DateTime(1990, 12, 1, 8, 0, 0) - new TimeSpan(0, Settings.PairingBufferInMinutes - 1 , 0), 
                1));
            List<Pairing> pairs = MatchMaker.Match(periods, dispos);
            Assert.IsTrue(pairs.Count == 1);
        }

        [TestMethod]
        public void TestMatchMaking_NoMatch()
        {
            List<Period> periods = new List<Period>();
            List<Disponibility> dispos = new List<Disponibility>();
            periods.Add(new Period(1, new DateTime(1990, 12, 1, 5, 0, 0), new DateTime(1990, 12, 1, 5, 8, 0)));
            //wrong year
            dispos.Add(new Disponibility("Jack", new DateTime(1991, 12, 1, 5, 0, 0), new DateTime(1991, 12, 1, 8, 0, 0), 1));
            //wrong day
            dispos.Add(new Disponibility("Bob", new DateTime(1990, 12, 2, 5, 0, 0), new DateTime(1990, 12, 2, 8, 0, 0), 1));
            //wrong hour
            dispos.Add(new Disponibility("AnotherBob", new DateTime(1990, 12, 1, 15, 0, 0), new DateTime(1990, 12, 1, 18, 0, 0), 1));
            List<Pairing> pairs = MatchMaker.Match(periods, dispos);
            Assert.IsTrue(pairs.Count == 0);
        }

        [TestMethod]
        public void TestMatchmaking_NoData()
        {
            List<Period> periods = new List<Period>();
            List<Disponibility> dispos = new List<Disponibility>();
            List<Pairing> pairs = MatchMaker.Match(periods, dispos);
            Assert.IsTrue(pairs.Count == 0);
        }

        [TestMethod]
        public void TestMatchmaking_OneWeekOffset()
        {
            List<Period> periods = new List<Period>();
            List<Disponibility> dispos = new List<Disponibility>();
            periods.Add(new Period(1, new DateTime(1990, 12, 1, 5, 0, 0), new DateTime(1990, 12, 1, 8, 0, 0)));
            dispos.Add(new Disponibility("Jack", new DateTime(1990, 12, 8, 5, 0, 0), new DateTime(1990, 12, 8, 8, 0, 0), 1));
            List<Pairing> pairs = MatchMaker.Match(periods, dispos);
            Assert.IsTrue(pairs.Count == 1);
        }

        [TestMethod]
        public void TestMatchmaking_Simple()
        {
            List<Period> periods = new List<Period>();
            List<Disponibility> dispos = new List<Disponibility>();
            periods.Add(new Period(1, new DateTime(1990, 12, 1, 5, 0, 0), new DateTime(1990, 12, 1, 8, 0, 0)));
            periods.Add(new Period(2, new DateTime(1990, 12, 2, 5, 0, 0), new DateTime(1990, 12, 2, 8, 0, 0)));
            dispos.Add(new Disponibility("Jack", new DateTime(1990, 12, 1, 5, 0, 0), new DateTime(1990, 12, 1, 8, 0, 0), 1));
            dispos.Add(new Disponibility("Jack", new DateTime(1990, 12, 2, 5, 0, 0), new DateTime(1990, 12, 2, 8, 0, 0), 1));
            dispos.Add(new Disponibility("Bob", new DateTime(1990, 12, 2, 5, 0, 0), new DateTime(1990, 12, 2, 8, 0, 0), 1));
            List<Pairing> pairs = MatchMaker.Match(periods, dispos);

            Assert.IsTrue(pairs.Count == 2);
        }

            /*simple
            hard case*/
    }
}
