import { LedgerDocumentTypeVM } from './ledger-documentType-vm'
import { SimpleEntity } from 'src/app/shared/classes/simple-entity'

export interface LedgerVM {

    date: string
    formattedDate: string
    supplier: SimpleEntity
    documentType: LedgerDocumentTypeVM
    invoiceNo: string
    debit: number
    credit: number
    balance: number

}
