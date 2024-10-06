import { Guid } from 'guid-typescript'
import { PierWriteDto } from './pier-write-dto'

export interface ReservationWriteDto {

    reservationId: Guid
    boatName: string
    customer: string
    loa: string
    fromDate: string
    toDate: string
    days: number
    email: string
    remarks: string
    isConfirmed: boolean
    isDocked: boolean
    isPartiallyPaid: boolean
    isFullyPaid: boolean
    isLongTerm: boolean
    piers: PierWriteDto[]
    putAt: string

}
