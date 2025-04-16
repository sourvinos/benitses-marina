import { Metadata } from '../../../../../shared/classes/metadata'

export interface HullTypeReadDto extends Metadata {

    id: number
    description: string
    isActive: boolean
    postAt: string
    postUser: string
    putAt: string
    putUser: string

}
