export interface DocumentTypeWriteDto {

    id: number
    discriminatorId: number
    description: string
    customers: string
    suppliers: string
    isStatistic: boolean
    isActive: boolean
    putAt: string

}
