import { SimpleEntity } from 'src/app/shared/classes/simple-entity'

export interface BillingPricesVM {

    id: number
    customer: SimpleEntity
    destination: SimpleEntity
    port: SimpleEntity
    from: string
    formattedFrom: string
    to: string
    formattedTo: string
    adultsWithTransfer: number
    adultsWithoutTransfer: number
    kidsWithTransfer: number
    kidsWithoutTransfer: number
}
