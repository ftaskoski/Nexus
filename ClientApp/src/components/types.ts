export type LooseAutoComplete<T extends string> = T | Omit<string, T>

export type UtilsSize = 'xxl' | 'xl' | 'l' | 'm' | 's' | 'xs'

export interface NavItem  { 
    id: string
    icon: string
    label: string
    to?: string
    hasPanel?: boolean
  }

export type PanelType = 'search' | 'notifications' | 'messages';
