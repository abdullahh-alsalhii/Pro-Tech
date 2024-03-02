"use strict";
class Reports_Status {
    static Closed_By_Customer = 1;
    static Closed_By_Admin = 2;
    static Open = 3;
}
const Reports_Status_list = ["", "أغلق من قبل الزبون", "مغلق من قبل الأدمن", "مفتوح"];
class Admin_Roles {
    static Create_User = 1;
    static Delete_User = 2;
    static Update_User = 3;
    static Edit_User_Roles = 4;
    static Manage_Reports = 5;
    static View_Reports = 6;
}
class User_Roles {
    static User = 1;
    static Admin = 2;
    static Tec = 3;
}
const User_Roles_List = ["", "مستخدم", "أدمن", "فني"];
class Runner {
    static Main() {
        FormController.Main();
        Api.Relate_AllForms();
    }
}
class Fx {
    static is_alert_on = false;
    static async sleep(dlay) {
        await new Promise((r) => {
            setTimeout(() => {
                r(true);
            }, dlay);
        });
    }
    static querySelectorAll(selector) {
        return Array.from(document.querySelectorAll(selector));
    }
    static querySelector(selector) {
        return document.querySelector(selector);
    }
    static alert(data, is_ok) {
        if (this.is_alert_on) {
            console.log("QDW;uh");
            return;
        }
        console.log("alretde");
        var alert_box = this.querySelector("#alert_note");
        alert_box.style.zIndex = "20";
        alert_box.querySelector("p").textContent = data;
        var icon = alert_box.querySelector("i");
        alert_box.style.background = is_ok ? "rgb(0, 127, 8)" : "rgb(223, 0, 0)";
        icon.className = is_ok ? "fa-solid fa-check" : "fa-solid fa-circle-xmark";
        this.sleep(1000).then(() => {
            alert_box.style.translate = "0 0";
        });
        this.sleep(5000).then(() => {
            alert_box.style.translate = "200% 0";
            this.sleep(1000).then(() => {
                this.is_alert_on = false;
            });
        });
    }
}
console.log("noooo");
class Api {
    static async Send_Req(path, body) {
        console.log(body);
        let req = await fetch(path, {
            method: "POST",
            body: JSON.stringify(body),
            headers: {
                "Content-Type": "application/json"
            }
        });
        var res = await req.text();
        return JSON.parse(res);
    }
    static async Get_Rports_Lest({ from, search, to }) {
        var data = await this.Send_Req("/reports/api_get_List", {
            from,
            to,
            search,
        });
        return data;
    }
    static async Get_reports_types_Lest({ from, search, to }) {
        var data = await this.Send_Req("/reports_types/api_get_list", {
            from,
            to,
            search,
        });
        return data;
    }
    static async Get_User_List({ from, search, to, role }) {
        var data = await this.Send_Req(`/Users/Api_Get_List_Role?role=${role}`, {
            from,
            to,
            search,
        });
        return data;
    }
    static async Get_User_List_2({ from, search, to, role, role2 }) {
        var data = await this.Send_Req(`/Users/Api_Get_List_Role?role=${role}&role2=${role2}`, {
            from,
            to,
            search,
        });
        return data;
    }
    static async Get_Tec_List({ from, search, to }) {
        return this.Get_User_List_2({ from, to, search, role: User_Roles.Tec, role2: User_Roles.Admin });
    }
    static async Get_Customer_List({ from, search, to }) {
        return this.Get_User_List({ from, to, search, role: User_Roles.User });
    }
    static Relate_AllForms = () => {
        Fx.querySelectorAll(".input_controller form").forEach(form => {
            var the_form = form;
            const action_url = the_form.action.replace(`${window.location.protocol}//${window.location.host}`, "");
            the_form.onsubmit = (e) => {
                e.preventDefault();
                const fromdata = new FormData(form);
                $.ajax({
                    url: action_url,
                    data: fromdata,
                    method: "POST",
                    contentType: false,
                    success: (data) => {
                        Fx.alert(data.msg, true);
                    },
                    error: (data) => {
                        Fx.alert(data.msg, false);
                    },
                    processData: false,
                });
            };
        });
    };
}
class Render_Controller {
    static record_index = 0;
    static buutons_indexes = [];
    static render_obj(tbody, box_id, database_id, data) {
        var colmon_index = 0;
        const render_td = (data, father_element, is_it_head) => {
            const value_element = document.createElement(is_it_head ? "th" : "td");
            value_element.textContent = data;
            if (!is_it_head) {
                value_element.dataset.data = data;
                value_element.dataset.id = `${database_id}`;
                value_element.dataset.index = `${colmon_index}`;
                value_element.classList.add(`colmon_index-${colmon_index}`);
            }
            father_element.appendChild(value_element);
        };
        const head = document.createElement("tr");
        const body = document.createElement("tr");
        data.forEach(obj => {
            render_td(obj.key, head, true);
            render_td(obj.value, body);
            colmon_index++;
            if (box_id) {
                body.onclick = (e) => {
                    var elm_traget = e.target;
                    if (!this.buutons_indexes.includes(Number(elm_traget.dataset.index))) {
                        FormController.With_box(body, box_id);
                    }
                };
            }
        });
        tbody.appendChild(head);
        tbody.appendChild(body);
        body.dataset.id = `${database_id}`;
        body.dataset.index = `${this.record_index}`;
        body.classList.add(`record_index-${this.record_index}`);
        this.record_index++;
    }
    static More_Button_Load() {
        let more_button = Fx.querySelector("#more_button");
        let more_loding = Fx.querySelector("#more_loding");
        more_button.style.display = "none";
        more_loding.style.display = "block";
    }
    static More_Button_Stable() {
        let more_button = Fx.querySelector("#more_button");
        let more_loding = Fx.querySelector("#more_loding");
        more_loding.style.display = "none";
        more_button.style.display = "block";
    }
    static Sort_Obj(data_list, func) {
        let final_data = [];
        data_list.forEach(data => {
            const sorted_data = func(data);
            final_data.push(sorted_data);
        });
        return final_data;
    }
    static Render_Obj_List(data_list, keys_list) {
        var table = Fx.querySelector("#customers tbody");
        for (let i = 0; i < data_list.length; i++) {
            var final_list = [];
            for (let j = 0; j < keys_list.length; j++) {
                var value = Object.values(data_list[i])[j];
                final_list.push({
                    key: keys_list[j],
                    value
                });
            }
            Render_Controller.render_obj(table, "table_glass", data_list[i].id, final_list);
        }
    }
    static Set_Colmun_Glass(index, glass_id, button_name, func) {
        Fx.querySelectorAll(`.colmon_index-${index}`).forEach(elm => {
            elm.style.textDecoration = "underline";
            elm.style.color = "#2a77ae";
            elm.textContent = button_name;
            this.buutons_indexes.push(index);
            elm.onclick = () => {
                FormController.With_box(elm, glass_id);
                func(elm.dataset.data, elm);
            };
        });
    }
    static change_value = (index, func) => {
        Fx.querySelectorAll(`.colmon_index-${index}`).forEach(elm => {
            func(elm.dataset.data, elm);
        });
    };
    static On_More_Button_Clicked(func) {
        let more_button = Fx.querySelector("#more_button");
        more_button.onclick = () => {
            this.More_Button_Load();
            Fx.sleep(1000).then(() => {
                func().then(() => {
                    this.More_Button_Stable();
                }).catch((err) => {
                    this.More_Button_Stable();
                    Fx.alert("حدث خطأ ما", false);
                    console.log(err);
                });
            });
        };
    }
    static On_Search_Button_Clicked(func) {
        let more_button = Fx.querySelector("#search_button");
        var table = Fx.querySelector("#customers tbody");
        more_button.onclick = () => {
            Array.from(table.children).forEach(elm => {
                elm.remove();
            });
            this.More_Button_Load();
            Fx.sleep(1000).then(() => {
                func().then(() => {
                    this.More_Button_Stable();
                }).catch((err) => {
                    this.More_Button_Stable();
                    Fx.alert("حدث خطأ ما", false);
                    console.log(err);
                });
            });
        };
    }
}
class FormController {
    static all_form_controllers = Fx.querySelectorAll(".input_controller");
    static Main() {
        this.Init_MakeCloseButtonWork();
    }
    static With_box(button, controller_id) {
        let box = Fx.querySelector(`#${controller_id}`);
        this.all_form_controllers.forEach(controller => {
            controller.style.zIndex = `10`;
        });
        if (box) {
            box.style.zIndex = `11`;
            let id_input = box.querySelector("input[type=hidden]");
            if (button.dataset?.id) {
                box.querySelectorAll("button").forEach(galss_button => {
                    galss_button.dataset.id = button.dataset.id;
                });
            }
            if (button.dataset?.id && id_input) {
                id_input.value = button.dataset.id;
            }
            box.style.display = "flex";
        }
    }
    static Init_MakeCloseButtonWork() {
        const all_x = Fx.querySelectorAll(".x");
        all_x.forEach(x => {
            x.onclick = () => {
                this.all_form_controllers
                    .forEach(controller => {
                    controller.style.display = "none";
                });
            };
        });
        this.all_form_controllers.forEach(controller => {
            controller.onclick = (e) => {
                let target = e.target;
                if (target.classList.contains("input_controller")) {
                    controller.style.display = "none";
                }
            };
        });
    }
}
Runner.Main();
