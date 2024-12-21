import { SimpleEntity } from 'src/app/shared/classes/simple-entity'

export interface ExpenseListVM {

    id: string
    supplier: SimpleEntity
    documentType: SimpleEntity
    paymentMethod: SimpleEntity
    date: string
    invoiceNo: string
    amount: number

}
