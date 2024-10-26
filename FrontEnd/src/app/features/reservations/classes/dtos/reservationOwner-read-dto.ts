import { Guid } from 'guid-typescript'

export interface ReservationOwnerReadDto {

    reservationId: Guid
    owner: string
    address: string
    taxNo: string
    taxOffice: string
    passportNo: string
    phones: string
    email: string

}
