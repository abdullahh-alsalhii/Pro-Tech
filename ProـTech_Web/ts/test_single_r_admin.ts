const main = async () => {
    const s_r_client = new Single_R_Client()
    await s_r_client.set_up({path:"/MineSocket"})
    s_r_client.On_Error(data => {
        console.error(data.msg)
        console.warn(data)
    })

    const socket_admin = new Admin_Single_R({controller:s_r_client})

    // Listeners
    socket_admin.On_Open_Report(data => {
        console.log(data)
    })
    socket_admin.On_Tec_Close_Report(data => {
        console.log(data)
    })
    socket_admin.On_Admin_Close_Report(data => {
        console.log(data)
    })
    socket_admin.On_Give_To_Tec(data => {
        console.log(data)
    })
    
    // Senders
    await socket_admin.Admin_Close_Report({id:1})
    // socket_admin.Give_To_Tec({
    //     id:7,
    //     tec_id:124,
    // })
}
main()