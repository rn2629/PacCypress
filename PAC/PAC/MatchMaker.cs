using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PAC
{
    public struct Period
    {
        public int ID;
        public DateTime StartTime;
        public DateTime EndTime;

        public Period(int id, DateTime startTime, DateTime endTime)
        {
            ID = id;
            StartTime = startTime;
            EndTime = endTime;
        }
    }

    public struct Disponibility
    {
        public string StudentID;
        public DateTime StartTime;
        public DateTime EndTime;
        public int Priority;

        public Disponibility(string studentid, DateTime startTime, DateTime endTime, int priority)
        {
            StudentID = studentid;
            StartTime = startTime;
            EndTime = endTime;
            Priority = priority;
        }
    }

    public struct Pairing
    {
        public string IDStudent;
        public int IDPeriod;
        public int Priority;

        public Pairing(string idStudent, int idPeriod, int priority)
        {
            IDStudent = idStudent;
            IDPeriod = idPeriod;
            Priority = priority;
        }
    }

    public static class MatchMaker
    {
        public static List<Pairing> Match(List<Period> periods, List<Disponibility> disponibilities)
        {
            List<Pairing> assignedPairs = new List<Pairing>();
            List<Pairing> validPairs = GetValidPairs(periods, disponibilities);

            while (validPairs.Count != 0)
            {
                Pairing currentPair = GetFirstSinglePossibilityPairing(validPairs);
                while (currentPair.IDStudent != null)
                {
                    assignedPairs.Add(currentPair);
                    RemoveFromPairings(currentPair, ref validPairs);
                    currentPair = GetFirstSinglePossibilityPairing(validPairs);
                }

                currentPair = GetLessGreedyPairing(validPairs);
                if (currentPair.IDStudent != null)
                {
                    assignedPairs.Add(currentPair);
                    RemoveFromPairings(currentPair, ref validPairs);
                }
            }
            return assignedPairs;
        }

        private static bool CheckPossibility(Disponibility d, Period p)
        {
            if (d.StartTime.DayOfWeek != p.StartTime.DayOfWeek)
                return false;

            if (d.StartTime.Hour * 60 + d.StartTime.Minute < p.StartTime.Hour * 60 + p.StartTime.Minute + Settings.PairingBufferInMinutes
                && d.EndTime.Hour * 60 + d.EndTime.Minute > p.EndTime.Hour * 60 + p.EndTime.Minute - Settings.PairingBufferInMinutes)
                return true;

            return false;
        }

        private static List<Pairing> GetValidPairs(List<Period> periods, List<Disponibility> disponibilities)
        {
            List<Pairing> validPairs = new List<Pairing>();
            foreach (Period p in periods)
                foreach (Disponibility d in disponibilities)
                    if (CheckPossibility(d, p))
                        validPairs.Add(new Pairing(d.StudentID, p.ID, d.Priority));
            return validPairs;
        }

        private static Pairing GetLessGreedyPairing(List<Pairing> validPairs)
        {
            if (validPairs.Count != 0)
            {
                List<Pairing> lessGreedyPairings = new List<Pairing>();
                int currentHunger = validPairs.Count;
                foreach (Pairing p in validPairs)
                {
                    int pairHunger = GetHunger(p, ref validPairs);
                    if (pairHunger < currentHunger)
                    {
                        lessGreedyPairings = new List<Pairing>();
                        currentHunger = pairHunger;
                    }
                    if (pairHunger == currentHunger)
                        lessGreedyPairings.Add(p);
                }

                Pairing toReturn = lessGreedyPairings[0];
                foreach (Pairing p in lessGreedyPairings)
                    if (p.Priority > toReturn.Priority)
                        toReturn = p;
                return toReturn;
            }
            else
            {
                return new Pairing();
            }
        }

        private static Pairing GetFirstSinglePossibilityPairing(List<Pairing> validPairs)
        {
            int i = 0;
            while (i < validPairs.Count)
            {
                if (!AlternativeExist(ref validPairs, i))
                    return validPairs[i];
                i++;
            }
            return new Pairing();
        }

        private static int GetHunger(Pairing pair, ref List<Pairing> pairings )
        {
            int hunger = 0;
            foreach (Pairing p in pairings)
                if (p.IDStudent == pair.IDStudent || p.IDPeriod == pair.IDPeriod)
                    hunger++;
            return hunger;
        }

        private static bool AlternativeExist(ref List<Pairing> pairings, int index)
        {
            bool studentHaveOtherOption = false;
            bool periodHaveOtherOption = false;
            int i = 0;
            while (i < pairings.Count)
            {
                if (i != index)
                {
                    if (pairings[i].IDStudent == pairings[index].IDStudent)
                        studentHaveOtherOption = true;
                    if (pairings[i].IDPeriod == pairings[index].IDPeriod)
                        periodHaveOtherOption = true;

                    if (studentHaveOtherOption && periodHaveOtherOption)
                        return true;
                }
                i++;
            }
            return false;
        }

        private static void RemoveFromPairings(Pairing toRemove, ref List<Pairing> pairings)
        {
            for (int i = 0; i < pairings.Count; i++)
            {
                if (toRemove.IDPeriod == pairings[i].IDPeriod || toRemove.IDStudent == pairings[i].IDStudent)
                {
                    pairings.RemoveAt(i);
                    i--;
                }
            }
        }
    }
}
