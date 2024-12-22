import { SimpleEntity } from 'src/app/shared/classes/simple-entity'

export interface ExpenseListVM {

    id: string
    date: string
    documentType: SimpleEntity
    paymentMethod: SimpleEntity
    supplier: SimpleEntity
    documentNo: string
    amount: number

}
