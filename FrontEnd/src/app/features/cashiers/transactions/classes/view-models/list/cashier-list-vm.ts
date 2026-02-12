import { SimpleEntity } from 'src/app/shared/classes/simple-entity'

export interface CashierListVM {

    id: string
    date: string
    company: SimpleEntity
    safe: SimpleEntity
    remarks: string
    debit: string
    credit: string
    putAt: string
    hasDocument: boolean

}
