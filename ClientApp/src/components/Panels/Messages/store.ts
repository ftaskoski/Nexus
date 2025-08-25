import { fetchy } from '@/plugins/axios'

export async function getFriendsData() {
  const res = await fetchy({
    url:    'friends',
    method: 'GET',
  })

  return res
}

export async function getChatId( friendId: string ) {
  const res = await fetchy({
    url:    `chat/start/${friendId}`,
    method: 'GET',
  })
  return res
}
