using System;
using System.Xml.Serialization;
using BT_Sport_Server;
using BT_Sport_Server.Opta;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BT_Sport_Server_Tests.Opta
{
    [TestClass]
    public class MatchEventTests
    {
        private readonly string filePath = "\\\\10.101.31.18\\FTP Data\\Processed\\TESTS\\";

        [TestMethod]
        public void FootballDeleteMatchEventTests()
        {
            Deletion deletion = XmlHandler.DeserializeFile<Deletion>(filePath + "f50-8-2014-755628-113495906-deleted[20150409-111004].xml", new XmlRootAttribute { ElementName = "Live" });

            Assert.AreEqual(755628, deletion.MatchID);
            Assert.AreEqual(113495906, deletion.EventID);
        }

        [TestMethod]
        public void FootballGoalMatchEventTests()
        {
            Goal goal = XmlHandler.DeserializeFile<Goal>(filePath + "f50-98-2014-741613-1651683701-goal[20140908-012546].xml", new XmlRootAttribute { ElementName = "Live" });

            // Event
            Assert.AreEqual(Convert.ToDateTime("2014-09-08 01:26:05"), goal.Timestamp);
            Assert.AreEqual(741613, goal.MatchID);
            Assert.AreEqual(1651683701, goal.EventID);
            Assert.AreEqual(2, goal.Period);
            Assert.AreEqual(59, goal.Min);
            Assert.AreEqual(52, goal.Sec);
            Assert.AreEqual(60, goal.Time);
            Assert.AreEqual(928, goal.TeamID);
            Assert.AreEqual("goal", goal.EventType);

            // Goal
            Assert.AreEqual(42532, goal.PlayerScorerID);
            Assert.AreEqual(17152, goal.PlayerAssistID);
            Assert.AreEqual("right footed", goal.BodyPart);
            Assert.AreEqual("goal", goal.GoalType);
            Assert.AreEqual(true, goal.OnTarget);
            Assert.AreEqual(2, goal.HomeScore);
            Assert.AreEqual(1, goal.AwayScore);
            Assert.AreEqual("Box, Right", goal.ScoringRange);
        }

        [TestMethod]
        public void FootballSubstitutionMatchEventTest()
        {
            Substitution substitution = XmlHandler.DeserializeFile<Substitution>(filePath + "f50-98-2015-791409-288155345-sub[20150330-020249].xml", new XmlRootAttribute { ElementName = "Live" });

            // Event
            Assert.AreEqual(Convert.ToDateTime("2015-03-30 02:06:18"), substitution.Timestamp);
            Assert.AreEqual(791409, substitution.MatchID);
            Assert.AreEqual(288155345, substitution.EventID);
            Assert.AreEqual(2, substitution.Period);
            Assert.AreEqual(91, substitution.Min);
            Assert.AreEqual(50, substitution.Sec);
            Assert.AreEqual(92, substitution.Time);
            Assert.AreEqual(1899, substitution.TeamID);
            Assert.AreEqual("substitution", substitution.EventType);

            // Substitution
            Assert.AreEqual(194187, substitution.PlayerOffID);
            Assert.AreEqual(150229, substitution.PlayerOnID);
            Assert.AreEqual("tactical", substitution.Reason);
        }

        [TestMethod]
        public void FootballCardMatchEventTest()
        {
            Card card = XmlHandler.DeserializeFile<Card>(filePath + "f50-98-2015-791409-264392395-card[20150330-013755].xml", new XmlRootAttribute { ElementName = "Live" });

            // Event
            Assert.AreEqual(Convert.ToDateTime("2015-03-30 01:41:18"), card.Timestamp);
            Assert.AreEqual(791409, card.MatchID);
            Assert.AreEqual(264392395, card.EventID);
            Assert.AreEqual(66, card.Min);
            Assert.AreEqual(51, card.Sec);
            Assert.AreEqual(67, card.Time);
            Assert.AreEqual(2077, card.TeamID);
            Assert.AreEqual(2, card.Period);
            Assert.AreEqual("card", card.EventType);

            // Card
            Assert.AreEqual("yellow", card.CardType);
            Assert.AreEqual("foul", card.Reason);
            Assert.AreEqual(207760, card.PlayerID);
        }

        [TestMethod]
        public void RugbyDeleteMatchEventTests()
        {
            Deletion deletion = XmlHandler.DeserializeFile<Deletion>(filePath + "RU50-242-2015-215701-3123689[20150405-132610].xml", new XmlRootAttribute { ElementName = "RU50_EventFeed" });

            Assert.AreEqual(215701, deletion.MatchID);
            Assert.AreEqual(3123689, deletion.EventID);
        }

        [TestMethod]
        public void RugbyTryMatchEventTests()
        {
            Goal goal = XmlHandler.DeserializeFile<Goal>(filePath + "RU50-201-2015-115183-3070195[20150329-161257].xml", new XmlRootAttribute { ElementName = "RU50_EventFeed" });

            // Event
            Assert.AreEqual(Convert.ToDateTime("2015-03-29 16:14:47"), goal.Timestamp);
            Assert.AreEqual(115183, goal.MatchID);
            Assert.AreEqual(3070195, goal.EventID);
            Assert.AreEqual(2, goal.Period);
            Assert.AreEqual(66, goal.Min);
            Assert.AreEqual(29, goal.Sec);
            Assert.AreEqual(67, goal.Time);
            Assert.AreEqual(1000, goal.TeamID);
            Assert.AreEqual("Try", goal.EventType);

            // Goal
            Assert.AreEqual(9877, goal.PlayerScorerID);
            Assert.AreEqual("Try", goal.GoalType);
            Assert.AreEqual(7, goal.HomeScore);
            Assert.AreEqual(36, goal.AwayScore);
        }

        [TestMethod]
        public void RugbySubstitutionMatchEventTest()
        {
            Substitution substitution = XmlHandler.DeserializeFile<Substitution>(filePath + "RU50-201-2015-115183-3070191[20150329-160657].xml", new XmlRootAttribute { ElementName = "RU50_EventFeed" });
            
            // Event
            Assert.AreEqual(Convert.ToDateTime("2015-03-29 16:10:27"), substitution.Timestamp);
            Assert.AreEqual(115183, substitution.MatchID);
            Assert.AreEqual(3070190, substitution.EventID);
            Assert.AreEqual(2, substitution.Period);
            Assert.AreEqual(68, substitution.Min);
            Assert.AreEqual(23, substitution.Sec);
            Assert.AreEqual(69, substitution.Time);
            Assert.AreEqual(1000, substitution.TeamID);
            Assert.AreEqual("substitution", substitution.EventType);

            // Substitution
            Assert.AreEqual(8060, substitution.PlayerOffID);
            Assert.AreEqual(18522, substitution.PlayerOnID);
            Assert.AreEqual("Tactical", substitution.Reason);
        }

        [TestMethod]
        public void RugbyCardMatchEventTest()
        {
            Card card = XmlHandler.DeserializeFile<Card>(filePath + "RU50-201-2015-115183-3070207[20150329-162711].xml", new XmlRootAttribute { ElementName = "RU50_EventFeed" });

            // Event
            Assert.AreEqual(Convert.ToDateTime("2015-03-29 16:30:32"), card.Timestamp);
            Assert.AreEqual(115183, card.MatchID);
            Assert.AreEqual(3070207, card.EventID);
            Assert.AreEqual(79, card.Min);
            Assert.AreEqual(54, card.Sec);
            Assert.AreEqual(80, card.Time);
            Assert.AreEqual(78, card.TeamID);
            Assert.AreEqual(2, card.Period);
            Assert.AreEqual("Card", card.EventType);

            // Card
            Assert.AreEqual("Yellow", card.CardType);
            Assert.AreEqual(1475, card.PlayerID);
        }
    }
}