import { Guid } from 'guid-typescript'
// Custom
import { Metadata } from 'src/app/shared/classes/metadata'
import { SimpleEntity } from 'src/app/shared/classes/simple-entity'

export interface InvoiceReadDto extends Metadata {

    expenseId: Guid
    date: string
    company: SimpleEntity
    documentType: SimpleEntity
    paymentMethod: SimpleEntity
    supplier: SimpleEntity
    documentNo: string
    amount: number
    remarks: string
    isDeleted: boolean
    postAt: string
    postUser: string
    putAt: string
    putUser: string

}
