import { Guid } from 'guid-typescript'
// Custom
import { Metadata } from 'src/app/shared/classes/metadata'
import { SimpleEntity } from 'src/app/shared/classes/simple-entity'

export interface ExpenseReadDto extends Metadata {

    id: Guid
    supplier: SimpleEntity
    documentType: SimpleEntity
    paymentMethod: SimpleEntity
    date: string
    invoiceNo: string
    amount: number
    postAt: string
    postUser: string
    putAt: string
    putUser: string

}
