export interface OnlineFriend {
  id: string;
  username: string;
  isOnline: boolean;
}

export interface RecentChat {
  id: string;
  isOnline: boolean;
  lastMessage: string;
  sentAt: string;
  username: string;
}
