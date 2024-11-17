﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MerosWebApi.Persistence.Entites
{
    public class DatabaseField
    {
        public string Text { get; set; }

        public string Type { get; set; }

        public bool Required { get; set; }

        public List<string> PossibleAnswers { get; set; }
    }
}
