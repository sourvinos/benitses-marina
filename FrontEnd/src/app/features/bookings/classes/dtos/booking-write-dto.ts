import { Guid } from 'guid-typescript'

export interface BookingWriteDto {

    bookingId: Guid
    boatTypeId: number
    boatName: string
    boatLength: string
    fromDate: string
    toDate: string
    stayDuration: number
    contactDetails: string
    email: string
    remarks: string
    isConfirmed: boolean
    isDocked: boolean
    isPaid: boolean
    putAt: string

}
