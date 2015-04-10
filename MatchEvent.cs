using System;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace BT_Sport_Server.Opta
{
    public abstract class MatchEvent : IXmlSerializable
    {
        public string SportType { get; private set; }

        public long EventID { get; set; }

        public int MatchID { get; set; }

        public int TeamID { get; set; }

        public int Period { get; set; }

        public int Min { get; set; }

        public int Sec { get; set; }

        public int Time { get; set; }

        public string EventType { get; set; }

        public DateTime Timestamp { get; set; }

        public bool Valid { get; set; }

        protected MatchEvent()
        {
            EventType = String.Empty;
            Timestamp = new DateTime();
        }

        public XmlSchema GetSchema()
        {
            return null;
        }

        public abstract string UpdateMessage { get; }

        protected abstract void ReadFootballXml(XmlReader reader);

        protected abstract void ReadRugbyXml(XmlReader reader);

        public void ReadXml(XmlReader reader)
        {
            if (reader.MoveToContent() != XmlNodeType.Element) return;

            switch (reader.LocalName)
            {
                case "Live":
                    {
                        SportType = "Soccer";
                        ReadFootballXml(reader);
                        Valid = true;
                        break;
                    }
                case "RU50_EventFeed":
                    {
                        SportType = "Rugby";
                        ReadRugbyXml(reader);
                        break;
                    }
                default:
                    {
                        throw new NotImplementedException("Unexpected XML Feed");
                    }
            }
        }

        public void WriteXml(XmlWriter writer)
        {
            throw new NotImplementedException("Writing MatchEvents as XML is not supported. Move or create a copy of the file instead");
        }
    }

    public class Deletion : MatchEvent
    {
        public override string UpdateMessage
        {
            get { return String.Format("UPDATESCORER|SOCCER|{0}|{1}|0|{1}|FORCEUPDATE", MatchID, EventID); }
        }

        protected override void ReadFootballXml(XmlReader reader)
        {
            reader.Read();
            MatchID = Convert.ToInt32(reader["id"]);
            reader.Read();
            EventID = Convert.ToInt64(reader["id"]);
        }

        protected override void ReadRugbyXml(XmlReader reader)
        {
            reader.Read();
            MatchID = Convert.ToInt32(reader["game_id"]);
            reader.Read();
            EventID = Convert.ToInt64(reader["id"]);
        }

        public override string ToString()
        {
            return String.Format("Deletion: {0} Match: {1}", EventType, MatchID);
        }
    }

    public class Substitution : MatchEvent
    {
        public int PlayerOffID { get; set; }

        public int PlayerOnID { get; set; }

        public string Reason { get; set; }

        public Substitution()
        {
            Reason = String.Empty;
        }

        public override string UpdateMessage
        {
            get
            {
                return String.Format("UPDATESUB|{0}|{1}|{2}|{3}|{4}|{5}", SportType.ToUpper(), MatchID, EventID,
                    Timestamp, PlayerOnID, PlayerOffID);
            }
        }

        protected override void ReadFootballXml(XmlReader reader)
        {
            // Live node
            Timestamp = Convert.ToDateTime(reader["timestamp"]);

            // Game node
            reader.ReadToFollowing("Game");
            MatchID = Convert.ToInt32(reader["id"]);

            // Event node
            reader.ReadToFollowing("Event");
            EventID = Convert.ToInt64(reader["id"]);
            Period = Convert.ToInt32(reader["period"]);
            string[] time = reader["time"].Split(':');
            Min = Convert.ToInt32(time[0]);
            Sec = Convert.ToInt32(time[1]);
            Time = Min + 1;
            EventType = "substitution";
            Reason = reader["reason"];

            // Team node
            reader.ReadToFollowing("Team");
            TeamID = Convert.ToInt32(reader["id"]);

            // Player off node
            reader.ReadToFollowing("Player");
            PlayerOffID = Convert.ToInt32(reader["id"]);

            // Player on node
            reader.ReadToFollowing("Player");
            PlayerOnID = Convert.ToInt32(reader["id"]);
        }

        protected override void ReadRugbyXml(XmlReader reader)
        {
            reader.Read();
            // Game node
            MatchID = Convert.ToInt32(reader["game_id"]);

            // Event node
            reader.Read();
            Valid = reader["outcome"] == "1";
            EventID = Convert.ToInt64(reader["id"]);
            Timestamp = Convert.ToDateTime(reader["timestamp"]);
            Period = Match.ConvertPeriod(reader["period"]);
            Min = Convert.ToInt32(reader["period_minute"]);
            Sec = Convert.ToInt32(reader["period_second"]);
            Time = Min + 1;
            TeamID = Convert.ToInt32(reader["team_id"]);
            PlayerOffID = Convert.ToInt32(reader["player_id"]);
            EventType = "substitution";

            // Player off node
            reader.ReadToFollowing("Event");
            PlayerOnID = Convert.ToInt32(reader["player_id"]);

            // First qualifier node
            reader.Read();
            Reason = reader["name"];
        }

        public override string ToString()
        {
            return String.Format("Event: {0} Match: {1} Player On: {2} Player Off: {3}", EventID, MatchID, PlayerOnID, PlayerOffID);
        }
    }

    public class Card : MatchEvent
    {
        public string CardType { get; set; }

        public int PlayerID { get; set; }

        public string Reason { get; set; }

        public Card()
        {
            Reason = String.Empty;
            CardType = String.Empty;
        }

        public override string UpdateMessage
        {
            get { return String.Format("UPDATECARD|{0}|{1}|{2}|{3}|{4}|{5}|", SportType.ToUpper(), MatchID, EventID, Time, PlayerID, CardType); }
        }

        protected override void ReadFootballXml(XmlReader reader)
        {
            // Live node
            Timestamp = Convert.ToDateTime(reader["timestamp"]);

            // Game node
            reader.ReadToFollowing("Game");
            MatchID = Convert.ToInt32(reader["id"]);

            // Event node
            reader.ReadToFollowing("Event");
            EventID = Convert.ToInt64(reader["id"]);
            Period = Convert.ToInt32(reader["period"]);
            string[] time = reader["time"].Split(':');
            Min = Convert.ToInt32(time[0]);
            Sec = Convert.ToInt32(time[1]);
            Time = Min + 1;
            EventType = reader["type"];

            CardType = reader["qualifier"];
            Reason = reader["reason"];

            // Team node
            reader.ReadToFollowing("Team");
            TeamID = Convert.ToInt32(reader["id"]);

            // Scoring player node
            reader.ReadToFollowing("Player");
            PlayerID = Convert.ToInt32(reader["id"]);
        }

        protected override void ReadRugbyXml(XmlReader reader)
        {
            reader.Read();
            MatchID = Convert.ToInt32(reader["game_id"]);

            reader.Read();
            Valid = reader["outcome"] == "1";
            EventID = Convert.ToInt64(reader["id"]);
            EventType = reader["event_type_name"];
            Period = Match.ConvertPeriod(reader["period"]);
            Min = Convert.ToInt32(reader["period_minute"]);
            Sec = Convert.ToInt32(reader["period_second"]);
            Time = Min + 1;
            PlayerID = Convert.ToInt32(reader["player_id"]);
            TeamID = Convert.ToInt32(reader["team_id"]);
            Timestamp = Convert.ToDateTime(reader["timestamp"]);

            reader.Read();
            CardType = reader["name"];
        }

        public override string ToString()
        {
            return String.Format("Event: {0} Match: {1} Card: {2} Player: {3} Reason: {4}", EventID, MatchID, CardType, PlayerID, Reason);
        }
    }

    public class Goal : MatchEvent
    {
        public string GoalType { get; set; }

        public int PlayerAssistID { get; set; }

        public int PlayerScorerID { get; set; }

        public string BodyPart { get; set; }

        public bool OnTarget { get; set; }

        public string ScoringRange { get; set; }

        public int HomeScore { get; set; }

        public int AwayScore { get; set; }

        public Goal()
        {
            GoalType = String.Empty;
            BodyPart = String.Empty;
            ScoringRange = String.Empty;
        }

        public override string UpdateMessage
        {
            get
            {
                return String.Format("UPDATESCORER|{0}|{1}|{2}|{3}|{4}|{5}|{6}", SportType.ToUpper(), MatchID, EventID,
                    Timestamp, PlayerScorerID, GoalType, 0);
            }
        }

        protected override void ReadFootballXml(XmlReader reader)
        {
            // Live node
            Timestamp = Convert.ToDateTime(reader["timestamp"]);

            // Game node
            reader.ReadToFollowing("Game");
            MatchID = Convert.ToInt32(reader["id"]);
            string[] score = reader["score"].Split('-');
            HomeScore = Convert.ToInt32(score[0]);
            AwayScore = Convert.ToInt32(score[1]);

            // Event node
            reader.ReadToFollowing("Event");
            EventID = Convert.ToInt64(reader["id"]);
            BodyPart = reader["bodypart"];
            GoalType = reader["goal_type"];
            OnTarget = reader["on_target"] == "Y";
            Period = Convert.ToInt32(reader["period"]);
            ScoringRange = reader["scoring_range"];
            string[] time = reader["time"].Split(':');
            Min = Convert.ToInt32(time[0]);
            Sec = Convert.ToInt32(time[1]);
            Time = Min + 1;
            EventType = reader["type"];

            // Team node
            reader.ReadToFollowing("Team");
            TeamID = Convert.ToInt32(reader["id"]);

            // Scoring player node
            reader.ReadToFollowing("Player");
            PlayerScorerID = Convert.ToInt32(reader["id"]);

            // Assist player node
            reader.ReadToFollowing("Player");
            PlayerAssistID = Convert.ToInt32(reader["id"]);
        }

        protected override void ReadRugbyXml(XmlReader reader)
        {
            // Game node
            reader.Read();
            HomeScore = Convert.ToInt32(reader["home_score"]);
            AwayScore = Convert.ToInt32(reader["away_score"]);
            MatchID = Convert.ToInt32(reader["game_id"]);

            // Event node
            reader.Read();
            Valid = reader["outcome"] == "1";
            EventID = Convert.ToInt64(reader["id"]);
            EventType = reader["event_type_name"];
            Timestamp = Convert.ToDateTime(reader["timestamp"]);
            GoalType = EventType;
            Min = Convert.ToInt32(reader["period_minute"]);
            Sec = Convert.ToInt32(reader["period_second"]);
            Time = Min + 1;
            TeamID = Convert.ToInt32(reader["team_id"]);
            Period = Match.ConvertPeriod(reader["period"]);
            PlayerScorerID = Convert.ToInt32(reader["player_id"]);
        }

        public override string ToString()
        {
            return String.Format("Event: {0} Match: {1} Player: {2} Score: {3} - {4} Goal Type: {5}", EventID, MatchID,
                PlayerScorerID, HomeScore, AwayScore, GoalType);
        }
    }
}