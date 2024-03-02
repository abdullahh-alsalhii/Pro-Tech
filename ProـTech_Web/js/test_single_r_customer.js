"use strict";
const main_2 = async () => {
    var customer_jwt = "NkYtODEtQTEtRTgtQUUtQzUtQTUtQUQtMTQtMUQtNTUtOUQtNEMtNkItMTUtMDEtMkItMUYtQ0YtNEEtOTktRTAtODItNTUtNjAtRDEtNTYtOTgtRkUtMzgtMzctQ0ItNzctNTktQkMtNDQtQ0YtMTItMzEtQ0MtOEMtQUItODYtNjQtMTMtM0EtOTItMzktNUUtOTYtQTgtNzktQTktQ0YtMDAtNjUtM0QtNkItRjktMTktOUUtQjItN0ItNUQtMTEtMUEtQzYtMzctNzMtRjQtMkUtNjEtODItMTQtRjktNTAtMDQtQjQtODUtOTktQTgtODgtRDItMEUtOTAtODQtMEQtM0EtRjYtM0EtODgtMDgtNzktNzQtQjctOEYtN0UtNzMtMTQtOTEtMjktNEEtNzktMEMtNDQtRDctODUtOTctOEQtMUUtNTYtOEItNzgtRDMtNDktOEItQ0MtOTYtOUItMDMtMUYtNTUtNUEtREEtOUItRTktOUItMDAtQkMtRTctODQtMTgtQ0UtNjMtRDItQjItMDMtOUEtNjQtNUYtOTMtOUUtQUItNEEtRTctMzYtMEMtMTktODItRkMtRDMtRjMtMTEtRDgtRTAtNjAtMjAtOEUtMDQtRTUtOTItQjItQTMtRUMtNEQtMkEtRDctRUYtNzMtOTgtQkMtNTctOEMtRTQtRDAtOTYtQUYtOUItODItQTktQzUtQkQtNzktNUMtNEMtQUItN0EtODEtRUMtRTQtQTEtMzItOTQtMTktRTMtNjYtN0EtMUYtNEMtNkQtQzctNUYtMTUtMkYtNUEtNzUtQzctRTEtN0YtM0UtQ0MtRkMtRkMtNUYtNDYtQjQtN0EtQ0YtN0ItMjEtRUMtQkMtQzUtMDctQ0YtRDQtRDAtQTAtQ0YtMTktQUUtNzYtNDQtMjAtM0QtQjQtRDktNEYtRDItODEtNjktNEYtOEEtMjAtNzgtMzEtMTctNkYtNEMtNzMtMkQtNzAtQkMtMkItRkMtRDMtMTAtNjItMjItQTgtQTYtMjUtRTctNDEtNzItRDktMUEtNEItNDAtMTUtNUYtMzYtNDQtNTYtOUQtRDEtNzctMUYtNzItRUEtRjgtNjYtQTMtQTUtMjAtMEUtOUEtNTItRUMtQjgtQjctRjItREQtMUItQzUtMkUtODUtRkMtMDUtNTctMjAtMTEtNjItQTctMTUtOEMtRUUtQjctMzAtODMtOTQtNDItQ0UtMzgtRjAtQTItNEQtQTUtOEYtMEMtMDAtMzUtNkUtNUEtQzAtNTMtQTEtMzUtMkYtRkItMEItQTYtNUItRjItOTUtN0UtOUUtQzEtQ0YtODgtNTktQUItOTctRkUtNkMtQzYtOEYtN0ItNDItMEMtRjYtMDMtRTUtNkYtQTUtRDgtMDUtQkYtMEQtMjAtQzUtRkMtRjEtQzEtMDctOEQtNjQtQ0EtNjUtMDYtQ0MtQ0ItQzYtQkQtREMtMjUtQzgtQTQtMkMtQ0UtNEUtNjQtRDAtM0ItNTYtNEUtRDUtRUQtMzUtQzctOEYtQkMtNkYtOEQtM0QtN0EtQTAtQzMtNjMtQ0EtNUQtMzQtMjEtREYtODQtM0QtNUQtQjktMjQtRjItMTMtOTMtOEEtMUQtNTctRkMtODUtREMtMjYtQjQtRTYtMUItMDktQzEtQzYtQTgtMjctRTMtRTMtNTQtMEYtRDQtMDctNTQtMzctQTMtNTQtRDMtQTgtQjItODMtOUEtRTYtNzgtMEYtQkMtNzctQjQtNzgtMTktMEQtQkEtNkItNTMtMTItNzctMEMtRDUtQTgtOTctMjItOEYtNTctRTktNTUtQUMtNDUtMkYtOUQtNDctQ0QtRUQtMTUtOTAtRjAtNUItMjgtMDAtNTEtMUEtMkEtRTctM0ItRjUtQTAtOEEtODgtNjItRDMtQkQtMUMtM0QtMUYtQkItQUMtRTUtRjItQzMtMEQtNjAtNTgtQzgtMkEtNUUtRDUtQjEtQTgtQjgtMDQtOEYtMUUtMUQtODMtRkItMEItRTMtREEtQUItQ0MtNDItNTQtQkYtOUMtNjAtRkEtNzMtQTYtQzYtNTktRkMtNkMtMzItMzItNUMtNDYtNDktNEItNzQtQzQtNTMtQkYtRTktNTYtM0YtNzAtOTEtNDAtOTUtNkMtMEItRkUtRkMtOEItNjItNTQtMDItRDUtM0MtQTYtMDMtNDgtMzAtQjEtNzktMDMtM0QtMjYtQkQtRDItODgtNkMtRTktMzMtNzctMTYtNkMtNzUtNjAtNzItODUtQTYtQUQtMEItRjgtQTgtMUUtMzEtOTItRDAtMUUtMEUtRTItQzQtMUUtN0YtMkEtMzAtQTYtRDAtNDYtRjQtRTUtMTEtNTgtOTEtNzYtNjgtQTctODUtRjMtQUItMTAtQUEtRDgtNzYtOTktM0UtMEItRDItODktRUMtNjgtNUMtN0MtOEEtRDAtQUItODUtMzMtMDktRDEtMzMtMTEtREUtNUYtMzQtNzMtNjctMTEtNUEtQjUtRkMtMzctODItMjUtNTEtNjQtMEQtRkItNjMtRUMtOUQtNzgtQTAtMkMtREEtMjktMTItMDktMkQtQkUtNDAtQTYtQjQtODgtOEYtMzQtODUtMkYtMkYtOTItODctN0MtM0QtMDMtQTQtODItNzUtMjgtOTUtNjktM0EtNUMtQjctQTYtODYtNTUtMTg=";
    const s_r_client = new Single_R_Client();
    await s_r_client.set_up({ path: "/MineSocket", jwt: customer_jwt });
    const socket_customer = new Customer_Single_R({ controller: s_r_client });
    // Listeners
    s_r_client.On_Error(data => {
        console.error(data.msg);
        console.warn(data);
    });
    socket_customer.On_Tec_Close_Report(data => {
        console.log(data);
    });
    socket_customer.On_Admin_Close_Report(data => {
        alert(`my report with id ${data.data.report.id} has peen closed`);
        console.log();
    });
    socket_customer.On_Give_To_Tec(data => {
        console.log(data);
    });
    // Senders
    // await socket_customer.Open_Report({
    //     lat:10.35558712,
    //     long:20.6457258,
    //     type:"yofi"
    // })
};
main_2();
