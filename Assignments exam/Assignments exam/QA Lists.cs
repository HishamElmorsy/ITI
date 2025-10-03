using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assignments_exam
{
    internal class QustionList:List<Question>
    {
        public string LogFilePath { get;set; }

        public QustionList(string filePath)
        {
            LogFilePath = filePath;
        }
        public new void Add(Question q)
        {
            base.Add(q);
            using (var writer = new StreamWriter(LogFilePath, true))
            {
                writer.WriteLine(q.Header);
                writer.WriteLine(q.Body);
                writer.WriteLine(q.Marks);
                writer.WriteLine("---------");

            }
            ;
        }
    }

    internal class Answer:ICloneable
    {
        public bool IsCorrect { get; set; }
        public List<int> GivenAnswer { get; set; }
        public int AnswerTime { get; set; }

        public Question RelatedQuestion { get; set; }

        public Answer(Question q, List<int> _GivenAnswer,int _AnswerTime)
        {
            RelatedQuestion = q;
            GivenAnswer = _GivenAnswer;
            AnswerTime = _AnswerTime;
        }

        public void Evaluate()
        {
            if (RelatedQuestion is TrueFalseQuestion TFQ)
            {
                bool stdAnswer = GivenAnswer[0] == 1;
                IsCorrect = stdAnswer == TFQ.CorrectAnswer;
            }
            else if(RelatedQuestion is ChooseOneQuestion COQ)
            {
                int stdAnswer = GivenAnswer[0];
                int correct = COQ.CorrectAnswer;
                IsCorrect = stdAnswer == correct;
            }
            else if (RelatedQuestion is ChooseAllQuestion CAQ)
            {
                IsCorrect = CAQ.CorrectAnswer.Count == GivenAnswer.Count;
                if (IsCorrect)
                {
                    foreach (int a in CAQ.CorrectAnswer)
                    {
                        if (!GivenAnswer.Contains(a))
                        {
                            IsCorrect = false;
                            break;
                        }
                    }
                }
            }
        }

        public object Clone()
        {
            return new Answer(RelatedQuestion, new List<int>(GivenAnswer), AnswerTime)
            {
                IsCorrect = this.IsCorrect
            };
        }
        public override string ToString()
        {
            string a = string.Join(",", GivenAnswer);
            return $"Answer => Correct:{IsCorrect},Given:[{a}], Time:{AnswerTime}s";
        }

        
        
    }
    internal class AnswerList : List<Answer>
    {
        public string LogFilePath { get; set; }

        public AnswerList(string filePath)
        {
            LogFilePath = filePath;
        }

        public new void Add(Answer a)
        {
            base.Add(a);
            using (var writer = new StreamWriter(LogFilePath, true))
            {
                writer.WriteLine(a.ToString());
                writer.WriteLine("---------");
            }
        }
    }
}
