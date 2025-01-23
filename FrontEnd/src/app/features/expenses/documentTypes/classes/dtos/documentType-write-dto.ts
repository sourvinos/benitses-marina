export interface DocumentTypeWriteDto {

    id: number
    discriminatorId: number
    abbreviation: string
    description: string
    suppliers: string
    isStatistic: boolean
    isActive: boolean
    putAt: string

}
