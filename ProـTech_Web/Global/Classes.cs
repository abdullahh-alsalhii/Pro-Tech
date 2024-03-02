namespace ProـTech_Web.Global
{
    public class Code_Res
    {
        public bool IsDone { get; set; }
        public string Msg { get; set; }
    }
    public class Res : Code_Res
    {

    }
    public class Res<T> : Res
    {
        public T Data { get; set; }
    }

    public class Code_Res_With_Data<T>
    {
        public Code_Res Code_Res { get; set; }
        public T Data { get; set; }
    }

    public class GetFromTo
    {
        public int From { get; set; }
        public int To { get; set; }
        public string? Search { get; set; }
        public DateTime? From_time { get; set; }
        public DateTime? To_time { get; set; }
    }
    public class GetFromTo_DTO<T>
    {       
        public GetFromTo From_To { get; set; }
        public T Data { get; set; }
    }
}
