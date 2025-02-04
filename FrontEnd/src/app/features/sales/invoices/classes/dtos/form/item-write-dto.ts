import { Guid } from 'guid-typescript'

export interface ItemWriteDto {

    id: number
    invoiceId: Guid
    code: string
    description: string
    englishDescription: string
    qty: number
    netAmount: number
    vatPercent: number
    vatAmount: number
    grossAmount: number

}
