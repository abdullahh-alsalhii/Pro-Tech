"use strict";
const ram_1 = {
    from: 0,
    to: 5,
    search_value: () => Fx.querySelector("#serach_bar").value
};
const Render_list_to_e_table = async ({ from, to, search }) => {
    var reports_list;
    try {
        reports_list = await Api.Get_Tec_List({
            from,
            to,
            search
        });
    }
    catch (err) {
        var e = err;
        console.log(err);
        if (e.isDone === false) {
            Fx.alert(e.msg, false);
        }
        return;
    }
    const keys_list = ["ID", "إسم الموضف", "رقم الجوال", "الإيميل", "رتبة الموضف"];
    var sorted_obj = Render_Controller.Sort_Obj(reports_list.data, (data) => {
        return {
            id: data.id,
            full_Name: data.full_Name,
            phone: data.phone,
            email: data.email,
            u_Role: data.u_Role,
            a_Roles: data.a_Roles,
            password: data.password
        };
    });
    Render_Controller.Render_Obj_List(sorted_obj, keys_list);
    Render_Controller.change_value(4, (data, elm) => {
        elm.textContent = User_Roles_List[Number(data)];
    });
};
(async () => {
    await Render_list_to_e_table({
        from: 0, to: 5, search: ""
    });
    Render_Controller.On_More_Button_Clicked(async () => {
        ram_1.from += 5,
            ram_1.to += 5;
        await Render_list_to_e_table({
            from: ram_1.from, to: ram_1.to, search: ram_1.search_value()
        });
    });
    Render_Controller.On_Search_Button_Clicked(async () => {
        ram_1.from = 0,
            ram_1.to = 5;
        console.log(ram_1.search_value());
        await Render_list_to_e_table({
            from: ram_1.from, to: ram_1.to, search: ram_1.search_value()
        });
    });
})();
console.log("w");
