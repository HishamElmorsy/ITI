using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assignments_exam
{
    internal class Question
    {
        public string Header { get; set; }
        public string Body { get; set; }
        public int Marks { get; set; }
        
        public Question()
        {
            
        }

        public Question(string _Header, string _Body, int _Marks)
        {
            Header = _Header;
            Body = _Body;
            Marks = _Marks;
        }

        public virtual void Display() { }

        public override string ToString()
        {
            return $"[Question] {Header} | Marks: {Marks}\n{Body}";
        }

    }

    internal class TrueFalseQuestion : Question
    {
        public bool CorrectAnswer { get; set; }

        public TrueFalseQuestion(string _Header, string _Body, int _Marks, bool _CorrectAnswer) : base( _Header,  _Body, _Marks)
        {
            CorrectAnswer = _CorrectAnswer;
        }
        public override void Display()
        {
            Console.WriteLine(Header);
            Console.WriteLine(Body);
            Console.WriteLine(Marks);
            Console.WriteLine("1)True , 2)False");
        }
    }
    internal class ChooseOneQuestion : Question
    {
        public int CorrectAnswer { get; set; }
        public List<string> Choices { get; set; }

        public ChooseOneQuestion(string _Header, string _Body, int _Marks, int _CorrectAnswer) : base(_Header, _Body, _Marks)
        {
            CorrectAnswer = _CorrectAnswer;
            Choices = new List<string>();
        }
        public override void Display()
        {
            Console.WriteLine(Header);
            Console.WriteLine(Body);
            Console.WriteLine(Marks);
            for (int i = 0; i < Choices.Count; i++)
            {
                Console.WriteLine($"{i+1}- {Choices[i]}");
            }
        }
    }
    internal class ChooseAllQuestion : Question
    {
        public List<int> CorrectAnswer { get; set; }
        public List<string> Choices { get; set; }


        public ChooseAllQuestion(string _Header, string _Body, int _Marks, List<int> _CorrectAnswer) : base(_Header, _Body, _Marks)
        {
            CorrectAnswer = _CorrectAnswer;
            Choices = new List<string>();
        }
        public override void Display()
        {
            Console.WriteLine(Header);
            Console.WriteLine(Body);
            Console.WriteLine(Marks);
            for (int i = 0; i < Choices.Count; i++)
            {
                Console.WriteLine($"{i + 1}- {Choices[i]}");
                

            }
        }
    }
}
