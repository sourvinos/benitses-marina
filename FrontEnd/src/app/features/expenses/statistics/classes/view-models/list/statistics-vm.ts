import { SimpleEntity } from 'src/app/shared/classes/simple-entity'

export interface StatisticsVM {

    supplier: SimpleEntity
    previousBalance: number
    debit: number
    credit: number
    periodBalance: number
    actualBalance: number

}
