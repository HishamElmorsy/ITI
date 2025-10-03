using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assignments_exam
{
    internal abstract class Exam
    {
        public int Time { get; set; }
        public int NumberOfQustions { get; set; }
        public Dictionary<Question, Answer> QA { get; set; }
        public ExamMode Mode { get; set; }
        public Subject ExamSubject { get; set; }
        public abstract void ShowExam();
        public Exam():this(0)
        {

        }
        public Exam(int _Time,Subject sub=null)
        {
            Time = _Time;
            QA = new Dictionary<Question, Answer>();
            ExamSubject = sub;
        }

        public delegate void ExamStartedHandler(Exam exam);
        public event ExamStartedHandler ExamStarted;
        public void StartExam()
        {
            Mode = ExamMode.Starting;
            ExamStarted?.Invoke(this);
        }





    }
    enum ExamMode
    {
        Starting, Queued , Finished

    }
    internal class PracticalExam : Exam
    {
        public PracticalExam(int time, Subject sub):base (time, sub)
        {
             
        }
        public override void  ShowExam()
        {
            Console.WriteLine("Practical Exam!");
            foreach(var q in QA.Keys.ToList())
            {

                q.Display();

                Console.Write("Your Answer: ");
                string input = Console.ReadLine();

                List<int> given = input.Split(",").Select(s => int.Parse(s.Trim())).ToList();

                Answer a = new Answer(q, given, 0);
                a.Evaluate();
                QA[q] = a;

                if (q is TrueFalseQuestion TFQ)
                {
                    Console.WriteLine($"The Correct Answer is: {(TFQ.CorrectAnswer)}");
                }
                else if (q is ChooseOneQuestion COQ)
                {
                    Console.WriteLine($"The Correct Answer is: {(COQ.CorrectAnswer)}");
                }
                else if (q is ChooseAllQuestion CAQ)
                {
                    Console.WriteLine($"The Correct Answer is: {string.Join(',',CAQ.CorrectAnswer)}");
                }
                Console.WriteLine("----------------");
            }
        }
    }
    internal class FinalExam : Exam
    {
        public FinalExam(int time, Subject sub) : base(time, sub)
        {
            
        }
        public override void ShowExam()
        {
            Console.WriteLine("Practical Exam!");
            foreach (var q in QA.Keys.ToList())
            {

                q.Display();

                Console.Write("Your Answer: ");
                string input = Console.ReadLine();

                List<int> given = input.Split(",").Select(s => int.Parse(s.Trim())).ToList();

                Answer a = new Answer(q, given, 0);
                a.Evaluate();
                QA[q] = a;

                Console.WriteLine("----------------");
            }
        }
    }
}
