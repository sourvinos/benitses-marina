import { Guid } from 'guid-typescript'
// Custom
import { ItemWriteDto } from './item-write-dto'

export interface SaleWriteDto {

    invoiceId: Guid
    date: string
    customerId: number
    documentTypeId: number
    paymentMethodId: number
    invoiceNo: number
    remarks: string
    items: ItemWriteDto[]
    putAt: string

}
