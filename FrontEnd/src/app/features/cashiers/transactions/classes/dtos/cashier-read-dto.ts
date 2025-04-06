import { Guid } from 'guid-typescript'
// Custom
import { Metadata } from 'src/app/shared/classes/metadata'
import { SimpleEntity } from 'src/app/shared/classes/simple-entity'

export interface CashierReadDto extends Metadata {

    date: string
    cashierId: string
    company: SimpleEntity
    safe: SimpleEntity
    entry: string
    amount: number
    remarks: string
    isDeleted: boolean
    postAt: string
    postUser: string
    putAt: string
    putUser: string

}
