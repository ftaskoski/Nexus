import { fetchy } from '@/plugins/axios'

export async function getFriend( friendId: string | null ) {
  const res = await fetchy({
    url:    `/friends/${friendId}`,
    method: 'GET',
  })
  return res
}

export async function sendMessage( receiverId: string | null, content: string, chatRoomId: string ) {
  return await fetchy({
    url:    '/chat/send',
    method: 'POST',
    data:   {
      receiverId,
      content,
      chatRoomId
    }
  })
}

export async function getMessages( chatRoomId: string | null, skip: number, take: number ) {
  const res = await fetchy({
    url:    `/chat/messages/${chatRoomId}?skip=${skip}&take=${take}`,
    method: 'GET',
  })
  return res
}
