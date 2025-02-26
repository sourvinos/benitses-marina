export interface DocumentTypeWriteDto {

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
    putAt: string

}
