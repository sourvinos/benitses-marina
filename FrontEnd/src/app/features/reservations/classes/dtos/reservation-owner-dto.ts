import { Guid } from 'guid-typescript'

export interface ReservationOwnerDto {

    reservationId: Guid
    name: string
    address: string
    taxNo: string
    taxOffice: string
    passportNo: string
    phones: string
    email: string

}
