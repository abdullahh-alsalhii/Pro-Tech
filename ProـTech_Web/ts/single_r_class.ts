

interface Single_R_Client_Constructor {
    path:string,
    jwt?:string,
}

type Req_Events = "SaveUser" | "Dev_Print_Users" | "Erorr" | "Open_Report" | "Tec_Close_Report" | "Admin_Close_Report" | "Give_To_Tec"
type Res_Events =  "SaveUser" | "Dev_Print_Users" | "Erorr" | "Open_Report" | "Tec_Close_Report" | "Admin_Close_Report" | "Give_To_Tec"

class Single_R_Client {
    private connection:any
    public jwt?:string
    public async set_up({path, jwt}:Single_R_Client_Constructor){
        this.connection = new signalR.HubConnectionBuilder().withUrl(path).build();
        await this.connection.start()
        this.jwt = jwt
        this.send<Req_Un_T>("SaveUser", {
            Jwt:jwt
        })
    }
    public async send<T> (event:Req_Events, data?:Req<T> | Req_Un_T | Res_Un_T){
        await this.connection.invoke(event, data)
    }
    public async send_any<T> (event:Req_Events, data?:T){
        await this.connection.invoke(event, data)
    }
    public on<T> (event:Res_Events, func:(data:Res<T>) => void) {
        this.connection.on(event, (data:Res<T>) => {
            func(data)
        })
    }
    public async Send_Error(msg:string){
        await this.send<Res_Un_T>("Erorr", {
            isDone:false,
            msg:msg
        })
    }
    public  On_Error(func:(data:Res<null>) => void){
        this.on<null>("Erorr", func)
    }
}

class Customer_Single_R {
    public controller:Single_R_Client
    constructor({controller}:{controller:Single_R_Client}){
        this.controller = controller
    }

    public On_Tec_Close_Report(func:(data:Res<ReportsModle>) => void){
        this.controller.on<ReportsModle>("Tec_Close_Report", func)
    }
    public On_Admin_Close_Report(func:(data:Res<ReportsModle>) => void){
        this.controller.on<ReportsModle>("Admin_Close_Report", func)
    }
    public On_Give_To_Tec(func:(data:Res<ReportsModle>) => void){
        this.controller.on<ReportsModle>("Give_To_Tec", func)
    }

    public async Open_Report({lat, long, type}:{
        type:string,
        lat:string | number,
        long:string | number
    }){
        if(isNaN(Number(`${lat}`)) || isNaN(Number(`${long}`))){
            await this.controller.Send_Error("ther is a problem with lat and lang");
            return;
        }
        await this.controller.send("Open_Report", {
            Jwt:this.controller.jwt,
            Data: {
                Type:type,
                Lat_Long_Location:`[${lat}, ${long}]`
            }
        })
    }
}

class Admin_Single_R {
    public controller:Single_R_Client
    constructor({controller}:{controller:Single_R_Client}){
        this.controller = controller
    }
    public On_Open_Report(func:(data:Res<ReportsModle>) => void){
        this.controller.on<ReportsModle>("Open_Report", func)
    }
    public On_Tec_Close_Report(func:(data:Res<ReportsModle>) => void){
        this.controller.on<ReportsModle>("Tec_Close_Report", func)
    }
    public On_Admin_Close_Report(func:(data:Res<ReportsModle>) => void){
        this.controller.on<ReportsModle>("Admin_Close_Report", func)
    }
    public On_Give_To_Tec(func:(data:Res<ReportsModle>) => void){
        this.controller.on<ReportsModle>("Give_To_Tec", func)
    }

    public async Admin_Close_Report({id}:{id:number}){
        this.controller.send_any("Admin_Close_Report", {
            Id:id
        })
    }
    public async Give_To_Tec({id ,tec_id}:{id:number, tec_id:number}){
        this.controller.send_any("Give_To_Tec", {
            Id:id,
            Tec_Id:tec_id
        })
    }
}

class Tec_Single_R {
    public controller:Single_R_Client
    constructor({controller}:{controller:Single_R_Client}){
        this.controller = controller
    }
    public On_Admin_Close_Report(func:(data:Res<ReportsModle>) => void){
        this.controller.on<ReportsModle>("Admin_Close_Report", func)
    }
    public On_Give_To_Tec(func:(data:Res<ReportsModle>) => void){
        this.controller.on<ReportsModle>("Give_To_Tec", func)
    }

    public async Tec_Close_Report(data:{
        id:number,
        close_Code:number
    }) {
        await this.controller.send("Tec_Close_Report", {
            Jwt:this.controller.jwt,
            Data:data
        })
    }
}

interface Req<T> {
    Jwt?:string
    Data:T
}
interface Req_Un_T {
    Jwt?:string
}

interface Res_Un_T {
    isDone:boolean,
    msg:string
}
interface Res<T> {
    isDone:boolean,
    msg:string,
    data?:T
}
