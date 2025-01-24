namespace zKassa_Server.ControllerModels
{
    public record LoginModel
    {
        public string UserName { get; set; }
        public string Password { get; set; }
    }
}
