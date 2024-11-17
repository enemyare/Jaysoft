using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MerosWebApi.Persistence.Entites
{
    public class DatabaseAnswer
    {
        public string QuestionText { get; set; }

        public List<string> QuestionAnswers { get; set; }
    }
}
