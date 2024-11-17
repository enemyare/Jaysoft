using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MerosWebApi.Core.Models;

namespace MerosWebApi.Application.Common.DTOs.MeroService
{
    public class FieldResDto
    {
        public string Text { get; set; }

        public string Type { get; set; }

        public bool Required { get; set; }

        public List<string>? Answers { get; set; }

        public static FieldResDto Map(Field field)
        {
            return new FieldResDto
            {
                Text = field.Text,
                Type = field.Type,
                Required = field.Required,
                Answers = field.Answers,
            };
        }
    }
}
