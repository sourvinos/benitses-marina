import { Metadata } from 'src/app/shared/classes/metadata'

export interface DocumentTypeReadDto extends Metadata {

    id: number
    discriminatorId: number
    abbreviation: string
    abbreviationEn: string
    abbreviationDataUp: string
    description: string
    batch: string
    customers: string
    isDefault: boolean
    isStatistic: boolean
    isActive: boolean
    postAt: string
    postUser: string
    putAt: string
    putUser: string

}
