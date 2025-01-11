import * as signalR from '@microsoft/signalr';

const connection = new signalR.HubConnectionBuilder()
    .withUrl("/hubs/notifications", {
        withCredentials: true,
        skipNegotiation: false,
        transport: signalR.HttpTransportType.WebSockets
    })
    .withAutomaticReconnect([0, 2000, 10000, 30000])
    .build();

const SignalRPlugin = {
    install(app: any) {
        connection.start()


        app.config.globalProperties.$signalR = connection;
        app.provide('signalR', connection);
    }
};

export default SignalRPlugin;