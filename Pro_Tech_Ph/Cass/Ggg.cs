using System;
namespace Pro_Tech_Ph.Cass
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
    public class Login_Res
    {
       public Code_Res Code_Res { get; set; }

       public string Jwt_Token { get; set;}
        public API_User Data { get; set; }

        //{"ode_Res":{"isDone":true,"msg":"User Found"},"

        //jwt_Token":"NkYtODEtQTEtRTgtQUUtQzUtQTUtQUQtMTQtMUQtNTUtOUQtNEMtNkItMTUtMDEtMkItMUYtQ0YtNEEtOTktRTAtODItNTUtNjAtRDEtNTYtOTgtRkUtMzgtMzctQ0ItNzctNTktQkMtNDQtQ0YtMTItMzEtQ0MtOEMtQUItODYtNjQtMTMtM0EtOTItMzktNUUtOTYtQTgtNzktQTktQ0YtMDAtNjUtM0QtNkItRjktMTktOUUtQjItN0ItNUQtMTEtMUEtQzYtMzctNzMtRjQtMkUtNjEtODItMTQtRjktNTAtMDQtQjQtODUtOTktQTgtODgtRDItMEUtOTAtODQtMEQtM0EtRjYtM0EtODgtMDgtNzktNzQtQjctOEYtN0UtNzMtMTQtOTEtMjktNEEtNzktMEMtNDQtRDctODUtOTctOEQtMUUtNTYtOEItNzgtRDMtNDktOEItQ0MtOTYtOUItMDMtMUYtNTUtNUEtREEtOUItRTktOUItMDAtQkMtRTctODQtMTgtQ0UtNjMtRDItQjItMDMtOUEtNjQtNUYtOTMtOUUtQUItNEEtRTctMzYtMEMtMTktODItRkMtRDMtRjMtMTEtRDgtRTAtNjAtMjAtOEUtMDQtRTUtOTItQjItQTMtRUMtNEQtMkEtRDctRUYtNzMtOTgtQkMtNTctOEMtRTQtRDAtOTYtQUYtOUItODItQTktQzUtQkQtNzktNUMtNEMtQUItN0EtODEtRUMtRTQtQTEtMzItOTQtMTktRTMtNjYtN0EtMUYtNEMtNkQtQzctNUYtMTUtMkYtNUEtNzUtQzctRTEtN0YtM0UtQ0MtRkMtRkMtNUYtNDYtQjQtN0EtQ0YtN0ItMjEtRUMtQkMtQzUtMDctQ0YtRDQtRDAtQTAtQ0YtMTktQUUtNzYtNDQtMjAtM0QtQjQtRDktNEYtRDItODEtNjktNEYtOEEtMjAtNzgtMzEtMTctNkYtNEMtNzMtMkQtNzAtQkMtMkItRkMtRDMtQjAtRDktQTMtQkQtNEYtODMtOTItOTktRTAtNTUtQzktQjgtODctRkYtN0ItQzgtOTMtRjUtQjgtRDEtRDYtNTYtQzYtOUItMTUtOTMtRDktRDItQkQtNjEtMjQtNEMtNEMtMkYtQjgtM0MtQkMtMzgtMzAtNjUtRjItREUtQjYtRUUtNDAtMkQtMjEtNzAtNDAtRDItNUItNUUtMDYtODItMjUtQTQtQ0QtMkMtMDktMzUtQkQtQTQtQzItRTUtNjUtNzUtODEtRjgtMjktNkUtRTItMjktMDgtM0UtN0EtQzYtOUItQzgtRDctMkMtODktREEtMEEtNDEtOEYtMEItMkItNDYtRDYtOTctMjQtRTMtQjktQTEtNTctREMtNjEtRkItOUUtMjUtQUMtNDMtRUQtNjgtRDEtQzktQjUtOUUtOTMtQjctQTMtNzItNDQtM0MtQjEtRDMtNkMtNzktOTgtM0QtQTctRTgtOTUtNkItQ0MtRUUtODMtODItRDktNUQtN0YtQ0EtQjMtODYtMzctRDUtRDQtMjMtRjYtRUMtNEItNzUtMjItQTctOUYtMDAtRkEtQTgtNDEtMjEtODMtNTktMEEtQjUtNTYtM0UtMDctNjYtQzUtNjktOTItOTUtNzYtRDMtRTgtNjAtNEItNEItQjMtRUMtNDgtMzgtRTUtN0ItRjktN0UtREYtNjMtMTktOTMtQzgtODgtNjAtMDEtOUMtODQtOTItRUEtNzctNTgtQ0YtRkItMjUtNTktRjktQkUtMzItNjYtNTgtMUUtNjEtMjgtNDYtMUUtQ0UtOEYtMUItMEItQzQtOEUtODktNUYtNTQtMjYtNkMtMEYtRTUtMUItOTYtRjEtNTYtQTItM0MtRTAtQjEtMUQtMEEtMjUtQ0EtNzktRjEtNTEtNDMtRDQtQzAtMkEtNTItQjktOTAtMTEtMzEtQTctNDAtNUMtMjYtMjgtRUEtQzMtMkEtOTMtNjgtREUtNDUtMjgtQUYtOEEtRjQtMzQtNTYtQTgtNzgtREItNDItQUMtNjQtOTYtMTYtNUUtQ0YtMDctNzItQjUtRTMtRkUtNzYtMEMtMTQtM0YtMjYtRTYtRkEtQTEtMDItNTgtQTEtN0MtMUQtQTAtQkYtN0UtNDgtNTYtMkQtQTgtRUYtODctNzEtRUYtMUItODMtNUYtRDItNzItQkItNjYtNzktQTUtQTEtMDItREMtRTItRjktNDYtMTYtM0UtNDYtMkMtQzEtQzEtQjItMEYtNzItQkUtRUMtMTktQUUtREMtODEtNjMtNDMtQjQtNzItNzgtMUMtMEYtNDctNTEtNUQtOTItQjktRDItNzYtNDUtMDMtQUItNjQtRkUtMEEtRTAtOEYtMEEtOEQtMDQtQUQtMjAtRUUtMDItRjMtN0MtQTQtOUUtQkEtRkQtQUItRTUtQzItNTMtMzMtODgtOUUtMEItREYtNTgtNzQtNDMtMjQtMkMtQjktOUYtNUUtQ0ItMEItQzktQzk="
        //,"data":{"id":2,"u_Role":1,"full_Name":"ali","email":"asd@asd.com","phone":"2343444"}}
    }
}

