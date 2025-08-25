import * as signalR from '@microsoft/signalr'

const connection1 = new signalR.HubConnectionBuilder()
  .withUrl( '/hubs/onlineUsers', {
    withCredentials: true,
    skipNegotiation: false,
    transport:       signalR.HttpTransportType.WebSockets
  })
  .withAutomaticReconnect( [ 0, 2000, 10000, 30000 ] )
  .build()

const connection2 = new signalR.HubConnectionBuilder()
  .withUrl( '/hubs/notifications', {
    withCredentials: true,
    skipNegotiation: false,
    transport:       signalR.HttpTransportType.WebSockets
  })
  .withAutomaticReconnect( [ 0, 2000, 10000, 30000 ] )
  .build()

const messageConnection = new signalR.HubConnectionBuilder()
  .withUrl( '/hubs/messages', {
    withCredentials: true,
    skipNegotiation: false,
    transport:       signalR.HttpTransportType.WebSockets
  })
  .withAutomaticReconnect( [ 0, 2000, 10000, 30000 ] )
  .build()

const SignalRPlugin = {
  install( app: any ) {
    connection1.start()
      .catch( err => console.error( 'Error starting connection1: ', err ))
    connection2.start()
      .catch( err => console.error( 'Error starting connection2: ', err ))
    messageConnection.start()
      .catch( err => console.error( 'Error starting messageConnection: ', err ))

    app.config.globalProperties.$signalR1 = connection1
    app.config.globalProperties.$signalR2 = connection2
    app.config.globalProperties.$messageSignalR = messageConnection

    app.provide( 'signalR1', connection1 )
    app.provide( 'signalR2', connection2 )
    app.provide( 'messageSignalR', messageConnection )
  }
}

export default SignalRPlugin
