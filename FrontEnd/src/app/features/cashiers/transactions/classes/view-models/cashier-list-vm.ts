import { SimpleEntity } from 'src/app/shared/classes/simple-entity'

export interface CashierListVM {

    cashierId: string
    date: string
    company: SimpleEntity
    remarks: string
    amount: number
    putAt: string
    hasDocument: boolean

}
