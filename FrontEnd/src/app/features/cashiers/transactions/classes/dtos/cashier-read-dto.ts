import { Guid } from 'guid-typescript'
// Custom
import { Metadata } from 'src/app/shared/classes/metadata'
import { SimpleEntity } from 'src/app/shared/classes/simple-entity'

export interface CashierReadDto extends Metadata {

    cashierId: Guid
    company: SimpleEntity
    discriminatorId: number
    date: string
    amount: number
    remarks: string
    isDeleted: boolean
    postAt: string
    postUser: string
    putAt: string
    putUser: string

}
