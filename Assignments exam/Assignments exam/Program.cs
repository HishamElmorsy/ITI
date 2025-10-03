using System.ComponentModel;

namespace Assignments_exam
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Subject Math = new Subject(1, "Math");

            Console.WriteLine("Choose Exam type: 1-Practical / 2-Final ");
            int choice = int.Parse(Console.ReadLine());
            Exam exam;
            if (choice == 1)
            {
                exam = new PracticalExam(60, Math);
            }
            else
            {
                exam = new FinalExam(90, Math);
            }

            exam.ExamStarted += Exam_ExamStarted;

            var q1 = new TrueFalseQuestion("q1", "4+4=8?", 5, true);
            var a1 = new Answer(q1, new List<int> { 1 }, 10);
            a1.Evaluate();
            exam.QA.Add(q1, a1);

            var q2 = new ChooseOneQuestion("q2", "What's 5*3?", 5, 2);
            q2.Choices.AddRange(new[] { "19", "15", "20", "25" });
            var a2 = new Answer(q2, new List<int> { 2 }, 15);
            a2.Evaluate();
            exam.QA.Add(q2, a2);

            var q3 = new ChooseAllQuestion("q3", "Select Prime numbers:", 10, new List<int> { 1, 3 });
            q3.Choices.AddRange(new[] {"2","4","3","6"});
            var a3 = new Answer(q3, new List<int> { 1, 3 }, 20);
            a3.Evaluate();
            exam.QA.Add(q3, a3);

            exam.StartExam();
            exam.ShowExam();
            

        }

        private static void Exam_ExamStarted(Exam exam)
        {
            Console.WriteLine($"Exam on{exam.ExamSubject.SubName} has started!");
        }
    }
}
