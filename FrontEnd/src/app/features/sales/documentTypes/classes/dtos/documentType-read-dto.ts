import { Metadata } from 'src/app/shared/classes/metadata'

export interface DocumentTypeReadDto extends Metadata {

    id: number
    discriminatorId: number
    abbreviation: string
    description: string
    batch: string
    customers: string
    isDefault: boolean
    isMyData: boolean
    isStatistic: boolean
    isActive: boolean
    table8_1: string
    table8_8: string
    table8_9: string
    postAt: string
    postUser: string
    putAt: string
    putUser: string

}
