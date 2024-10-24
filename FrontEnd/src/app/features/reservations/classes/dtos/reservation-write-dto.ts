import { Guid } from 'guid-typescript'
import { BerthWriteDto } from './berth-write-dto'

export interface ReservationWriteDto {

    reservationId: Guid
    boatName: string
    customer: string
    loa: string
    fromDate: string
    toDate: string
    days: number
    email: string
    contact: string
    remarks: string
    financialRemarks: string
    isDocked: boolean
    paymentStatusId: number
    isLongTerm: boolean
    isAthenian: boolean
    berths: BerthWriteDto[]
    putAt: string

}
