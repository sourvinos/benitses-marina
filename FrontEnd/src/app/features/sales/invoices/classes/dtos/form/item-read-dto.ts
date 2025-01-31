import { Guid } from 'guid-typescript'

export interface ItemReadDto {

    id: number
    invoiceId: Guid
    code: string
    description: string
    englishDescription: string
    qty: number
    netAmount: number
    vatAmount: number
    grossAmount: number

}
