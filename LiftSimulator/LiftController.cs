﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LiftSimulator
{
    class LiftController
    {
        // List of lifts we control
        private Lift[] lifts;
        // Index for adding items to the lifts array
        private int ali = 0;
        // Used for picking a lift if we have a clash
        Random randGen = new Random();

        // This is magic.
        private int[,] liftFlrPrio = new int[5,5] {
            {0,40,30,20,10},
            {40,0,40,30,20},
            {30,40,0,40,30},
            {20,30,40,0,40},
            {10,20,30,40,0}
        };

        // Up and down queues for when 'people' push the up/down buttons on the floors
        private int[] upQueueWaiting = new int[5] { 0, 0, 0, 0, 0 };
        private int[] downQueueWaiting = new int[5] { 0, 0, 0, 0, 0 };

        public LiftController(int lnum)
        {
            lifts = new Lift[lnum];
        }

        public void AddLift(Lift l)
        {
            lifts[ali] = l;
            ali++;
        }

        public void GoingUp(int calledFrom)
        {
            upQueueWaiting[calledFrom] = 1;
        }

        public void GoingDown(int calledFrom)
        {
            downQueueWaiting[calledFrom] = 1;
        }

        public void SendLift(int floor)
        {
            int nl = GetNearestLift(floor);

            if (nl != -1)
            {
                lifts[nl].Move(floor);
            }
        }

        private int GetNearestLift(int floor)
        {
            // TODO: Check direction lift is already travelling
            int liftToSend = -1;
            int highestPrio = -1;

            for (int i = 0; i < lifts.Count(); i++)
            {
                int liftFloor = lifts[i].GetCurrentFloor;
                int liftPrio = liftFlrPrio[liftFloor, floor];

                if (liftPrio == 0) { return -1; }

                if (highestPrio < liftPrio)
                {
                    highestPrio = liftPrio;
                    liftToSend = i;
                }
            }

            return liftToSend;
        }
    }
}
