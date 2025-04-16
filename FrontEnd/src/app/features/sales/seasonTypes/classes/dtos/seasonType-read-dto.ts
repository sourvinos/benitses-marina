import { Metadata } from '../../../../../shared/classes/metadata'

export interface SeasonTypeReadDto extends Metadata {

    id: number
    description: string
    isActive: boolean
    postAt: string
    postUser: string
    putAt: string
    putUser: string

}
