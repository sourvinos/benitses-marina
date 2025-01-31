import { SaleListAadeVM } from './sale-list-aade-vm'
import { SimpleEntity } from '../../../../../../shared/classes/simple-entity'

export interface SaleListVM {

    customer: SimpleEntity
    date: SimpleEntity
    documentType: SimpleEntity
    batch: string
    grossAmount: number
    invoiceId: string
    invoiceNo: number
    formattedDate: string
    isEmailPending: boolean
    isEmailSent: boolean
    ship: SimpleEntity
    shipOwner: SimpleEntity
    aade: SaleListAadeVM

}
