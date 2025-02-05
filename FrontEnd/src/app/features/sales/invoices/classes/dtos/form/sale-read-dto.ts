import { Guid } from 'guid-typescript'
// Custom
import { DocumentTypeVM } from '../../view-models/form/documentType-vm'
import { ItemReadDto } from './item-read-dto'
import { Metadata } from 'src/app/shared/classes/metadata'
import { SimpleEntity } from 'src/app/shared/classes/simple-entity'

export interface SaleReadDto extends Metadata {

    invoiceId: Guid
    date: string
    invoiceNo: number
    customer: SimpleEntity
    documentType: DocumentTypeVM
    paymentMethod: SimpleEntity
    items: ItemReadDto[]
    remarks: string
    isEmailSent: boolean
    isCancelled: boolean
    netAmount: number
    vatPercent: number
    vatAmount: number
    grossAmount: number

}
