using System;
using System.Collections.Generic;
using System.Text;

namespace Cellophain
{
    class Request
    {
        List<Instruction> instructions;
        float priority = 0;

        public Request(List<Instruction> instructions)
        {
            this.instructions = instructions;
        }

        public Request(List<Instruction> instructions, float priority)
        {
            this.instructions = instructions;
            this.priority = priority;
        }

        public List<Instruction> GetInstructions()
        {
            return instructions;
        }

        public float GetPriority()
        {
            return priority;
        }
    }
}
