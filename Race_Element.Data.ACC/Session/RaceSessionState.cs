﻿using RaceElement.Broadcast;

namespace RaceElement.Data.ACC.Session
{
    public class RaceSessionState
    {
        public static bool IsFormationLap(bool globalRed, SessionPhase phase)
        {
            return globalRed && (phase == SessionPhase.PreSession || phase == SessionPhase.FormationLap);
        }
    }
}
