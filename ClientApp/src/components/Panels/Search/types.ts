export enum FriendRequestStatus {
  Pending = 0,
  Accepted = 1,
  Rejected = 2
}

export interface UserSearchResult {
  id:         string
  username:   string
  status:     FriendRequestStatus | null
  isIncoming: boolean
}
