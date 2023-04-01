using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hb.AuthServer.Common.Dtos
{
    public  class ErrorDto
    {
        public List<string> Erros { get; private set; }
        public bool  IsShow { get; private set; }
        public ErrorDto()
        {
            Erros = new();
        }
        public ErrorDto(string error,bool isShow)
        {
            Erros= new List<string>() { error};
            this.IsShow= isShow;
        }
        public ErrorDto(List<string> errors, bool isShow)
        {
            Erros = new List<string>(errors); 
            this.IsShow = isShow;
        }

    }
}
