﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SimpleComponents
{
    public class Clock
    {
        private List<SequentialGate> m_lListeners;
        private bool m_bUp;

        private static Clock m_cClock = new Clock();

        private Clock()
        {
            m_lListeners = new List<SequentialGate>();
            m_bUp = true;
        }

        private void Register(SequentialGate g)
        {
            m_lListeners.Add(g);
        }
        public static void RegisterSequentialGate(SequentialGate g)
        {
            m_cClock.Register(g);
        }

        public static void Clear()
        {
            m_cClock = new Clock();
        }

        private void Up()
        {
            if (!m_bUp)
            {
                foreach (SequentialGate g in m_lListeners)
                    g.OnClockUp();
                m_bUp = true;
            }
        }

        private void Down()
        {
            if (m_bUp)
            {
                foreach (SequentialGate g in m_lListeners)
                    g.OnClockDown();
                m_bUp = false;
            }
        }

        public static void ClockUp()
        {
            m_cClock.Up();
        }
        public static void ClockDown()
        {
            m_cClock.Down();
        }
    }
}