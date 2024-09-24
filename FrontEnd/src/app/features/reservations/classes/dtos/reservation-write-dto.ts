import { Guid } from 'guid-typescript'
import { PierWriteDto } from './pier-write-dto'

export interface ReservationWriteDto {

    reservationId: Guid
    boatTypeId: number
    boatName: string
    loa: string
    fromDate: string
    toDate: string
    days: number
    email: string
    remarks: string
    isConfirmed: boolean
    isDocked: boolean
    isPaid: boolean
    piers: PierWriteDto[]
    putAt: string

}
