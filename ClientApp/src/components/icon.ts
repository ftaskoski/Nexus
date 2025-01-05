import type { LooseAutoComplete } from './types'
export type IconName = LooseAutoComplete< 'message' | 'email' | 'lock' | 'hidden-password' | 'visible-password' | 'loading' | 'cancel' | 'none' |'error' | 'user'>

interface Icon {
  name: IconName
  path: string[]
}

export const icons: Icon[] = [
  {
    name: 'none',
    path: [],
  },
  {
    name: 'message',
    path: ['M8 12h.01M12 12h.01M16 12h.01M21 12c0 4.418-4.03 8-9 8a9.863 9.863 0 01-4.255-.949L3 20l1.395-3.72C3.512 15.042 3 13.574 3 12c0-4.418 4.03-8 9-8s9 3.582 9 8z'],
  },
  {
    name: 'email',
    path: ['M2.003 5.884L10 9.882l7.997-3.998A2 2 0 0016 4H4a2 2 0 00-1.997 1.884z', 'M18 8.118l-8 4-8-4V14a2 2 0 002 2h12a2 2 0 002-2V8.118z'],
  },
  {
    name: 'lock',
    path: ['M5 9V7a5 5 0 0110 0v2a2 2 0 012 2v5a2 2 0 01-2 2H5a2 2 0 01-2-2v-5a2 2 0 012-2zm8-2v2H7V7a3 3 0 016 0z'],
  },
  {
    name: 'hidden-password',
    path: ['M13.875 18.825A10.05 10.05 0 0112 19c-4.478 0-8.268-2.943-9.543-7a9.97 9.97 0 011.563-3.029m5.858.908a3 3 0 114.243 4.243M9.878 9.878l4.242 4.242M9.88 9.88l-3.29-3.29m7.532 7.532l3.29 3.29M3 3l3.59 3.59m0 0A9.953 9.953 0 0112 5c4.478 0 8.268 2.943 9.543 7a10.025 10.025 0 01-4.132 5.411m0 0L21 21'],
  },
  {
    name: 'visible-password',
    path: ['M15 12a3 3 0 11-6 0 3 3 0 016 0z', 'M2.458 12C3.732 7.943 7.523 5 12 5c4.478 0 8.268 2.943 9.542 7-1.274 4.057-5.064 7-9.542 7-4.477 0-8.268-2.943-9.542-7z'],
  },
  {
    name: 'loading',
    path: ['M4 12a8 8 0 018-8V0C5.373 0 0 5.373 0 12h4zm2 5.291A7.962 7.962 0 014 12H0c0 3.042 1.135 5.824 3 7.938l3-2.647z'],
  },
  {
    name: 'cancel',
    path: ['M6 18L18 6M6 6l12 12'],
  },
  {
    name: 'error',
    path: ['M12 7v6M12 17.01l.01-.011M12 22c5.523 0 10-4.477 10-10S17.523 2 12 2 2 6.477 2 12s4.477 10 10 10z'],
  },
  {
    name: 'user',
    path: [
      'M12 12c2.21 0 4-1.79 4-4s-1.79-4-4-4-4 1.79-4 4 1.79 4 4 4z',
      'M6 18c0-3.31 2.69-6 6-6s6 2.69 6 6H6z',
    ],
  },
];

