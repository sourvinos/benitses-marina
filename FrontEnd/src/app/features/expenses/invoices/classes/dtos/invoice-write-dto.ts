import { Guid } from 'guid-typescript'

export interface InvoiceWriteDto {

    id: Guid
    supplierId: number
    documentTypeId: number
    paymentMethodId: number
    date: string
    documentNo: string
    amount: number
    putAt: string

}
