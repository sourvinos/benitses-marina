import { SimpleEntity } from 'src/app/shared/classes/simple-entity'

export interface InvoiceListVM {

    expenseId: string
    date: string
    company: SimpleEntity
    documentType: SimpleEntity
    paymentMethod: SimpleEntity
    supplier: SimpleEntity
    documentNo: string
    amount: number
    hasDocument: boolean

}
