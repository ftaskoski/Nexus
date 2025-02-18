import { fetchy } from "@/plugins/axios";
export async function getFriendsData() {
    const res = await fetchy({
      url: "friends",
      method: "GET",
    });
  
    return res;
  }
  