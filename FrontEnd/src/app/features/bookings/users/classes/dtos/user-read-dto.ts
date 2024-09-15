import { Guid } from 'guid-typescript'
// Custom
import { Metadata } from 'src/app/shared/classes/metadata'

export interface UserReadDto extends Metadata {

    id: Guid
    username: string
    displayname: string
    email: string
    isFirstFieldFocused: boolean
    isAdmin: boolean
    isActive: boolean

}
