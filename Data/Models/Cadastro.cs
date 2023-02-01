using System;
using System.Collections.Generic;
using System.Text;

namespace Data.Models
{
    public class Cadastro
    {
        public Guid Id { get; set; }

        public string Nome { get; set; }

        public int Cpf { get; set; }

        public string Email { get; set; }

        private string Situacao { get; set; }






    }
}
