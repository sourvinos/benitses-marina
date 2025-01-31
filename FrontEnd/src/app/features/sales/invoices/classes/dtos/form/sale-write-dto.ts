import { Guid } from 'guid-typescript'
// Custom
import { ItemWriteDto } from './item-write-dto'

export interface SaleWriteDto {

    invoiceId: Guid
    customerId: number
    documentTypeId: number
    paymentMethodId: number
    date: string
    invoiceNo: number
    netAmount: number
    vatPercent: number
    vatAmount: number
    grossAmount: number
    items: ItemWriteDto[]
    remarks: string
    putAt: string

}
