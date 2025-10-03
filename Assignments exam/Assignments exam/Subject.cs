using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assignments_exam
{
    internal class Subject
    {
        public int SubID { get; set; }
        public string SubName { get; set; }
        public Subject(int _SubID, string _SubName)
        {
            SubID = _SubID;
            SubName = _SubName;
        }
        public override string ToString()
        {
            return $"{SubID} - {SubName}";
        }
    }

}
