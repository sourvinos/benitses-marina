import { Metadata } from 'src/app/shared/classes/metadata'

export interface DocumentTypeReadDto extends Metadata {

    id: number
    discriminatorId: number
    abbreviation: string
    description: string
    suppliers: string
    isStatistic: boolean
    isActive: boolean
    postAt: string
    postUser: string
    putAt: string
    putUser: string

}
