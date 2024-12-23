import { SimpleEntity } from 'src/app/shared/classes/simple-entity'

export interface InvoiceListVM {

    id: string
    date: string
    documentType: SimpleEntity
    paymentMethod: SimpleEntity
    supplier: SimpleEntity
    documentNo: string
    amount: number

}
