import { fetchy } from "@/plugins/axios";
export async function getFriendsData() {
    const res = await fetchy({
        url: 'friendrequests/friends/online',
        method: 'GET',
      })
      return res;
  }