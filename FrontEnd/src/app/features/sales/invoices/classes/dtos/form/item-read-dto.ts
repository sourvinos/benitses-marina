import { Guid } from 'guid-typescript'

export interface ItemReadDto {

    id: number
    invoiceId: Guid
    code: string
    description: string
    englishDescription: string
    quantity: number
    netAmount: number
    vatPercent: number
    vatAmount: number
    grossAmount: number

}
