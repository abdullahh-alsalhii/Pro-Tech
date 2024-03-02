class Reports_Status
{
    public static Closed_By_Customer = 1;
    public static Closed_By_Admin = 2;
    public static Open = 3;
}
const Reports_Status_list =  ["", "أغلق من قبل الزبون" , "مغلق من قبل الأدمن", "مفتوح"];
class Admin_Roles {
    public static Create_User = 1
    public static Delete_User = 2
    public static Update_User = 3
    public static Edit_User_Roles = 4
    public static Manage_Reports = 5
    public static View_Reports = 6
}
class User_Roles {
    public static User = 1;
    public static Admin = 2;
    public static Tec = 3;
}
const User_Roles_List = ["", "مستخدم", "أدمن", "فني"]

class Runner {
    public static Main(){
        FormController.Main();
        Api.Relate_AllForms();
    }
}

class Fx {
    private static is_alert_on = false;
    public static async sleep(dlay:number){
        await new Promise((r) => {
            setTimeout(() => {
                r(true)
            }, dlay)
        })
    }
    public static querySelectorAll(selector:string):HTMLElement[]{
        return Array.from(document.querySelectorAll(selector)) as HTMLElement[]
    }
    public static querySelector(selector:string):HTMLElement | undefined{
        return document.querySelector(selector) as HTMLElement | undefined
    }
    public static alert(data:string, is_ok:boolean){
        if(this.is_alert_on){
            console.log("QDW;uh")
            return
        }
        console.log("alretde")
        var alert_box = this.querySelector("#alert_note")!
        alert_box.style.zIndex = "20"
        alert_box.querySelector("p")!.textContent = data
        var icon = alert_box.querySelector("i") as HTMLElement
        alert_box.style.background = is_ok ? "rgb(0, 127, 8)" : "rgb(223, 0, 0)"
        icon.className = is_ok ? "fa-solid fa-check" : "fa-solid fa-circle-xmark"
        this.sleep(1000).then(() => {
            alert_box.style.translate ="0 0";
        })
        this.sleep(5000).then(() => {
            alert_box.style.translate ="200% 0";
            this.sleep(1000).then(() => {
                this.is_alert_on = false
            })
        })
    }
}
console.log("noooo")
class Api {
    public static async Send_Req<T, Y>(path:string, body:Y):Promise<T>{
        console.log(body)
        let req = await fetch(path, {
            method:"POST",
            body:JSON.stringify(body),
            headers:{
                "Content-Type":"application/json"
            }
        })
        var res = await req.text()
        return JSON.parse(res);
    }
    public static async Get_Rports_Lest({from, search, to}:{
        from:number
        to:number
        search:string
    }){
        var data = await this.Send_Req<Res<{
            report:ReportsModle
            customer:UsersModle
            tec:UsersModle | null
        }[]>, any>("/reports/api_get_List", {
            from,
            to, 
            search,
        })
        return data
    }
    public static async Get_reports_types_Lest({from, search, to}:{
        from:number
        to:number
        search:string
    }){
        var data = await this.Send_Req<Res<Report_TypesModle[]>, any>("/reports_types/api_get_list", {
            from,
            to, 
            search,
        })
        return data
    }
    private static async Get_User_List({from, search, to, role}:{
        from:number
        to:number
        search:string,
        role:User_Roles
    }){
        var data = await this.Send_Req<Res<UsersModle[]>, any>(`/Users/Api_Get_List_Role?role=${role}`, {
            from,
            to, 
            search,
        })
        return data
    }
    private static async Get_User_List_2({from, search, to, role, role2}:{
        from:number
        to:number
        search:string,
        role:User_Roles,
        role2:User_Roles,
    }){
        var data = await this.Send_Req<Res<UsersModle[]>, any>(`/Users/Api_Get_List_Role?role=${role}&role2=${role2}`, {
            from,
            to, 
            search,
        })
        return data
    }
    public static async Get_Tec_List({from, search, to}:{
        from:number
        to:number
        search:string
    }){
        return this.Get_User_List_2({from, to, search, role:User_Roles.Tec, role2:User_Roles.Admin})
    }
    public static async Get_Customer_List({from, search, to}:{
        from:number
        to:number
        search:string
    }){
        return this.Get_User_List({from, to, search, role:User_Roles.User})
    }
    public static Relate_AllForms = () => {
        Fx.querySelectorAll(".input_controller form").forEach(form => {
            var the_form = form as HTMLFormElement
            const action_url = the_form.action.replace(`${window.location.protocol}//${window.location.host}`, "")
            the_form.onsubmit = (e) => {
                e.preventDefault()
                const fromdata = new FormData(form as HTMLFormElement)
                $.ajax({
                    url:action_url,
                    data:fromdata,
                    method:"POST",
                    contentType: false,
                    success:(data:any) => {
                        Fx.alert(data.msg, true)
                    },
                    error:(data:any) => {
                        Fx.alert(data.msg, false)
                    },
                    processData: false,
                })
            }
        })
    }
}

