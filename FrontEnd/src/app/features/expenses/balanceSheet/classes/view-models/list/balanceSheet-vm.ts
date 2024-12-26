import { SimpleEntity } from 'src/app/shared/classes/simple-entity'

export interface BalanceSheetVM {

    supplier: SimpleEntity
    previousBalance: number
    debit: number
    credit: number
    actualBalance: number

}
