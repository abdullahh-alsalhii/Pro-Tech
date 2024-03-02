
const ram = {
    from:0,
    to:5,
    search_value:() => (Fx.querySelector("#serach_bar") as HTMLInputElement).value
}

const  Render_list_to_table = async({
    from,
    to,
    search
}:{
    from:number,
    to:number,
    search:string
}) => {
    var reports_list
    try{
        reports_list = await Api.Get_Rports_Lest({
            from,
            to,
            search
        })
    } catch(err){
        var e = err as Res_Un_T
        if(e.isDone === false){
            Fx.alert(e.msg, false)
        }
        return;
    }

    
    const keys_list = ["ID", "صاحب الطلب", "الفني", "تاريخ الطلب",  "تاريخ الإغلاق", "حالة الطلب", "نوع الطلب", "موقع الطلب"]
    type the_data_we_nead = {
        id:number,
        customer:string
        tec:string | null
        opened_At:Date
        closed_At:Date | null
        status:Reports_Status
        type:string
        location:string
    };
    var sorted_obj = Render_Controller.Sort_Obj<the_data_we_nead, {
        report:ReportsModle
        customer:UsersModle
        tec:UsersModle | null
    }>(reports_list.data!, (data) => {
        console.log(data.tec, data.report.id)
        return {
            id:data.report.id,
            customer:data.customer.full_Name.split(" ")[0],
            tec:!data.tec ? data.tec : data.tec.full_Name.split(" ")[0],
            opened_At:data.report.opened_At,
            closed_At:data.report.closed_At,
            status:data.report.status,
            type:data.report.type,
            location:data.report.lat_Long_Location as string,
        }
    })
    Render_Controller.Render_Obj_List(sorted_obj, keys_list)
    Render_Controller.Set_Colmun_Glass(7, "map_viewr", "إظهار الموقع", (data) => {
        const lat_long = JSON.parse(data) as [number, number]
        const google_uri = `https://maps.google.com/maps?q=${lat_long[0]},${lat_long[1]}&z=15&output=embed`
        const iframe = Fx.querySelector("#map_viewr iframe") as HTMLIFrameElement
        iframe.src = google_uri
    })
    Render_Controller.change_value(5, (data, elm) => {
        elm.textContent = Reports_Status_list[Number(data)]
    })
}
console.log("nooo !");
const socket = async() => {
    const s_r_client = new Single_R_Client()
    await s_r_client.set_up({path:"/MineSocket"})
    s_r_client.On_Error(data => {
        console.log(data)
        Fx.alert(data.msg, data.isDone)
    })

    const socket_admin = new Admin_Single_R({controller:s_r_client})
    socket_admin.On_Give_To_Tec(data => {
        Fx.alert(data.msg, data.isDone)
    })
    socket_admin.On_Admin_Close_Report(data => {
        Fx.alert(data.msg, data.isDone)
    })
    
    const give_to_tec_button = Fx.querySelector("#give_to_tec")!
    const close_report_button = Fx.querySelector("#close_report_button")!
    const give_to_tec_func = () => {
        var id_input = give_to_tec_button.parentElement!.querySelector("input[type=hidden]") as HTMLInputElement
        var tec_id_input = give_to_tec_button.parentElement!.querySelector(".forms input") as HTMLInputElement
        
        give_to_tec_button.onclick = () => {
            if(!tec_id_input?.value){
                Fx.alert("أكمل الحقل !!!", false)
                return
            }
            socket_admin.Give_To_Tec({
                id:Number(id_input.value),
                tec_id:Number(tec_id_input.value)
            })
        }
    }
    const close_report_func = () => {
        var id_input = close_report_button.parentElement!.querySelector("input[type=hidden]") as HTMLInputElement
        
        close_report_button.onclick = () => {
            socket_admin.Admin_Close_Report({
                id:Number(id_input.value),
            })
        }
    }
    give_to_tec_func()
    close_report_func()

}

(async() => {
    await socket()
    await Render_list_to_table({
        from:0, to:5, search:""
    })
    Render_Controller.On_More_Button_Clicked(async () => {
        ram.from += 5,
        ram.to += 5
        await Render_list_to_table({
            from:ram.from, to:ram.to, search:ram.search_value()
        })
    })
    Render_Controller.On_Search_Button_Clicked(async() => {
        ram.from = 0,
        ram.to = 5
        await Render_list_to_table({
            from:ram.from, to:ram.to, search:ram.search_value()
        })
    })
})();
