import { Metadata } from 'src/app/shared/classes/metadata'

export interface DocumentTypeReadDto extends Metadata {

    id: number
    discriminatorId: number
    description: string
    customers: string
    suppliers: string
    isStatistic: boolean
    isActive: boolean
    postAt: string
    postUser: string
    putAt: string
    putUser: string

}
