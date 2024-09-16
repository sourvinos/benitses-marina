import { Guid } from 'guid-typescript'

export interface UpdateUserDto {

    id: Guid
    username: string
    displayname: string
    email: string
    isFirstFieldFocused: boolean
    isAdmin: boolean
    isActive: boolean

}