class Render_Controller {
    private static record_index = 0;
    private static buutons_indexes:number[] = []
    public static render_obj<T>(tbody:HTMLElement, box_id:string | null, database_id:number ,data:{
        key:string,
        value:string
    }[]){
        var colmon_index = 0;
        const render_td = (data:string, father_element:HTMLElement, is_it_head?:boolean) => {
            const value_element = document.createElement(is_it_head ? "th" : "td")
            value_element.textContent = data
            if(!is_it_head){
                value_element.dataset.data = data;
                value_element.dataset.id = `${database_id}`
                value_element.dataset.index = `${colmon_index}`
                value_element.classList.add(`colmon_index-${colmon_index}`)
            }
            
            father_element.appendChild(value_element)
        }
        const head = document.createElement("tr")
        const body = document.createElement("tr")
        data.forEach(obj => {
            render_td(obj.key, head, true)
            render_td(obj.value, body)
            colmon_index++
            if(box_id){
                body.onclick = (e) => {
                    var elm_traget = e.target as HTMLElement
                    if(!this.buutons_indexes.includes(Number(elm_traget.dataset.index))){
                        FormController.With_box(body, box_id)
                    }
                }
            }
        })
        
        tbody.appendChild(head)
        tbody.appendChild(body)
        body.dataset.id = `${database_id}`
        body.dataset.index = `${this.record_index}`
        body.classList.add(`record_index-${this.record_index}`)
        this.record_index++;
    }
    public static More_Button_Load(){
        let more_button = Fx.querySelector("#more_button")!
        let more_loding = Fx.querySelector("#more_loding")!
        more_button.style.display = "none"
        more_loding.style.display = "block"
    }
    public static More_Button_Stable(){
        let more_button = Fx.querySelector("#more_button")!
        let more_loding = Fx.querySelector("#more_loding")!
        more_loding.style.display = "none"
        more_button.style.display = "block"
    }
    public static Sort_Obj<T, Y>(data_list:Y[], func:(obj:Y) => T):T[] {
        let final_data:T[] = []
        data_list.forEach(data => {
            const sorted_data = func(data)
            final_data.push(sorted_data)
        })
        return final_data
    }
    public static Render_Obj_List<T>(data_list:T[], keys_list:string[]){
        var table = Fx.querySelector("#customers tbody")!;
        for(let i = 0; i < data_list.length; i++){
            var final_list:Kay_Value[] = []
            for(let j = 0; j < keys_list.length; j++){
                var value = Object.values((data_list as any)[i])[j] as string
                final_list.push({
                    key:keys_list[j],
                    value
                })
            }
            Render_Controller.render_obj(table, "table_glass", (data_list[i] as any).id , final_list);
        }
    }
    public static Set_Colmun_Glass(index:number, glass_id:string, button_name:string, func:(data:string, elm:HTMLElement) => void){
        Fx.querySelectorAll(`.colmon_index-${index}`).forEach(elm => {
            elm.style.textDecoration = "underline"
            elm.style.color = "#2a77ae"
            elm.textContent = button_name
            this.buutons_indexes.push(index)
            elm.onclick = () => {
                FormController.With_box(elm, glass_id)
                func(elm.dataset!.data!, elm)
            }
        })
    }
    public static change_value = (index:number, func:(data:string, elm:HTMLElement) => void) => {
        Fx.querySelectorAll(`.colmon_index-${index}`).forEach(elm => {
            func(elm.dataset!.data!, elm)
        })
    }
    public static On_More_Button_Clicked(func:() => Promise<void>){
        let more_button = Fx.querySelector("#more_button")!
        more_button.onclick = () => {
            this.More_Button_Load()
            Fx.sleep(1000).then(() => {
                func().then(() => {
                    this.More_Button_Stable()
                }).catch((err) => {
                    
                    this.More_Button_Stable()
                    Fx.alert("حدث خطأ ما", false)
                    console.log(err)
                })
            })
        }
    }
    public static On_Search_Button_Clicked(func:() => Promise<void>){
        let more_button = Fx.querySelector("#search_button")!
        var table = Fx.querySelector("#customers tbody")!;
        more_button.onclick = () => {
            Array.from(table.children).forEach(elm => {
                elm.remove()
            })
            this.More_Button_Load()
            Fx.sleep(1000).then(() => {
                func().then(() => {
                    this.More_Button_Stable()
                }).catch((err) => {
                    this.More_Button_Stable()
                    Fx.alert("حدث خطأ ما", false)
                    console.log(err)
                })
            })
        }
    }
}

class FormController {
    private static all_form_controllers = Fx.querySelectorAll(".input_controller")

    public static Main(){
        this.Init_MakeCloseButtonWork()
    }

    public static With_box(button:HTMLElement, controller_id:string){
        let box = Fx.querySelector(`#${controller_id}`)
        this.all_form_controllers.forEach(controller => {
            controller.style.zIndex = `10`
        })
        if(box){
            box.style.zIndex = `11`
            let id_input = box.querySelector("input[type=hidden]") as HTMLInputElement
            if(button.dataset?.id){
                box.querySelectorAll("button").forEach(galss_button => {
                    galss_button.dataset.id = button.dataset.id
                })
            }
            if(button.dataset?.id && id_input){
                
                id_input.value = button.dataset.id
            }
            box!.style.display = "flex"
        }
    }

    public static Init_MakeCloseButtonWork(){
        const all_x = Fx.querySelectorAll(".x")

        all_x.forEach(x => {
            x.onclick = () => {
                this.all_form_controllers
                .forEach(controller => {
                    controller.style.display = "none"
                })
            }
            
        })

        this.all_form_controllers.forEach(controller => {
            controller.onclick = (e) => {
                let target = e.target as HTMLElement
                if(target.classList.contains("input_controller")){
                    controller.style.display = "none"
                }
            }
        })
    }

}
Runner.Main()