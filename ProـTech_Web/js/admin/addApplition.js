"use strict";
const ram_3 = {
    from: 0,
    to: 5,
    search_value: () => Fx.querySelector("#serach_bar").value
};
const Render_list_to_t_table = async ({ from, to, search }) => {
    var reports_list;
    try {
        reports_list = await Api.Get_reports_types_Lest({
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
    const keys_list = ["ID", "إسم النوع"];
    var sorted_obj = Render_Controller.Sort_Obj(reports_list.data, (data) => {
        return {
            id: data.id,
            name: data.name
        };
    });
    Render_Controller.Render_Obj_List(sorted_obj, keys_list);
};
(async () => {
    await Render_list_to_t_table({
        from: 0, to: 5, search: ""
    });
    Render_Controller.On_More_Button_Clicked(async () => {
        ram_3.from += 5,
            ram_3.to += 5;
        await Render_list_to_t_table({
            from: ram_3.from, to: ram_3.to, search: ram_3.search_value()
        });
    });
    Render_Controller.On_Search_Button_Clicked(async () => {
        ram_3.from = 0,
            ram_3.to = 5;
        await Render_list_to_t_table({
            from: ram_3.from, to: ram_3.to, search: ram_3.search_value()
        });
    });
})();
