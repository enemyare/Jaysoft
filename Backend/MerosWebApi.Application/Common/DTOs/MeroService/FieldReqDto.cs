using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MerosWebApi.Application.Common.DTOs.MeroService
{
    public class FieldReqDto
    {
        public string Text { get; set; }

        public string Type { get; set; }

        public bool Required { get; set; }

        public List<string>? Answers { get; set; }
    }
}
