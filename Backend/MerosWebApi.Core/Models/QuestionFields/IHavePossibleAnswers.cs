using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MerosWebApi.Core.Models.Questions
{
    public interface IHavePossibleAnswers
    {
        void AddPossibleAnswer(string answer);

        void RemovePossibleAnswer(string answer);
    }
}
