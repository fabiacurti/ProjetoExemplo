namespace Business.TransferObjects.Mensagens
{
    public class MensagemSucessoDto
    {
        public MensagemSucessoDto() { }
        public MensagemSucessoDto(string mensagem, object data)
        {
            this.mensagem = mensagem;
            this.data = data;
        }
        public string mensagem { get; set; }
        public object data { get; set; }
    }
}
