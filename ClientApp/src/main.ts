import './assets/main.css'
import { createApp } from 'vue'
import App from './App.vue'
import router from './router'
import SignalRPlugin from './plugins/signalr'

const app = createApp(App)
app.use(router)
app.use(SignalRPlugin)
app.mount('#app')
