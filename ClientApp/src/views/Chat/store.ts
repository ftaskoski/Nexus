import { fetchy } from "@/plugins/axios";
export async function getFriend(friendId: string | null) {
  const res = await fetchy({
    url: `/friends/${friendId}`,
    method: "GET",
  });
  return res;
}
