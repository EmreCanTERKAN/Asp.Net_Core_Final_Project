namespace Project.MVC.Models
{
    public class ServiceResponseDto<T>
    {
        public bool IsSucceed { get; set; }
        public string Message { get; set; }
        public T Data { get; set; }
    }
}
