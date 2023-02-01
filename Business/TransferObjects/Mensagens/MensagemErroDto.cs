namespace Business.TransferObjects.Mensagens
{
    public class MensagemErroDto
    {
        public MensagemErroDto() { }
        public MensagemErroDto(string mensagem)
        {
            this.mensagem = mensagem;
            this.data = null;
        }
        public MensagemErroDto(string mensagem, object data)
        {
            this.mensagem = mensagem;
            this.data = data;
        }
        public string mensagem { get; set; }
        public object data { get; set; }
    }
}
