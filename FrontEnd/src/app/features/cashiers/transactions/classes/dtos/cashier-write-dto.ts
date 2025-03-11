import { Guid } from 'guid-typescript'

export interface CashierWriteDto {

    cashierId: Guid
    companyId: number
    discriminatorId: number
    date: string
    amount: number
    remarks: string
    isDeleted: boolean
    putAt: string

}
