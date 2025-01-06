export type LooseAutoComplete<T extends string> = T | Omit<string, T>

export type UtilsSize = 'xxl' | 'xl' | 'l' | 'm' | 's' | 'xs'

export type NavItem = {
    id: string,
    label: string,
    icon: string,
    to?: string
}

