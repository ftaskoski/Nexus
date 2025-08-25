import { createApp } from 'vue'
import App from './App.vue'
import SignalRPlugin from './plugins/signalr'
import router from './router'
import './assets/main.css'

const app = createApp( App )
app.use( router )
app.use( SignalRPlugin )
app.mount( '#app' )
