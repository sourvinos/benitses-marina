import { Guid } from 'guid-typescript'

export interface InvoiceWriteDto {

    expenseId: Guid
    companyId: number
    supplierId: number
    documentTypeId: number
    paymentMethodId: number
    date: string
    documentNo: string
    amount: number
    remarks: string
    isDeleted: boolean
    putAt: string

}
