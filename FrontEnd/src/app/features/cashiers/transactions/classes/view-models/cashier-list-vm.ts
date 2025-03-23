import { SimpleEntity } from 'src/app/shared/classes/simple-entity'

export interface CashierListVM {

    cashierId: string
    date: string
    company: SimpleEntity
    safe: SimpleEntity
    remarks: string
    isDebit: string
    isCredit: string
    amount: number
    putAt: string
    hasDocument: boolean

}
