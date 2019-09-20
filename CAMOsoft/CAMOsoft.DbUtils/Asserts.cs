using System;
using System.Collections.Generic;
using System.Text;

namespace CAMOsoft.DbUtils
{
    public static class Asserts
    {

        public static void Assert(bool pCond, string pMessage)
        {
            if (!pCond)
                if (pMessage == null)
                    throw new Exception("Assertion failed");
                else
                    throw new Exception("Assertion failed: " + pMessage);
        }

        public static void Assert(bool pCond, Exception pThrowEx)
        {
            if (!pCond)
                if (pThrowEx == null)
                    throw new Exception("Assertion failed");
                else
                    throw pThrowEx;
        }

        public static void Assert(bool pCond)
        {
            Assert(pCond, String.Empty);
        }

    }
}
