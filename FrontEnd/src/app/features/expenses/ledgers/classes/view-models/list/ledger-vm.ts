import { SimpleEntity } from 'src/app/shared/classes/simple-entity'

export interface LedgerVM {

    id: string
    date: string
    formattedDate: string
    supplier: SimpleEntity
    documentType: SimpleEntity
    hasDocument: boolean
    documentName: boolean
    invoiceNo: string
    debit: number
    credit: number
    balance: number

}
