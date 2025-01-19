import {ref} from "vue";
import type { Notification } from './types';
import { fetchy } from "@/plugins/axios";
export const notificationsCount = ref<number>(0)

export const notifications = ref<Notification[]>([]);


export const handleNotification = (notification: Notification) => {
    notifications.value.push(notification);
    notificationsCount.value = notifications.value.length;
  
  };

  export async function getNotifications() { 
    const res = await fetchy({
      url: 'friendrequests/notifications',
      method: 'GET',
    });
    return {
      notifications: res.payload,
      count: res.payload.length
    }
  }