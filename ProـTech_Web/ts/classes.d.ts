

class ReportsModle
{
    public id:number
    public tec_Id:number
    public customer_Id:number
    public status:number
    public opened_At:Date
    public closed_At:Date | null
    public lat_Long_Location:[string | number, string | number] | string
    public type:string
    public close_Code:number
}
class Report_TypesModle
{
    public id:number
    public name:string
}

class UsersModle
{
    public id
    public full_Name:string
    public email:string
    public password:string
    public phone:string
    public u_Role:User_Roles
    public a_Roles:string | Array<Admin_Roles>
}

interface Kay_Value {
    key:string,
    value:string
}