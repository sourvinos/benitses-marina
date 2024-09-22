import { Guid } from 'guid-typescript'

export interface ReservationWriteDto {

    reservationId: Guid
    boatTypeId: number
    boatName: string
    length: string
    fromDate: string
    toDate: string
    days: number
    email: string
    remarks: string
    isConfirmed: boolean
    isDocked: boolean
    isPaid: boolean
    putAt: string

}
