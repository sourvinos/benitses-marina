import { Guid } from 'guid-typescript'

export interface CashierWriteDto {

    cashierId: Guid
    date: string
    companyId: number
    safeId: number
    entry: string
    amount: number
    remarks: string
    isDeleted: boolean
    putAt: string

}
