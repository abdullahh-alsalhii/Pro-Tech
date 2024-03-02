"use strict";
class Single_R_Client {
    connection;
    jwt;
    async set_up({ path, jwt }) {
        this.connection = new signalR.HubConnectionBuilder().withUrl(path).build();
        await this.connection.start();
        this.jwt = jwt;
        this.send("SaveUser", {
            Jwt: jwt
        });
    }
    async send(event, data) {
        await this.connection.invoke(event, data);
    }
    async send_any(event, data) {
        await this.connection.invoke(event, data);
    }
    on(event, func) {
        this.connection.on(event, (data) => {
            func(data);
        });
    }
    async Send_Error(msg) {
        await this.send("Erorr", {
            isDone: false,
            msg: msg
        });
    }
    On_Error(func) {
        this.on("Erorr", func);
    }
}
class Customer_Single_R {
    controller;
    constructor({ controller }) {
        this.controller = controller;
    }
    On_Tec_Close_Report(func) {
        this.controller.on("Tec_Close_Report", func);
    }
    On_Admin_Close_Report(func) {
        this.controller.on("Admin_Close_Report", func);
    }
    On_Give_To_Tec(func) {
        this.controller.on("Give_To_Tec", func);
    }
    async Open_Report({ lat, long, type }) {
        if (isNaN(Number(`${lat}`)) || isNaN(Number(`${long}`))) {
            await this.controller.Send_Error("ther is a problem with lat and lang");
            return;
        }
        await this.controller.send("Open_Report", {
            Jwt: this.controller.jwt,
            Data: {
                Type: type,
                Lat_Long_Location: `[${lat}, ${long}]`
            }
        });
    }
}
class Admin_Single_R {
    controller;
    constructor({ controller }) {
        this.controller = controller;
    }
    On_Open_Report(func) {
        this.controller.on("Open_Report", func);
    }
    On_Tec_Close_Report(func) {
        this.controller.on("Tec_Close_Report", func);
    }
    On_Admin_Close_Report(func) {
        this.controller.on("Admin_Close_Report", func);
    }
    On_Give_To_Tec(func) {
        this.controller.on("Give_To_Tec", func);
    }
    async Admin_Close_Report({ id }) {
        this.controller.send_any("Admin_Close_Report", {
            Id: id
        });
    }
    async Give_To_Tec({ id, tec_id }) {
        this.controller.send_any("Give_To_Tec", {
            Id: id,
            Tec_Id: tec_id
        });
    }
}
class Tec_Single_R {
    controller;
    constructor({ controller }) {
        this.controller = controller;
    }
    On_Admin_Close_Report(func) {
        this.controller.on("Admin_Close_Report", func);
    }
    On_Give_To_Tec(func) {
        this.controller.on("Give_To_Tec", func);
    }
    async Tec_Close_Report(data) {
        await this.controller.send("Tec_Close_Report", {
            Jwt: this.controller.jwt,
            Data: data
        });
    }
}
